using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FileManagementSystem.Accessory;
using Newtonsoft.Json;

namespace FileManagementSystem
{
	class Backup
	{   // Принцип сохранения состояний:
		// -При запуске программы сохраняется общий слепок состояния
		// -Последующие изменения в файлах сохраняются в виде описания изменения и/или карты файла с приложеным изменённым куском файла (если файл изменён, а не переименован и не удалён)

		private readonly string workDirectory;				// Рабочий каталог
		private readonly string backupPath;					// Путь сохранения состояний

		public Backup(string workDirectory)
		{   // Конструктор класса backup.

			this.workDirectory = workDirectory;
			this.backupPath = workDirectory + @"\_backup";

			if (!Directory.Exists(backupPath))
			{
				Directory.CreateDirectory(backupPath);
			}

			FullBackup();   // Создаём копию текущего состояния в начале работы
		}

		public string GetBackupPath()
		{	// Вспомогательный метод, возвращающий адресс папки с бэкапом

			return backupPath;
		}

		public void FileRemoved(FileSystemEventArgs e)
		{   // Метод логгирующий удаление файла в специальный файл changes, который содержит метку о удалении и адрес удалённого файла

			string pathForCurrentBackup = CreateCurBackupDir('D');
			ChangesObject changes = new ChangesObject("remove", e.Name);

			File.WriteAllText($"{pathForCurrentBackup}\\changes", JsonConvert.SerializeObject(changes));
		}



		public void FileRenamed(RenamedEventArgs e)
		{   // Метод логирующий переименование файла в файл changes, который содержит старое имя, и новое имя

			string pathForCurrentBackup = CreateCurBackupDir('R');
			ChangesObject changes = new ChangesObject("rename", e.OldName, e.Name);

			File.WriteAllText($"{pathForCurrentBackup}\\changes", JsonConvert.SerializeObject(changes));
		}

		public void FileChanged(FileSystemEventArgs e)
		{   // Сложный метод логгироввания изменений в файле. Для логирования, в процессе Restore.RestoreFile файл восстанавливвается от состояния, в котором он находился в полном бекапе
			// до последнего состояния перед изменением, тоесть к восстановленному из последнего полного бекапа файлу применяются все залоггированые изменения. Следом восстановленный
			// файл сравнивается с файлом который был изменён, карта изменений и изменённый кусок выносятся в специальные файлы-карты.

			string pathForCurrentBackup = CreateCurBackupDir('C');

			FileObject file1 = Restore.RestoreFile(workDirectory, e.Name, string.Join("", pathForCurrentBackup.Skip(pathForCurrentBackup.RFind('\\') + 1)));    // Восстановление файла из бекапа

			BackupHandler.Backup(file1.body, File.ReadAllBytes(e.FullPath), e.Name, pathForCurrentBackup, workDirectory);                                       // Сравнение файлов и логгирование изменений
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
			File.WriteAllText($"{pathForCurrentBackup}\\map", JsonConvert.SerializeObject(map));
			File.Copy(e.FullPath, $"{workDirectory}\\{nameForBackup}");
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

				File.WriteAllText($"{pathForCurrentBackup}\\multipleChanges", JsonConvert.SerializeObject(multipleChanges));

				return true;
			}
			return false;

		}

		public bool DirectoryCreated(FileSystemEventArgs e)
		{	// Метод создающий отдельную резервную копию создаваемой дирректории:

			// Получаем список файлов дирректории:
			string[] files = Directory.GetFiles(e.FullPath, "*.txt", SearchOption.AllDirectories).Select(item => item.Replace($"{workDirectory}\\", "")).ToArray();


			// Если дирректория не является пустой:
			if (files.Length != 0)
			{

				// Создаём каталог для бэкапов и карту бэкапа, переносим файлы в каталог для бекапа:
				string pathForCurrentBackup = CreateCurBackupDir('I');
				string pathOfBackupForLog = pathForCurrentBackup.Replace($"{workDirectory}\\", "");
				Dictionary<string, string> map = new Dictionary<string, string>();

				int count = 0;

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
			return false;
		}

		public bool DirectoryRemoved(FileSystemEventArgs e)
		{   // Метод создающий бэкап в случае удаления папки

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

			// Из полного бэкапа извлечём список всех сохранённых в нём файлов:
			List<FileObject> removedFiles = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText($@"{fullBackup}\map")).
						Select(KeyValuePair => new FileObject(KeyValuePair.Key, null)).ToList();

			// Пройдёмся по списку бэкапов, и применим все изменения касательно имён и адресов файлов, содержащиеся в нём, к списку файлов:
			Restore.ApplyChangesToFilesList(backupsList, removedFiles, workDirectory, true);

			// В полученном списке файлов на момент до удаления дирректории, удалим все файлы которые не были в удалённой дирректории:
			removedFiles = removedFiles.Where(item => item != null && item.path.Contains($"{e.Name}\\")).ToList();

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


		private void FullBackup()
		{   // Метод осуществляющий полный бэкап заданной дирректории

			// Получаем список файлов дирректории
			string[] files = Directory.GetFiles(workDirectory, "*.txt", SearchOption.AllDirectories);

			// Проверяем, есть ли изменения в файлах со времён последнего полного бекапа для определения необходимости полного бэкапа:
			if (BackupNecessary(files))
			{
				// Создаём каталог для помещения текущей копии и карту файлов в копии:
				string pathForCurrentBackup = CreateCurBackupDir('F');

				int count = 0;
				Dictionary<string, string> Map = new Dictionary<string, string>();

				foreach (string file in files)
				{	// Копируем файлы в каталог копии состояния, добавляя изначальное расположение в карту:

					string newname = $@"{pathForCurrentBackup}\{count}.{string.Join("", file.Skip(file.RFind('\\') + 1)).Replace(".txt", ".bak")}";
					File.Copy(file, newname);
					Map.Add(file.Replace($"{workDirectory}\\", ""), newname.Replace($"{workDirectory}\\", ""));
					count++;
				}

				// Сериализуем карту в файл в дирректорию копии состояния:
				File.WriteAllText($"{pathForCurrentBackup}\\map", JsonConvert.SerializeObject(Map));
			}
		}

		private bool BackupNecessary(string[] files)
		{	// Метод проверябщий изменения со времён последнего полного бекапа с целью проверки необходимости полного бекапа

			// Получаем имя каталога последнего бэкапа:
			string lastFullBackup = Directory.GetDirectories(backupPath).Where(item => item.Contains("[F]")).OrderByDescending(item => item).FirstOrDefault();

			if (lastFullBackup == null)
			{
				return true;
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
		{	// Вспомогательный метод создающий каталог для сохранения текущих изменений с соответствующим атрибутом

			string pathForCurrentBackup = $@"{backupPath}\[{DateTime.Now:dd.MM.yyyy.HH.mm.ss.fffffff}{$"{StaticRandom.Get(1000)}".PadLeft(4, '0')}][{attr}]";
			Directory.CreateDirectory(pathForCurrentBackup);

			return pathForCurrentBackup;
		}
	}
}
