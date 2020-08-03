using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FileManagementSystem
{
	class BackupAgent
	{   // Данный класс является достаточно костыльной ПРОСЛОЙКОЙ между классом-наблюдателем (IntendanceceUI) и классом занимающимся созданием бэкапов (Backup),
		// занимается обработкой запросов формируемых FileSystemWatcherом, отсеканием лишних (дублирующихся) запросов, построением очереди
		// Реализован в последнюю очередь и на скорую руку. Могло быть и лучше, но...
		// Контроль повторных запросов осуществляется посредством ведения списка файлов включающего время последнего изменения каждого файла.

		// Было бы больше времени - в первувю очередь переписал бы этот клас ввиде вменяемой обрабатываемой очереди

		private readonly int creationDelay = 150;			// Интервал времени в мс, в который игнорируются повторные срабатывания для одного файла
		private readonly string backupPath;                 // Путь сохранения состояний
		private readonly Action<string> draw;				// Делегат для передачи сообщений о произведённых бекапах в окно консоли
		private readonly Backup backup;                     // Класс создающий бэкапы

		private readonly Dictionary<string, DateTime>  FilesLastWriteTimeDates;			// Список файлов со штампом времени последнего изменения
		private readonly Dictionary<string, DateTime> DirectoriesCreationTimeDates;     // Список папок со штампом времени даты создания

		public BackupAgent(string workDirectory, Action<string> draw)
		{	// Конструктор класса

			this.draw = draw;
			backup = new Backup(workDirectory);
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

		}

		public async void FileChanged(object source, FileSystemEventArgs e)
		{	// Метод вызываемый при создании, изменении и удалении файлов

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
									draw($" File: {e.FullPath} {e.ChangeType}");
								});
								break;
							case (WatcherChangeTypes.Created):
								await Task.Run(() => {
									backup.FileCreated(e);
									draw($" File: {e.FullPath} {e.ChangeType}");
								});
								break;
							case (WatcherChangeTypes.Deleted):
								backup.FileRemoved(e);
								draw($" File: {e.FullPath} {e.ChangeType}");
								break;
						}
					}
				}
			}
		}

		public async void DirectoryChanged(object source, FileSystemEventArgs e)
		{   // Метод вызываемый при создании, изменении и удалении дирректории

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
									draw($" Directory: {e.FullPath} {e.ChangeType}");
								}
							});
							break;

						case (WatcherChangeTypes.Deleted):
							await Task.Run(() =>
							{
								if (backup.DirectoryRemoved(e))
								{
									draw($" Directory: {e.FullPath} {e.ChangeType}");
								}
							});
							break;
					}
				}
			}
		}

		public void FileDirectoryRenamed(object source, RenamedEventArgs e)
		{
			if (!e.FullPath.Contains(backupPath))
			{
				if (Directory.Exists(e.FullPath))
				{
					if (backup.DirectoryRenamed(e))
					{
						draw($" Directory: {e.OldFullPath} renamed to {e.FullPath}");
					}
				}
				else
				{
					backup.FileRenamed(e);
					draw($" File: {e.OldFullPath} renamed to {e.FullPath}");
				}
			}
		}
	}
}
