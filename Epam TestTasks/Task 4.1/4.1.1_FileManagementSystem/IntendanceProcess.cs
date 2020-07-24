using Outputlib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Permissions;
using System.Threading;

namespace FileManagementSystem
{
	class Runtime
	{   // Класс в котором происходит наблюдение за изменениями в выбранном каталоге

        private readonly string path;                                      // Путь рабочей дирректории
        private readonly string name;                                      // Имя программы в заголовке
        private readonly List<string> changedList = new List<string>();    // Список последних изменённых .txt файлов

        private int directive = 0;            // Дирректива дальнейших действий возвращаемая функцию main
        private bool exit = false;            // Флаг завершения работы цикла
        private bool refreshNeeded = true;    // Флаг необходимости обновить экран


        public Runtime(string name, string path)
        {   // Конструктор класса Runtime
            this.name = name;
            this.path = path;
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public int Run()
        {
            // Создание экземпляра файлового наблюдателя:
            using (FileSystemWatcher watcher = new FileSystemWatcher())
            {
                watcher.Path = path;
                watcher.IncludeSubdirectories = true;

                // Установка флагов реагирования на изменения:
                watcher.NotifyFilter = NotifyFilters.LastAccess
                                     | NotifyFilters.LastWrite
                                     | NotifyFilters.FileName
                                     | NotifyFilters.DirectoryName;

                // Фильтр файлов, за которыми ведётся наблюдение:
                watcher.Filter = "*.txt";

                // Подписка методов класса на события наблюдателя:
                watcher.Changed += OnChanged;
                watcher.Created += OnChanged;
                watcher.Deleted += OnChanged;
                watcher.Renamed += OnRenamed;

                // Запуск наблюдателя:
                watcher.EnableRaisingEvents = true;

                // Запуск обработки ввода с клавиатуры:
                new Thread(() => Input()).Start();
                Console.CursorVisible = false;

                while (!exit)
                {   // Рабочий цикл наблюдателя

                    if (refreshNeeded)
                    {   // Обновлене содержимого окна после изменения файлов:
                        DrawScreen();
                        refreshNeeded = false;
                    }

                    Thread.Sleep(100);
                }

                // Вовзрат дирректив на дальнейшие действия в метод main
                return directive;

            }
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {   // Обработчик события изменения текстовых файлов:

            ChangedItem($" File: {e.FullPath} {e.ChangeType}");
        }


        private void OnRenamed(object source, RenamedEventArgs e)
        {   // Обработчик события переименовывания текстовых файлов:

            ChangedItem($" File: {e.OldFullPath} renamed to {e.FullPath}");
        }

        private void ChangedItem(string item)
        {   // Менеджер списка последних изменений

            changedList.Add(item);
            if (changedList.Count > 3)
            {
                changedList.RemoveAt(0);
            }
            refreshNeeded = true;
        }

        private void DrawScreen()
        {   // Метод отрисовки содержимого окна:

            Console.Clear();
            Output.Print("b", "g", name.PadRight(120));
            Output.Print("b", "c", " Обработка выбранного каталога: ".PadRight(120));
            Console.WriteLine($" Каталог: {path}\n\n 0. Выход\n 1. Восстановление из базы\n 2. Выбор другого каталога\n");
            Output.Print("b", "c", " Последние зафиксированые изменения:".PadRight(120));
            Console.WriteLine(string.Join("\n", changedList));
        }

        private void Input()
        {   // Фоновый метод обработки ввода с клавиатуры:

            ConsoleKeyInfo key;
            bool inputStop = false;

            while (!inputStop)
            {
                key = Console.ReadKey();

                switch (key.Key)
                {
                    case (ConsoleKey.D0):
                        Console.CursorVisible = true;
                        directive = 0;
                        inputStop = true;
                        exit = true;
                        break;
                    case (ConsoleKey.D1):
                        Console.CursorVisible = true;
                        directive = 1;
                        inputStop = true;
                        exit = true;
                        break;
                    case (ConsoleKey.D2):
                        Console.CursorVisible = true;
                        directive = 2;
                        inputStop = true;
                        exit = true;
                        break;
                    default:
                        refreshNeeded = true;
                        break;
                }

                Thread.Sleep(50);
            }
        }
    }
}
