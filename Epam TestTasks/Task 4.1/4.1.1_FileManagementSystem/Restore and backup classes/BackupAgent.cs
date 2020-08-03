using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileManagementSystem
{
	class BackupAgent
	{   // Данный класс является достаточно костыльной ПРОСЛОЙКОЙ между классом-наблюдателем (IntendanceceUI) и классом занимающимся созданием бэкапов (Backup),
		// занимается обработкой запросов формируемых FileSystemWatcherом, отсечением лишних (дублирующихся) запросов, построением очереди
		// Реализован в последнюю очередь и на скорую руку. Могло быть и лучше, но...
		// Контроль повторных запросов осуществляется посредством ведения списка файлов включающего время последнего изменения каждого файла.

		// Было бы больше времени - в первувю очередь переписал бы этот клас ввиде более вменяемой обрабатываемой очереди запросов

		private readonly int backupsBetweenFull = 30;		// Интервал с которым совершается полный бэкап
		private readonly int creationDelay = 300;			// Интервал времени в мс, в который игнорируются повторные срабатывания для одного файла
		private readonly string backupPath;                 // Путь сохранения состояний
		private readonly Action<string> draw;               // Делегат для передачи сообщений о произведённых бекапах в окно консоли
		private DateTime lastfullBackupTime;				// Время последнего полного бэкапа. Используется для синхронизации создания полных юэкапов через интервал времени
		private DateTime lastBackupTime;                    // Время последнего бэкапа. Используется для синхронизации создания полных юэкапов через интервал времени
		private readonly Backup backup;                     // Класс создающий бэкапы
		private int backupsCount = 0;                       // Счётчик бэкапов. Необходим для переодического совершения полного бэкапа
		private bool exit = false;

		// Поля относящиеся к демонам группировки создаваемых и удаляемых файлов
		private bool creationDaemonRunning = false;
		private bool removingDaemonRunning = false;
		private readonly int fileCreationNRemovingDelay = 10;
		private readonly List<FileSystemEventArgs> createdFilesList = new List<FileSystemEventArgs>();
		private readonly List<FileSystemEventArgs>  removedFilesList = new List<FileSystemEventArgs>();
		private readonly static object locker = new object();

		private readonly Dictionary<string, DateTime>  filesLastWriteTimeDates;			// Список файлов со штампом времени последнего изменения
		private readonly Dictionary<string, DateTime> directoriesCreationTimeDates;     // Список папок со штампом времени даты создания

		public BackupAgent(string workDirectory, Action<string> draw, Action wrongDirectory)
		{	// Конструктор класса

			this.draw = draw;
			lastBackupTime = DateTime.Now;
			lastfullBackupTime = DateTime.Now;
			backup = new Backup(workDirectory, wrongDirectory);

			while (backup.IsReady == 0)
			{
				Thread.Sleep(50);
			}
			if (backup.IsReady == 1)
			{
				backupPath = backup.GetBackupPath();

				// Формирование списков файлов и папок: 

				filesLastWriteTimeDates = new Dictionary<string, DateTime>();
				directoriesCreationTimeDates = new Dictionary<string, DateTime>() { { workDirectory, Directory.GetCreationTime(workDirectory) } };

				// Занесение в таблицу дат последнего изменения или создания для файлов и папок:
				foreach (string file in Directory.GetFiles(workDirectory, "*.txt", SearchOption.AllDirectories))
				{
					filesLastWriteTimeDates.Add(file, File.GetLastWriteTime(file));
				}

				foreach (string directory in Directory.GetDirectories(workDirectory, "*", SearchOption.AllDirectories))
				{
					directoriesCreationTimeDates.Add(directory, Directory.GetCreationTime(directory));
				}

				// Запустим метод производящий полный бэкап в указанном в полях класса интервале:
				AsyncFullBackupControlDaemon();
			}
		}

		public async void FileChanged(object source, FileSystemEventArgs e)
		{   // Метод вызываемый при создании, изменении и удалении файлов

			// Проверим, не проводился ли полный бэкап только что, и, в случае чего, дадим ему время на завершение операций:
			while (DateTime.Now.Subtract(lastfullBackupTime).TotalMilliseconds < creationDelay)
			{
				Thread.Sleep(100);
			}

			// Вычленяем каталог из пути файла:
			string directory = string.Join("", e.FullPath.Take(e.FullPath.RFind('\\')));
			bool newfile = false;

			// Продолжаем дальше, если каталог, содержащий файл содержится в списке каталогов, и если со времени создания диалога прошло более 150 милисекунд (Изначально. Интервал меняется в начале стр):
			if (directoriesCreationTimeDates.ContainsKey(directory) && DateTime.Now.Subtract(directoriesCreationTimeDates[directory]).TotalMilliseconds > creationDelay)
			{
				// Если файл отсутствует в списке файлов - следовательно он новый и создан НЕ вместе с дирректорией, потому добавляем его в список файлов и присваиваем ему метку нового файла
				if (!filesLastWriteTimeDates.ContainsKey(e.FullPath))
				{
					filesLastWriteTimeDates.Add(e.FullPath, File.GetLastWriteTime(e.FullPath));
					newfile = true;
				}

				// Если файл является новым, или с момента создания файла прошло более 150мс (Изначально. Интервал меняется в начале стр): передаём информацию о изменении в соответствующие методы класса Backup.
				if (newfile || DateTime.Now.Subtract(filesLastWriteTimeDates[e.FullPath]).TotalMilliseconds > creationDelay)
				{
					filesLastWriteTimeDates[e.FullPath] = DateTime.Now;

					if (!e.FullPath.Contains(backupPath))
					{
						switch (e.ChangeType)
						{
							case (WatcherChangeTypes.Changed):
								await Task.Run(() =>
								{
									backup.FileChanged(e);
									draw($" [{DateTime.Now:HH:mm:ss}] File: {e.FullPath} {e.ChangeType}");
									lastBackupTime = DateTime.Now;
									backupsCount++;
								});
								break;
							case (WatcherChangeTypes.Created):
								FileCreationListAccesGate(e);
								FileCreationDaemon();
								break;
							case (WatcherChangeTypes.Deleted):
								FileRemovingListAccesGate(e);
								FileRemovingDaemon();
								break;
						}
					}
				}
			}
		}

		public async void DirectoryChanged(object source, FileSystemEventArgs e)
		{   // Метод вызываемый при создании, изменении и удалении дирректории

			// Проверим, не проводился ли полный бэкап только что, и, в случае чего, дадим ему время на завершение операций:
			while (DateTime.Now.Subtract(lastfullBackupTime).TotalMilliseconds < creationDelay)
			{
				Thread.Sleep(100);
			}

			// Вычленяем адресс родительского каталога из пути файла:
			string fatherDirectory = e.FullPath.Substring(0, e.FullPath.RFind('\\'));
			bool justCreated = false;

			// Если список дат создания каталогов НЕ содержит информацию о каталоге:
			if (!directoriesCreationTimeDates.ContainsKey(e.FullPath))
			{
				// Добавляем информацию о каталоге в список:
				directoriesCreationTimeDates.Add(e.FullPath, Directory.GetCreationTime(e.FullPath));

				// Сканируем каталог на файлы и подкаталоги, и регистрируем их так же:
				foreach (string directory in Directory.GetDirectories(e.FullPath, "*", SearchOption.AllDirectories))
				{
					if (!directoriesCreationTimeDates.ContainsKey(directory))
					{
						directoriesCreationTimeDates.Add(directory, Directory.GetCreationTime(directory));
					}

				}
				foreach (string file in Directory.GetFiles(e.FullPath, ".txt", SearchOption.AllDirectories))
				{
					if (!filesLastWriteTimeDates.ContainsKey(file))
					{
						filesLastWriteTimeDates.Add(file, File.GetLastWriteTime(file));
					}
				}

				// Присваиваем каталогу метку нового каталога, если с момента создания РОДИТЕЛЬСКОГО КАТАЛОГА прошло не более 150мс (Изначально, интервал меняется в начале страницы):
				if (DateTime.Now.Subtract(directoriesCreationTimeDates[fatherDirectory]).TotalMilliseconds > creationDelay)
				{
					justCreated = true;
				}
			}

			// Если каталог имеет метку нового каталога, или если с момента создания прошло менее 150мс, передаём информацию о изменении в соответствующие методы класса Backup:
			if (justCreated == true || DateTime.Now.Subtract(directoriesCreationTimeDates[e.FullPath]).TotalMilliseconds > creationDelay)
			{
				if (!e.FullPath.Contains(backupPath))
				{
					switch (e.ChangeType)
					{
						case (WatcherChangeTypes.Created):

							await Task.Run(() =>
							{
								Task.Delay(500).Wait();
								if (backup.DirectoryCreated(e))
								{
									draw($" [{DateTime.Now:HH:mm:ss}] Directory: {e.FullPath} {e.ChangeType}");
									lastBackupTime = DateTime.Now;
									backupsCount++;
								}
							});
							break;

						case (WatcherChangeTypes.Deleted):
							await Task.Run(() =>
							{
								if (backup.DirectoryRemoved(e))
								{
									draw($" [{DateTime.Now:HH:mm:ss}] Directory: {e.FullPath} {e.ChangeType}");
									lastBackupTime = DateTime.Now;
									backupsCount++;
								}
							});
							break;
					}
				}
			}
		}

		public void FileDirectoryRenamed(object source, RenamedEventArgs e)
		{
			// Проверим, не проводился ли полный бэкап только что, и, в случае чего, дадим ему время на завершение операций:
			while (DateTime.Now.Subtract(lastfullBackupTime).TotalMilliseconds < creationDelay)
			{
				Thread.Sleep(100);
			}

			if (Directory.Exists(e.FullPath))
			{
				directoriesCreationTimeDates.Add(e.FullPath, directoriesCreationTimeDates[e.OldFullPath]);
				directoriesCreationTimeDates.Remove(e.OldFullPath);

				List<string> filesInRenamedDirectory = new List<string>();

				foreach(string file in filesLastWriteTimeDates.Keys)
				{
					if (file.Contains(e.OldFullPath) && file.Replace($"{e.OldFullPath}\\", "").Contains('\\'))
					{
						filesInRenamedDirectory.Add(file);
					}
				}

				foreach(string file in filesInRenamedDirectory)
				{
					filesLastWriteTimeDates.Add(file.Replace(e.OldFullPath, e.FullPath), DateTime.Now);
					filesLastWriteTimeDates.Remove(file);
				}
			}
			else
			{	// Этот кусок кода не работает. Исправлю позже. 
				List<string> renamedFiles = new List<string>(); 
				foreach (string file in filesLastWriteTimeDates.Keys)
				{
					if (file == e.OldFullPath)
					{
						renamedFiles.Add(file);
					}
				}

				foreach (string file in renamedFiles)
				{
					filesLastWriteTimeDates.Add(e.FullPath, DateTime.Now);
					filesLastWriteTimeDates.Remove(e.OldFullPath);
				}
			}

			if (!e.FullPath.Contains(backupPath))
			{
				if (Directory.Exists(e.FullPath))
				{
					if (backup.DirectoryRenamed(e))
					{
						draw($" [{DateTime.Now:HH:mm:ss}] Directory: {e.OldFullPath} renamed to {e.FullPath}");
						lastBackupTime = DateTime.Now;
						backupsCount++;
					}
				}
				else
				{
					backup.FileRenamed(e);
					draw($" [{DateTime.Now:HH:mm:ss}] File: {e.OldFullPath} renamed to {e.FullPath}");
					lastBackupTime = DateTime.Now;
					backupsCount++;
				}
			}
		}

		public void ClosureTime()
		{	// Вспомогательный метод оповещающий фоновый процесс о закрытии
			exit = true;
		}

		private async void AsyncFullBackupControlDaemon()
		{	// Фоновый метод производящий полный бэкап в указанный в классе интервал бэкапов:

			await Task.Run(() =>
			{
				while (!exit)
				{
					if (backupsCount >= backupsBetweenFull)
					{
						while (DateTime.Now.Subtract(lastBackupTime).TotalSeconds < 3)
						{
							Task.Delay(100).Wait();
						}

						lastfullBackupTime = DateTime.Now;

						backup.FullBackup();
						backupsCount = 0;
						draw($" [{DateTime.Now:HH:mm:ss}] Произведено снятие полной резервной копии");

						lastfullBackupTime = DateTime.Now;

					}
					Task.Delay(100).Wait();
				}
			});
		}

		private void FileCreationListAccesGate(FileSystemEventArgs item, bool remove = false)
		{   // Вспомогательнный метод обеспечивающий синхронизацию работы со списком созданнных файлов:
			lock (locker)
			{
				if (remove)
				{
					createdFilesList.Clear();
				}
				else
				{
					createdFilesList.Add(item);
				}
			}
		}

		private void FileRemovingListAccesGate(FileSystemEventArgs item, bool remove = false)
		{   // Вспомогательнный метод обеспечивающий синхронизацию работы со списком созданнных файлов:
			lock (locker)
			{
				if (remove)
				{
					removedFilesList.Clear();
				}
				else
				{
					removedFilesList.Add(item);
				}
			}
		}

		private async void FileRemovingDaemon()
		{   // Инициализатор демона созданния файлов
			if (!removingDaemonRunning)
			{
				removingDaemonRunning = true;
				await Task.Run(() => FileRemovingDaemonBody());
			}
		}

		private async void FileCreationDaemon()
		{   // Инициализатор демона созданния файлов
			if (!creationDaemonRunning)
			{
				creationDaemonRunning = true;
				await Task.Run(() => FileCreationDaemonBody());
			}
		}

		private async void FileCreationDaemonBody()
		{   // Демон создания файлов. Инициализируется при создании одного любого файла, и ждёт установленный лимит времени. При создании в момент ожидания ещё файлов - ожидание продлевается.
			// Передаёт файлы на обработку классу Backup тогда,  когда без поступления новых файлов таймер достигает нуля. Необходим, чтобы логгировать файлы группами, при груповом копировании, например.

			int timer = fileCreationNRemovingDelay;
			int count = createdFilesList.Count;
			while (timer >= 0)
			{   // Таймер
				Task.Delay(1).Wait();
				if (createdFilesList.Count > count)
				{
					timer += fileCreationNRemovingDelay;
					count = createdFilesList.Count;     // Продление в случае поступления новых элементов
				}
				timer--;
			}

			// Таймер отыграл, если в списке один файл - файл подаётся в соответствующий метод. Если группа файлов - в соответствующий.
			if (createdFilesList.Count > 1)
			{
				string path = string.Join("", createdFilesList[0].FullPath.Take(createdFilesList[0].FullPath.RFind('\\')));
				string[] createdFiles = createdFilesList.Select(item => item.Name).ToArray();
				FileCreationListAccesGate(null, true);

				await Task.Run(() =>
				{
					if (backup.DirectoryCreated(null, createdFiles))
					{

						draw($" [{DateTime.Now:HH:mm:ss}] Multiple Changes in {path}");
					}
				});
			}
			else
			{
				await Task.Run(() =>
				{
					backup.FileCreated(createdFilesList[0]);
					draw($" [{DateTime.Now:HH:mm:ss}] File: {createdFilesList[0].FullPath} {createdFilesList[0].ChangeType}");
				});
			}

			creationDaemonRunning = false;
			lastBackupTime = DateTime.Now;
			backupsCount++;
		}

		private async void FileRemovingDaemonBody()
		{   // Демон удаления файлов. Инициализируется при удалении любого файла, и ждёт установленный лимит времени. При удалении в момент ожидания ещё файлов - ожидание продлевается.
			// Передаёт файлы на обработку классу Backup тогда, когда без поступления новых файлов таймер достигает нуля. Необходим, чтобы логгировать файлы группами, при груповом удалении, например.

			int timer = fileCreationNRemovingDelay;
			int count = removedFilesList.Count;
			while (timer >= 0)
			{   // Таймер
				Task.Delay(1).Wait();
				if (removedFilesList.Count > count)
				{
					timer += fileCreationNRemovingDelay;
					count = removedFilesList.Count;         // Продление в случае поступления новых элементов
				}
				timer--;
			}

			// Таймер отыграл, если в списке один файл - файл подаётся в соответствующий метод. Если группа файлов - в соответствующий.
			if (removedFilesList.Count > 1)
			{
				string path = string.Join("", removedFilesList[0].FullPath.Take(removedFilesList[0].FullPath.RFind('\\')));
				List<FileObject> removedFiles = removedFilesList.Select(item => new FileObject(item.Name, null)).ToList();
				FileRemovingListAccesGate(null, true);

				await Task.Run(() =>
				{
					if (backup.DirectoryRemoved(null, removedFiles))
					{
						draw($" [{DateTime.Now:HH:mm:ss}] Multiple Changes in {path}");
					}
				});
			}
			else
			{
				backup.FileRemoved(removedFilesList[0]);
				draw($" [{DateTime.Now:HH:mm:ss}] File: {removedFilesList[0].FullPath} {removedFilesList[0].ChangeType}");
			}

			removingDaemonRunning = false;
			lastBackupTime = DateTime.Now;
			backupsCount++;
		}
	}
}
