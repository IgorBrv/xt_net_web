using System;
using System.Diagnostics;
using System.IO;
using Outputlib;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;

namespace FileManagementSystem
{
	class Program
	{   // Программа занимающаяся логгированием состояний файлов .txt в выбранной папке
		// По задумке:
		// -программа получает уведомления о изменении файлов от FileSystemWatcher
		// -при первом запуске программы производится полный бэкап всех .txt файлов в папке
		// -при удалении/переименовывании файла сохраняется информация о соответствующем действии
		// -при изменении файла - сохраняется изменённый кусок и его адресс или адресс удалённого куска файла
		// -при восстановлении - программа берёт полный бэкап, и проходит по списку последующих бэкапов применяя изменения до удовлетворения нужному состоянию
		//
		// Карта программы:
		// IntendanceUI содержит в себе наблюдатель SystemFileWatcher
		// IntendanceUI обращается в backup через прослойку ввиде backupAgent (для синхронизации обращений) для создания бэкапов
		// Класс backup логгирует изменения или обращается в класс restore для восстановления состояния файла для последующего вычисления изменений в backupHandler
		// IntendanceUI обращается в RestoreUI для отображения меню восстановления бэкапов
		// RestoreUI обращается в Restore для восстановления из выбранного бэкапа
		// Restore обращается в RestoreHandler если бэкап отражает изменения файла. 


		private static readonly string path = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
		private static readonly string configpath = path + @"\config.cfg";
		private static readonly string name = " File Management System";
		private static readonly Draw draw = new Draw(name);
		private static string workDirectory;


		[STAThread]
		static void Main()
		{   // Точка входа приложения

			// Проверка наличия config.cfg:
			if (File.Exists(configpath))
			{	// В случае наличия конфига - из него считывается рабочий каталог:
				try
				{
					workDirectory = File.ReadAllText(configpath);

					if (!Directory.Exists(workDirectory))
					{   // Если записанная в конфиге дирректория не существует - создаётся новый config.cfg:
						CreateConfig();
					}

				}
				catch
				{   // Исключений много - решение одно (потому catch обобщённый):
					CreateConfig();
				}

			}
			else
			{   // В случае отсутствия конфига - создаётся новый config.cfg:
				CreateConfig();
			}


			// Создание объекта рабочего процесса с учётом рабочей дирректории:
			Runtime runtime = new Runtime(name, workDirectory);
			bool exit = false;

			while (!exit)
			{	// Рабочий цикл, производит переключение между процессом обработки, окном выбора точки восстановления, окном выбора папки:
				switch (runtime.Run())
				{
					case (0):	// Завершение работы
						exit = true;
						break;
					case (1):   // Выбор точки восстановления
						RestoreMenu.Show(name, workDirectory);
						runtime = new Runtime(name, workDirectory);
						break;
					case (2):	// Выбор рабочей папки
						CreateConfig();
						runtime = new Runtime(name, workDirectory);
						break;
				}
			}
		}

		private static void CreateConfig()
		{   // Метод создающий новый config.cfg с путём к обрабатываемому каталогу. Выводит на экран окно выбора каталога.

			bool exit = false;

			FolderBrowserDialog fbd = new FolderBrowserDialog
			{	// Создание диалога выбора папки
				Description = "Выберите папку для обработки:",
			};

			Environment.SpecialFolder[] specialFolders = { Environment.SpecialFolder.Windows, Environment.SpecialFolder.ApplicationData, Environment.SpecialFolder.ProgramFiles, Environment.SpecialFolder.ProgramFilesX86, Environment.SpecialFolder.System };
			string systemDisc = string.Join("", Environment.GetFolderPath(specialFolders[0]).Take(2));

			while (!exit)
			{	// Процесс выбора папки

				if (fbd.ShowDialog() == DialogResult.OK)
				{	// Отображение диалога выбора папки

					File.WriteAllText(configpath, fbd.SelectedPath);

					bool dirAvaiable = true;

					// Проверки дирректории на доступность:

					if (fbd.SelectedPath == systemDisc)
					{	// Проверка, не является ли дирректория системным диском:
						dirAvaiable = false;
					}

					foreach (Environment.SpecialFolder sp in specialFolders)
					{   // Проверка, не находится ли дирректория в списке системных папок:
						if (fbd.SelectedPath == Environment.GetFolderPath(sp))
						{
							dirAvaiable = false;
						}
					}

					try
					{	// Избыточная проверка доступности дирректории:

						Directory.GetFiles(fbd.SelectedPath, "*.txt", SearchOption.AllDirectories);
						Directory.GetDirectories(fbd.SelectedPath, "*", SearchOption.AllDirectories);
					}
					catch (UnauthorizedAccessException)
					{
						dirAvaiable = false;
					}

					if (dirAvaiable)
					{
						workDirectory = fbd.SelectedPath;
						exit = true;
					}

				}
				else
				{   // Меню выбора папки, выводится на экран в случае нажатия пользователем кнопки "Отмена" в диалоге выбора папки

					if (workDirectory != null)
					{	// Если ссылка на рабочий каталог уже имеется, отмена выбора файла допускается
						if (Directory.Exists(workDirectory))
						{
							exit = true;
						}
					}
					else
					{	// Иначе выбор папки производится в принудительном порядке:

						string[] strings = { "", "Для начала работы с программой необходимо выбрать папку для наблюдения!", " 0. Выход\n 1. Выбрать папку\n" };
						string input = draw.Form(new int[] { 0, 0, 1 }, strings, true);

						if (input == "0")
						{
							Environment.Exit(0);
						}
					}
				}
			}
		}
	}
}
