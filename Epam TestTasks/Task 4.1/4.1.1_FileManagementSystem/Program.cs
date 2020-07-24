using System;
using System.Diagnostics;
using System.IO;
using Outputlib;
using System.Windows.Forms;


namespace FileManagementSystem
{
	class Program
	{
		private static readonly string path = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
		private static readonly string configpath = path + @"\config.cfg";
		private static readonly string name = " EPAM File Management System";
		private static readonly Draw draw = new Draw(name);
		private static string workDirectory;

		[STAThread]
		static void Main()
		{	// Точка входа приложения

			// Проверка наличия config.cfg:
			if (File.Exists(configpath))
			{	// В случае наличия конфига - из него считывается рабочий каталог:
				workDirectory = File.ReadAllText(configpath);
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

			while (!exit)
			{	// Процесс выбора папки

				if (fbd.ShowDialog() == DialogResult.OK)
				{	// Отображение диалога выбора папки

					File.WriteAllText(configpath, fbd.SelectedPath);
					workDirectory = fbd.SelectedPath;
					exit = true;
				}
				else
				{   // Меню выбора папки, выводится на экран в случае нажатия пользователем кнопки "Отмена" в диалоге выбора папки

					if (workDirectory != null)
					{	// Если ссылка на рабочий каталог уже имеется, отмена выбора файла допускается
						exit = true;
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
