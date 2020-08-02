using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FileManagementSystem
{
	class Backup
	{   // Принцип сохранения состояний:
		// -При запуске программы сохраняется общий слепок состояния
		// -Последующие изменения в файлах (кроме копирования/переименовывания папки) сохраняются в виде описания изменения и изменённого куска файла (если файл изменён, а не переименован и не удалён)
		// -При переименовывании или копировании целой дирректории - снимается полный слепок состояния (так сделано для упрощения кода, в дальнейшем можно было бы так же развернуть в карту изменений конкретных файлов)

		private readonly Mutex asyncDelay = new Mutex();	// Мьютекс для обеспечения доступа к демпферу  множественных запросов
		private readonly string workDirectory;				// Рабочий каталог
		private readonly string backupPath;					// Путь сохранения состояний
		private bool OperationBlocker = false;				// Метка процесса снятия полного бэкапа
		private int timer;                                  // Таймер обеспечивающий демпфирование множестввенных запросов
		Action<string> draw;

		public Backup(string workDirectory, Action<string> draw)
		{   // Конструктор класса backup.

			this.workDirectory = workDirectory;
			this.backupPath = workDirectory + @"\_backup";
			this.draw = draw;

			if (!Directory.Exists(backupPath))
			{
				Directory.CreateDirectory(backupPath);
			}

			FullBackup();   // Создаём копию текущего состояния в начале работы
		}

		private int Timer
		{	// Таймер для демпфера множественных запросов:
			get => timer;
			set
			{
				asyncDelay.WaitOne();
				{
					timer += value;
				}
				asyncDelay.ReleaseMutex();
			}
		}

		public void FileRemoved(object source, FileSystemEventArgs e)
		{	// Метод логгирующий удаление файла в специальный файл changes, который содержит метку о удалении и адрес удалённого файла

			if (!e.FullPath.Contains(backupPath))
			{
				draw($" File: {e.FullPath} {e.ChangeType}");

				string pathForCurrentBackup = CreateCurBackupDir('D');
				ChangesObject changes = new ChangesObject("remove", e.Name);
				File.WriteAllText($"{pathForCurrentBackup}\\changes", JsonConvert.SerializeObject(changes));
			}
		}

		public async void FileRenamed(object source, RenamedEventArgs e)
		{   // Метод логирующий переименование файла в файл changes, который содержит старое имя, и новое имя

			if (!e.FullPath.Contains(backupPath))
			{
				await Task.Run(() =>
				{   // Задержка с последующей проверкой на выполнение операции полного бэкапа.
					// Необходима, т.к. во время переименовывания или вставки папки параллельно с полным бэкапом запускаются эвенты создания файлов

					Task.Delay(10).Wait();  // Задержка, чтобы процесс полного бэкапа успел запуститься.
					if (!OperationBlocker)
					{
						draw($" File: {e.OldFullPath} renamed to {e.FullPath}");

						string pathForCurrentBackup = CreateCurBackupDir('R');

						ChangesObject changes = new ChangesObject("rename", e.OldName, e.Name);
						File.WriteAllText($"{pathForCurrentBackup}\\changes", JsonConvert.SerializeObject(changes));
					}
				});
			}
		}

		public async void FileChanged(object source, FileSystemEventArgs e)
		{	// Сложный метод логгироввания изменений в файле. Для логирования, в процессе Restore.Restore file файл восстанавливвается от состояния, в котором он находился в полном бекапе
			// до последнего состояния перед изменением, тоесть к восстановленному из последнего полного бекапа файлу применяются все залоггированые изменения. Следом восстановленный
			// файл сравнивается с файлом который был изменён, карта изменений и изменённый кусок выносятся в специальные файлы.

			if (!e.FullPath.Contains(backupPath))
			{
				
				await Task.Run(() =>
				{   // Задержка с последующей проверкой на выполнение операции полного бэкапа.
					// Необходима, т.к. во время переименовывания или вставки папки параллельно с полным бэкапом запускаются эвенты создания файлов

					Task.Delay(10).Wait();  // Задержка, чтобы процесс полного бэкапа успел запуститься.
					if (!OperationBlocker)
					{
						draw($" File: {e.FullPath} {e.ChangeType}");

						string pathForCurrentBackup = CreateCurBackupDir('C');

						FileObject file1 = Restore.RestoreFile(workDirectory, e.Name, string.Join("", pathForCurrentBackup.Skip(pathForCurrentBackup.RFind('\\') + 1)));	// Восстановление файла из бекапа

						BackupHandler.Backup(file1.body, File.ReadAllBytes(e.FullPath), e.Name, pathForCurrentBackup, workDirectory);										// Сравнение файлов и логгирование изменений
					}
				});
			}
		}

		public async void FileCreated(object source, FileSystemEventArgs e)
		{	// Метод создающий отдельный бэкап свежесозданного файла

			if (!e.FullPath.Contains(backupPath))
			{
				
				await Task.Run(() =>
				{   // Задержка с последующей проверкой на выполнение операции полного бэкапа.
					// Необходима, т.к. во время переименовывания или вставки папки параллельно с полным бэкапом запускаются эвенты создания файлов

					Task.Delay(10).Wait();  // Задержка, чтобы процесс полного бэкапа успел запуститься.
					if (!OperationBlocker)
					{
						draw($" File: {e.FullPath} {e.ChangeType}");

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
				});
			}
		}

		public void DirectoryRenamed(object source, RenamedEventArgs e)
		{
			if (!e.FullPath.Contains(backupPath))
			{
				draw($" Directory: {e.OldFullPath} renamed to {e.FullPath}");

				string pathForCurrentBackup = CreateCurBackupDir('M');

				string[] files = Directory.GetFiles(e.FullPath, "*.txt", SearchOption.AllDirectories);

				List<ChangesObject> multipleChanges = new List<ChangesObject>();

				foreach (string file in files)
				{
					multipleChanges.Add(new ChangesObject("rename", file.Replace(e.FullPath, e.OldFullPath).Replace($"{workDirectory}\\", ""), file.Replace($"{workDirectory}\\", "")));
				}

				File.WriteAllText($"{pathForCurrentBackup}\\multipleChanges", JsonConvert.SerializeObject(multipleChanges));
			}
		}

		public async void DirectoryCreated(object source, FileSystemEventArgs e)
		{
			if (!OperationBlocker)
			{
				OperationBlocker = true;
				await Task.Run(() =>
				{
					Timer = 10;
					while (Timer >= 0)
					{
						timer--;
						Task.Delay(1).Wait();
					}

					if (!e.FullPath.Contains(backupPath))
					{
						string[] files = Directory.GetFiles(e.FullPath, "*.txt", SearchOption.AllDirectories).Select(item => item.Replace($"{workDirectory}\\", "")).ToArray();
						if (files.Length != 0)
						{
							draw($" Directory: {e.FullPath} {e.ChangeType}");

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
						}
					}
					OperationBlocker = false;

				});
			}
			else   // Реализация демпфера множественных запросов. Т.к. По какой то причине (причину я забыл на момент написания комментария), 
			{      // иногда вызывается несколько запросов на полный бэкап. В таком случае первый запрос запустит фоновый таймер, а все последующие запросы будут этот таймер прибавлять
				   // Когда запросы закончатся, и таймер дойдёт до нуля - запустится процесс полного бэкапа в единственном экземпляре
				Timer += 10;
			}
		}

		public void DirectoryRemoved(object source, FileSystemEventArgs e)
		{	// Метод создающий бэкап в случае удаления папки

			if (!e.FullPath.Contains(backupPath))
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

				// Из полного бэкапа извлечём список всех сохранённых в нём файлов:
				List<FileObject> removedFiles = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText($@"{fullBackup}\map")).
							Select(KeyValuePair => new FileObject(KeyValuePair.Key, null)).ToList();

				// Пройдёмся по списку бэкапов, и применим все изменения касательно имён и адресов файлов, содержащиеся в нём, к списку файлов:
				Restore.ApplyChangesToFilesList(backupsList, removedFiles, workDirectory, true);

				// В полученном списке файлов на момент до удаления дирректории, удалим все файлы которые не были в удалённой дирректории:
				removedFiles = removedFiles.Where(item => item != null && item.path.Contains(e.Name)).ToList();
				
				// Сохраним список удалённых файлов в соответствующей карте изменений:
				if (removedFiles.Count > 0)
				{
					draw($" Directory: {e.FullPath} {e.ChangeType}");
					string pathForCurrentBackup = CreateCurBackupDir('U');

					List<ChangesObject> ChangesList = removedFiles.Select(item => new ChangesObject("remove", item.path)).ToList();
					File.WriteAllText($"{pathForCurrentBackup}\\multipleChanges", JsonConvert.SerializeObject(ChangesList));
				}
			}
		}


		public async void DirrectoryChanges(object source, FileSystemEventArgs e)
		{   // Метод осуществляющий запуск полного бекапа по сигналу копирования/удаления папки.
			// Реализован демпфер множественных запросов:

			if (!e.FullPath.Contains(backupPath))
			{
				draw($" Directory: {e.FullPath} {e.ChangeType}");

				if (!OperationBlocker)
				{
					OperationBlocker = true;
					await Task.Run(() =>
					{
						Timer = 10;
						while (Timer >= 0)
						{
							timer--;
							Task.Delay(1).Wait();
						}
						FullBackup();
						OperationBlocker = false;
					});
				}
				else   // Реализация демпфера множественных запросов. Т.к. По какой то причине (причину я забыл на момент написания комментария), 
				{      // иногда вызывается несколько запросов на полный бэкап. В таком случае первый запрос запустит фоновый таймер, а все последующие запросы будут этот таймер прибавлять
					   // Когда запросы закончатся, и таймер дойдёт до нуля - запустится процесс полного бэкапа в единственном экземпляре
					Timer += 10;
				}
			}
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

			string pathForCurrentBackup = $@"{backupPath}\{DateTime.Now:[dd.MM.yyyy.HH.mm.ss.fffffff]}[{attr}]";
			Directory.CreateDirectory(pathForCurrentBackup);

			return pathForCurrentBackup;
		}
	}
}
