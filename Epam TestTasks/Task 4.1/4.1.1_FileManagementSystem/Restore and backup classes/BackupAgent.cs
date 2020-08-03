using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FileManagementSystem
{
	class BackupAgent
	{   // Данный класс является достаточно костыльной ПРОСЛОЙКОЙ между классом-наблюдателем (IntendanceceUI) и классом занимающимся созданием бэкапов (Backup),
		// занимается обработкой запросов формируемых FileSystemWatcherом, отсечением лишних (дублирующихся) запросов, построением очереди
		// Реализован в последнюю очередь и на скорую руку. Могло быть и лучше, но...
		// Контроль повторных запросов осуществляется посредством ведения списка файлов включающего время последнего изменения каждого файла.

		// Было бы больше времени - в первувю очередь переписал бы этот клас ввиде более вменяемой обрабатываемой очереди запросов

		private readonly int backupsBetweenFull = 30;		// Интервал с которым совершается полный бэкап
		private readonly int creationDelay = 150;			// Интервал времени в мс, в который игнорируются повторные срабатывания для одного файла
		private readonly string backupPath;                 // Путь сохранения состояний
		private readonly Action<string> draw;               // Делегат для передачи сообщений о произведённых бекапах в окно консоли
		private DateTime lastfullBackupTime;				// Время последнего полного бэкапа. Используется для синхронизации создания полных юэкапов через интервал времени
		private DateTime lastBackupTime;                    // Время последнего бэкапа. Используется для синхронизации создания полных юэкапов через интервал времени
		private readonly Backup backup;                     // Класс создающий бэкапы
		private int backupsCount = 0;                       // Счётчик бэкапов. Необходим для переодического совершения полного бэкапа
		private bool exit = false;


		private readonly Dictionary<string, DateTime>  FilesLastWriteTimeDates;			// Список файлов со штампом времени последнего изменения
		private readonly Dictionary<string, DateTime> DirectoriesCreationTimeDates;     // Список папок со штампом времени даты создания

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

				FilesLastWriteTimeDates = new Dictionary<string, DateTime>();
				DirectoriesCreationTimeDates = new Dictionary<string, DateTime>() { { workDirectory, Directory.GetCreationTime(workDirectory) } };

				// Занесение в таблицу дат последнего изменения или создания для файлов и папок:
				foreach (string file in Directory.GetFiles(workDirectory, "*.txt", SearchOption.AllDirectories))
				{
					FilesLastWriteTimeDates.Add(file, File.GetLastWriteTime(file));
				}

				foreach (string directory in Directory.GetDirectories(workDirectory, "*", SearchOption.AllDirectories))
				{
					DirectoriesCreationTimeDates.Add(directory, Directory.GetCreationTime(directory));
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
			if (DirectoriesCreationTimeDates.ContainsKey(directory) && DateTime.Now.Subtract(DirectoriesCreationTimeDates[directory]).TotalMilliseconds > creationDelay)
			{
				// Если файл отсутствует в списке файлов - следовательно он новый и создан НЕ вместе с дирректорией, потому добавляем его в список файлов и присваиваем ему метку нового файла
				if (!FilesLastWriteTimeDates.ContainsKey(e.FullPath))
				{
					FilesLastWriteTimeDates.Add(e.FullPath, File.GetLastWriteTime(e.FullPath));
					newfile = true;
				}

				// Если файл является новым, или с момента создания файла прошло более 150мс (Изначально. Интервал меняется в начале стр): передаём информацию о изменении в соответствующие методы класса Backup.
				if (newfile || DateTime.Now.Subtract(FilesLastWriteTimeDates[e.FullPath]).TotalMilliseconds > creationDelay)
				{
					FilesLastWriteTimeDates[e.FullPath] = DateTime.Now;

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
								await Task.Run(() => {
									backup.FileCreated(e);
									draw($" [{DateTime.Now:HH:mm:ss}] File: {e.FullPath} {e.ChangeType}");
									lastBackupTime = DateTime.Now;
									backupsCount++;
								});
								break;
							case (WatcherChangeTypes.Deleted):
								backup.FileRemoved(e);
								draw($" [{DateTime.Now:HH:mm:ss}] File: {e.FullPath} {e.ChangeType}");
								lastBackupTime = DateTime.Now;
								backupsCount++;
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
			if (!DirectoriesCreationTimeDates.ContainsKey(e.FullPath))
			{
				// Добавляем информацию о каталоге в список:
				DirectoriesCreationTimeDates.Add(e.FullPath, Directory.GetCreationTime(e.FullPath));

				// Сканируем каталог на файлы и подкаталоги, и регистрируем их так же:
				foreach (string directory in Directory.GetDirectories(e.FullPath, "*", SearchOption.AllDirectories))
				{
					if (!DirectoriesCreationTimeDates.ContainsKey(directory))
					{
						DirectoriesCreationTimeDates.Add(directory, Directory.GetCreationTime(directory));
					}

				}
				foreach (string file in Directory.GetFiles(e.FullPath, ".txt", SearchOption.AllDirectories))
				{
					if (!FilesLastWriteTimeDates.ContainsKey(file))
					{
						FilesLastWriteTimeDates.Add(file, File.GetLastWriteTime(file));
					}
				}

				// Присваиваем каталогу метку нового каталога, если с момента создания РОДИТЕЛЬСКОГО КАТАЛОГА прошло не более 150мс (Изначально, интервал меняется в начале страницы):
				if (DateTime.Now.Subtract(DirectoriesCreationTimeDates[fatherDirectory]).TotalMilliseconds > creationDelay)
				{
					justCreated = true;
				}
			}

			// Если каталог имеет метку нового каталога, или если с момента создания прошло менее 150мс, передаём информацию о изменении в соответствующие методы класса Backup:
			if (justCreated == true || DateTime.Now.Subtract(DirectoriesCreationTimeDates[e.FullPath]).TotalMilliseconds > creationDelay)
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
	}
}
