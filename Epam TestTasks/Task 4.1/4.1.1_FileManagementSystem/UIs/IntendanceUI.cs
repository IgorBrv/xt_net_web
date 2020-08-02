﻿using Outputlib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Permissions;
using System.Threading;

namespace FileManagementSystem
{
	class Runtime
	{   // Класс в котором происходит наблюдение за изменениями в выбранном каталоге

        private readonly string workDirectory;                                      // Путь рабочей дирректории
        private readonly string programName;                                      // Имя программы в заголовке
        private readonly Backup backupAgent;
        private readonly List<string> changesList = new List<string>();    // Список последних изменений

        private int directive = 0;            // Дирректива дальнейших действий возвращаемая функцию main
        private bool exit = false;            // Флаг завершения работы цикла
        private bool refreshNeeded = true;    // Флаг необходимости обновить экран


        public Runtime(string name, string workDirectory)
        {   // Конструктор класса Runtime
            this.programName = name;
            this.workDirectory = workDirectory;
            this.backupAgent = new Backup(workDirectory, AddChangedItemToDrawList);
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public int Run()
        {
            // Создание экземпляра файлового наблюдателя:
            using (FileSystemWatcher watcher = new FileSystemWatcher())
            {


                watcher.Path = workDirectory;
                watcher.IncludeSubdirectories = true;

                // Установка флагов реагирования на изменения:
                watcher.NotifyFilter = NotifyFilters.LastAccess
                                     | NotifyFilters.LastWrite
                                     | NotifyFilters.DirectoryName
                                     | NotifyFilters.FileName;


                // Фильтр файлов, за которыми ведётся наблюдение:
                watcher.Filter = "*.txt";

                // Подписка методов класса на события наблюдателя:
                watcher.Changed += backupAgent.FileChanged;
                watcher.Created += backupAgent.FileCreated;
                watcher.Deleted += backupAgent.FileRemoved;
                watcher.Renamed += backupAgent.FileRenamed;

                // Запуск наблюдателя:
                watcher.EnableRaisingEvents = true;

                // Запуск обработки ввода с клавиатуры:
                new Thread(() => Input()).Start();
                Console.CursorVisible = false;

                // Дополнительно создадим отдельный FileSystemWatcher для наблюдения за переименовыванием и удалением папок
                FileSystemWatcher dirWatcher = new FileSystemWatcher(workDirectory)
                {
                    IncludeSubdirectories = true,
                    NotifyFilter = NotifyFilters.DirectoryName,
                    EnableRaisingEvents = true
                };

                dirWatcher.Deleted += backupAgent.DirectoryRemoved;
                dirWatcher.Renamed += backupAgent.DirectoryRenamed;
                dirWatcher.Created += backupAgent.DirectoryCreated;

                while (!exit)
                {   // Рабочий цикл наблюдателя

                    if (refreshNeeded)
                    {   // Обновлене содержимого окна после изменения файлов:
                        DrawScreen();
                        refreshNeeded = false;
                    }

                    Thread.Sleep(50);
                }

                // выгружаем наблюдателя дирректорий
                dirWatcher.Dispose();

                // Вовзрат дирректив на дальнейшие действия в метод main
                return directive;
            }
        }

        private void AddChangedItemToDrawList(string item)
        {   // Менеджер списка последних изменений

            changesList.Add(item);
            if (changesList.Count > 16)
            {
                changesList.RemoveAt(0);
            }
            refreshNeeded = true;
        }

        private void DrawScreen()
        {   // Метод отрисовки содержимого окна:

            Console.Clear();
            Output.Print("b", "g", programName.PadRight(120));
            Output.Print("b", "c", " Обработка выбранного каталога: ".PadRight(120));
            Console.WriteLine($" Каталог: {workDirectory}\n\n 0. Выход\n 1. Восстановление из базы\n 2. Выбор другого каталога\n");
            Output.Print("b", "c", " Последние зафиксированые изменения:".PadRight(120));
            Console.WriteLine(string.Join("\n", changesList));
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
