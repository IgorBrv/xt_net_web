using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FileManagementSystem
{
	class Backup
	{   // Принцип сохранения состояний:
		// -При запуске программы сохраняется общий слепок состояния
		// -Последующие изменения в файлах сохраняются в виде описания изменения и/или карты файла с приложеным изменённым куском файла (если файл изменён, а не переименован и не удалён)

		private readonly string workDirectory;              // Рабочий каталог
		private readonly string backupPath;                 // Путь сохранения состояний
		private readonly Action wrongDirectory;

		public Backup(string workDirectory, Action wrongDirectory)
		{   // Конструктор класса backup.

			this.wrongDirectory = wrongDirectory;
			this.workDirectory = workDirectory;
			this.backupPath = workDirectory + @"\_backup";

			if (!Directory.Exists(backupPath))
			{
				Directory.CreateDirectory(backupPath);
			}

			FullBackup();   // Создаём копию текущего состояния в начале работы
		}

		// Вспомогательное свойство, говорящее о том, что полный бэкап прошёл успешно
		public int IsReady { get; private set; } = 0;

		public string GetBackupPath()
		{	// Вспомогательный метод, возвращающий адресс папки с бэкапом

			return backupPath;
		}

		public void FileRemoved(FileSystemEventArgs e)
		{   // Метод логгирующий удаление файла в специальный файл changes, который содержит метку о удалении и адрес удалённого файла

			string pathForCurrentBackup = CreateCurBackupDir('D');
			ChangesObject changes = new ChangesObject("remove", e.Name);

			File.WriteAllText($"{pathForCurrentBackup}\\changes", JsonConvert.SerializeObject(changes));	// Эксепшн по записи должен всплыть ещё в блоке создания дирректори, где есть блок обработки исключения
		}



		public void FileRenamed(RenamedEventArgs e)
		{   // Метод логирующий переименование файла в файл changes, который содержит старое имя, и новое имя

			string pathForCurrentBackup = CreateCurBackupDir('R');
			ChangesObject changes = new ChangesObject("rename", e.OldName, e.Name);

			File.WriteAllText($"{pathForCurrentBackup}\\changes", JsonConvert.SerializeObject(changes));    // Эксепшн по записи должен всплыть ещё в блоке создания дирректори, где есть блок обработки исключения
		}

		public void FileChanged(FileSystemEventArgs e)
		{   // Сложный метод логгироввания изменений в файле. Для логирования, в процессе Restore.RestoreFile файл восстанавливвается от состояния, в котором он находился в полном бекапе
			// до последнего состояния перед изменением, тоесть к восстановленному из последнего полного бекапа файлу применяются все залоггированые изменения. Следом восстановленный
			// файл сравнивается с файлом который был изменён, карта изменений и изменённый кусок выносятся в специальные файлы-карты.

			string pathForCurrentBackup = CreateCurBackupDir('C');

			FileObject file1 = Restore.RestoreFile(workDirectory, e.Name, string.Join("", pathForCurrentBackup.Skip(pathForCurrentBackup.RFind('\\') + 1)));    // Восстановление файла из бекапа

			if (file1 == null)
			{   // Если не удалось воссоздать файл - подразумеваем что есть какие-то ошибки в цепочке сохранённых состояний - делаем fullBackup с целью предотвратить последующую потерю данных:

				Directory.Delete(pathForCurrentBackup);
				Task.Delay(500).Wait();
				FullBackup();
			}
			else
			{
				BackupHandler.Backup(file1.body, File.ReadAllBytes(e.FullPath), e.Name, pathForCurrentBackup, workDirectory);                                       // Сравнение файлов и логгирование изменений
			}
		}

		public void FileCreated(FileSystemEventArgs e)
		{   // Метод создающий отдельный бэкап свежесозданного файла

			string pathForCurrentBackup = CreateCurBackupDir('N');
			string pathOfBackupForLog = pathForCurrentBackup.Replace($"{workDirectory}\\", "");

			string name = e.Name;

			if (name.Contains('\\'))
			{
				name = string.Join("", name.Skip(name.RFind('\\')));
			}

			string nameForBackup = $"{pathOfBackupForLog}\\{name.Replace(".txt", ".bak")}";
			Dictionary<string, string> map = new Dictionary<string, string> { { e.Name, nameForBackup } };
			File.WriteAllText($"{pathForCurrentBackup}\\map", JsonConvert.SerializeObject(map));    // Эксепшн по записи должен всплыть ещё в блоке создания дирректори, где есть блок обработки исключения

			try
			{
				File.Copy(e.FullPath, $"{workDirectory}\\{nameForBackup}");
			}
			catch (FileNotFoundException)
			{	// В случае невозможности создания резервной копии файла (может быть в случае, еслии файл создан и тут же удалён например), создаём полный бэкап, с целью не потерять возможность восстановления последующих состояний:

				Directory.Delete(pathForCurrentBackup);
				Task.Delay(500).Wait();
				FullBackup();
			}
		}

		public bool DirectoryRenamed(RenamedEventArgs e)
		{	// Метод логгирующий замену имени дирректории:

			// Создаём каталог для бэкапа и получаем список файлов дирректории:
			string pathForCurrentBackup = CreateCurBackupDir('M');

			string[] files = Directory.GetFiles(e.FullPath, "*.txt", SearchOption.AllDirectories);

			if (files.Length != 0)
			{
				List<ChangesObject> multipleChanges = new List<ChangesObject>();

				// Создаём карту изменений используя данные о старом имени каталога от файлсистемвотчера:
				foreach (string file in files)
				{
					multipleChanges.Add(new ChangesObject("rename", file.Replace(e.FullPath, e.OldFullPath).Replace($"{workDirectory}\\", ""), file.Replace($"{workDirectory}\\", "")));
				}

				File.WriteAllText($"{pathForCurrentBackup}\\multipleChanges", JsonConvert.SerializeObject(multipleChanges));    // Эксепшн по записи должен всплыть ещё в блоке создания дирректори, где есть блок обработки исключения

				return true;
			}
			return false;

		}

		public bool DirectoryCreated(FileSystemEventArgs e, string[] files = null)
		{	// Метод создающий отдельную резервную копию создаваемой дирректории:

			// Получаем список файлов дирректории:
			if (files == null)
			{
				files = Directory.GetFiles(e.FullPath, "*.txt", SearchOption.AllDirectories).Select(item => item.Replace($"{workDirectory}\\", "")).ToArray();
			}

			// Если дирректория не является пустой:
			if (files.Length != 0)
			{

				// Создаём каталог для бэкапов и карту бэкапа, переносим файлы в каталог для бекапа:
				string pathForCurrentBackup = CreateCurBackupDir('I');
				string pathOfBackupForLog = pathForCurrentBackup.Replace($"{workDirectory}\\", "");
				Dictionary<string, string> map = new Dictionary<string, string>();

				int count = 0;

				try
				{
					foreach (string file in files)
					{
						string name = file;

						if (name.Contains('\\'))
						{
							name = string.Join("", name.Skip(name.RFind('\\') + 1));
						}

						string nameForBackup = $"{pathOfBackupForLog}\\[{count}].{name.Replace(".txt", ".bak")}";
						File.Copy($"{workDirectory}\\{file}", $"{workDirectory}\\{nameForBackup}");
						map.Add(file, nameForBackup);

						count++;
					}

					File.WriteAllText($"{pathForCurrentBackup}\\map", JsonConvert.SerializeObject(map));

					return true;
				}
				catch (IOException)
				{   // В случае невозможности создания резервной копии дирректории (может быть в случае, еслии дирректория создана и тут же удалеа например), создаём полный бэкап, с целью не потерять возможность восстановления последующих состояний:

					Directory.Delete(pathForCurrentBackup);
					Task.Delay(500).Wait();
					FullBackup();
					return false;
				}
			}
			return false;
		}

		public bool DirectoryRemoved(FileSystemEventArgs e, List<FileObject> removedFiles = null)
		{   // Метод создающий бэкап в случае удаления папки
			if (removedFiles == null)
			{
				List<string> backupsList = new List<string>();
				string fullBackup = default;

				// Просканируем папку с бэкапами, и сохраним адреса всех бэкапов до нахождения последнего полного бэкапа, ссылку на который сохраним отдельно
				foreach (string backup in Directory.GetDirectories(backupPath).OrderByDescending(item => item))
				{
					if (backup.EndsWith("[F]"))
					{
						fullBackup = backup;
						break;
					}
					else if (!backup.EndsWith("[F]") && !backup.EndsWith("[C]"))
					{
						backupsList.Insert(0, backup);
					}
				}

				// Удостоверимся, что полный бэкап не является ссылкой на другой полный бэкап:
				if (Directory.GetFiles(fullBackup).Contains($"{fullBackup}\\flink"))
				{
					while (!Directory.GetFiles(fullBackup).Contains($"{fullBackup}\\map"))
					{
						fullBackup = $"{workDirectory}\\{File.ReadAllText($"{fullBackup}\\flink")}";
					}
				}

				// Из полного бэкапа извлечём список всех сохранённых в нём файлов:
				removedFiles = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText($@"{fullBackup}\map")).
							Select(KeyValuePair => new FileObject(KeyValuePair.Key, null)).ToList();

				// Пройдёмся по списку бэкапов, и применим все изменения касательно имён и адресов файлов, содержащиеся в нём, к списку файлов:
				try
				{
					Restore.ApplyChangesToFilesList(backupsList, removedFiles, workDirectory, true);
				}
				catch (FileNotFoundException)
				{
					Task.Delay(500).Wait();
					FullBackup();
					return false;
				}

				// В полученном списке файлов на момент до удаления дирректории, удалим все файлы которые не были в удалённой дирректории:
				removedFiles = removedFiles.Where(item => item != null && item.path.StartsWith($"{e.Name}\\")).ToList();
			}

			// Сохраним список удалённых файлов в соответствующей карте изменений:
			if (removedFiles.Count > 0)
			{
				string pathForCurrentBackup = CreateCurBackupDir('U');

				List<ChangesObject> ChangesList = removedFiles.Select(item => new ChangesObject("remove", item.path)).ToList();
				File.WriteAllText($"{pathForCurrentBackup}\\multipleChanges", JsonConvert.SerializeObject(ChangesList));

				return true;
			}
			return false;
		}


		public void FullBackup()
		{   // Метод осуществляющий полный бэкап заданной дирректории

			// Получаем список файлов дирректории
			string[] files = default;
			try
			{	// Избыточная проверка на валидность директории
				IsReady = 0;

				files = Directory.GetFiles(workDirectory, "*.txt", SearchOption.AllDirectories);

				// Получаем имя каталога последнего бэкапа и последннего ПОЛННОГО бэкапа:
				string lastBackup = Directory.GetDirectories(backupPath).OrderByDescending(item => item).FirstOrDefault();
				string lastFullBackup = Directory.GetDirectories(backupPath).Where(item => item.EndsWith("[F]")).OrderByDescending(item => item).FirstOrDefault();

				// Проверяем, есть ли изменения в файлах со времён последнего полного бекапа для определения необходимости полного бэкапа:
				bool backupNessesary = BackupNecessary(lastFullBackup, files);

				if (backupNessesary)
				{
					int count = 0;
					Dictionary<string, string> Map = new Dictionary<string, string>();

					// Создаём каталог для помещения текущей копии и карту файлов в копии:
					string pathForCurrentBackup = CreateCurBackupDir('F');

					foreach (string file in files)
					{   // Копируем файлы в каталог копии состояния, добавляя изначальное расположение в карту:

						string newname = $@"{pathForCurrentBackup}\{count}.{string.Join("", file.Skip(file.RFind('\\') + 1)).Replace(".txt", ".bak")}";
						File.Copy(file, newname);
						Map.Add(file.Replace($"{workDirectory}\\", ""), newname.Replace($"{workDirectory}\\", ""));
						count++;
					}

					// Сериализуем карту в файл в дирректорию бэкапа:
					File.WriteAllText($"{pathForCurrentBackup}\\map", JsonConvert.SerializeObject(Map));
				}
				else if (!lastBackup.EndsWith("[F]") && !backupNessesary)
				{   // Если отличий в файлах от последнего ПОЛНОГО бэкапа нет, и последним бэкапом НЕ является ПОЛНЫЙ бэкап - оставим ссылку на последний полный бэкап

					string pathForCurrentBackup = CreateCurBackupDir('F');

					// Сериализуем ссылку в файл в дирректорию бэкапа:
					File.WriteAllText($"{pathForCurrentBackup}\\flink", lastFullBackup.Replace($"{workDirectory}\\", ""));
				}

				IsReady = 1;
			}
			catch (UnauthorizedAccessException)
			{
				wrongDirectory();
				IsReady = -1;
			}
		}

		private bool BackupNecessary(string lastFullBackup, string[] files)
		{	// Метод проверябщий изменения со времён последнего полного бекапа с целью проверки необходимости полного бекапа

			if (lastFullBackup == null)
			{
				return true;
			}

			if (Directory.GetFiles(lastFullBackup).Contains($"{lastFullBackup}\\flink"))
			{
				while (!Directory.GetFiles(lastFullBackup).Contains($"{lastFullBackup}\\map"))
				{
					lastFullBackup = $"{workDirectory}\\{File.ReadAllText($"{lastFullBackup}\\flink")}";
				}
			}

			// Получаем карту последнего бекапа десериализируя её из сопроводительного файла:
			Dictionary<string, string> lastFullBackupMap = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText($"{lastFullBackup}\\map"));

			int count = 0;

			if (files.Length != lastFullBackupMap.Count)
			{	// Проводим предварительную проверку на общее колличество элементов, с целью сэкономить время на более ресурсоёмких операциях вычисления кеша

				return true;
			}

			foreach (string str in files)
			{   // Сравнениваем  файлы с резервными копиями подсчитывая их MD5 кеш:

				string strName = str.Replace($"{workDirectory}\\", "");

				if (lastFullBackupMap.Keys.Contains(strName))
				{	// Получение MD5-хэша:
					string file1md5 = MD5Calc.GetMd5(File.ReadAllBytes(str));
					string file2md5 = MD5Calc.GetMd5(File.ReadAllBytes($"{workDirectory}\\{lastFullBackupMap[strName]}"));

					if (file1md5 != file2md5)
					{	// Сравнение MD5-хеша. Полный бекап необходим, если есть разница.
						return true;
					}

					count++;
				}
				else
				{	// Полный бекап необходим, если в текущем состоянии есть файлы не отражённые в бекапе

					return true;
				}
			}

			if (count != lastFullBackupMap.Count)
			{   // Полный бекап необходим, если в текущем состоянии есть файлы не отражённые в бекапе
				return true;
			}

			return false;
		}

		private string CreateCurBackupDir(char attr)
		{   // Вспомогательный метод создающий каталог для сохранения текущих изменений с соответствующим атрибутом

			string pathForCurrentBackup = $@"{backupPath}\[{DateTime.Now:dd.MM.yyyy.HH.mm.ss.fffffff}{$"{Guid.NewGuid()}".PadLeft(4, '0')}][{attr}]";
			try
			{
				Directory.CreateDirectory(pathForCurrentBackup);
				return pathForCurrentBackup;
			}
			catch (UnauthorizedAccessException)
			{   // Если при одновременном изменении 2х файлов сгенерируется один GUID повторим операцию:

				pathForCurrentBackup = $@"{backupPath}\[{DateTime.Now:dd.MM.yyyy.HH.mm.ss.fffffff}{$"{Guid.NewGuid()}".PadLeft(4, '0')}][{attr}]";
				Directory.CreateDirectory(pathForCurrentBackup);
				return pathForCurrentBackup;
			}
		}
	}
}
