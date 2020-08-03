using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Outputlib;

namespace FileManagementSystem
{
	static class RestoreMenu
	{	// Класс занимающийся отрисовкой меню восстановления файлов из резервных копий.

		public static void Show(string name, string workDirectory)
		{	// Метод занимающийся отрисовкой меню

			bool exit = false;                                      // Индикатор выхода из меню
			List<string> drawList = new List<string>();				// Список бэкапов подготовленный для отрисовки
			List<string> backupList = new List<string>();           // Список сохранённых бэкапов

			Console.CursorVisible = false;

			if (Directory.Exists($"{workDirectory}\\_backup"))
			{   // Создание списка адресов бэкапов, и списка для отрисовки:

				backupList = Directory.GetDirectories($"{workDirectory}\\_backup").Select(item => item.Replace($"{workDirectory}\\_backup\\", "")).OrderByDescending(item => item).ToList();

				foreach (string item in backupList)
				{
					string desc = GetTranscription(item);
					item.RFind('[');
					drawList.Add($" {item.RTakePart(0, item.RFind('.'))} - {desc}");
				}
			}

			if (backupList.Count == 0)
			{
				drawList.Add(" Не найдено сохранённых копий! ");
				backupList.Add(null);
			}

			// Переменные формирующие длину списка и положение индикатора выбранной позиции:

			int pos = 0;
			int page = 21;
			int basesize = 21;

			if (page > backupList.Count())
			{
				page = backupList.Count() - 1;
				basesize = backupList.Count() - 1;
			}

			// Предварительная отрисовка элементов окна:

			Console.Clear();
			Output.Print("b", "g", name.PadRight(120));
			Output.Print("b", "c", " Список сохранённых состояний. Для перемотки используйте клавиши вниз и вверх".PadRight(120));
			Console.SetCursorPosition(0, 27);
			Output.Print("b", "c", $" [ВВЕРХ][ВНИЗ] перемотка{new string(' ', 35)}[ESC] выход{new string(' ', 35)}[ENTER] выбор".PadRight(120));


			while (!exit)
			{   // Цикл отрисовки элементов списка осуществляющий перехват ввода с клавиатуры

				Console.SetCursorPosition(0, 4);

				for (int i = page - basesize; i <= page; i ++)
				{
					if (i == pos)
					{
						Output.Print("b", "w", $" {drawList[i]}");
					}
					else
					{
						Console.WriteLine($"{drawList[i]} ");
					}
				}

				ConsoleKeyInfo key = Console.ReadKey();
				{
					switch (key.Key)
					{
						case (ConsoleKey.UpArrow):
							ListPageUp(ref pos, ref page);
							break;
						case (ConsoleKey.DownArrow):
							ListPageDown(ref pos, ref page, backupList.Count());
							break;
						case (ConsoleKey.Enter):

							if (backupList[0] != null)
							{
								if (!Restore.RestoreState(workDirectory, backupList[pos]))
								{
									drawList[pos] = " ВОССТАНОВЛЕНИЕ НЕ УДАЛОСЬ ";
									backupList[pos] = " ВОССТАНОВЛЕНИЕ НЕ УДАЛОСЬ ";
								}
							}

							break;
						case (ConsoleKey.Escape):
							exit = true;
							break;
					}
				}
			}
			Console.CursorVisible = true;
		}

		private static void ListPageUp(ref int pos, ref int page)
		{	// Метод производящий перемотку списка сохранений вверх
			if (pos > 0)
			{
				pos--;
				if (pos < page - 21)
				{
					if (page - 21 >= 0)
					{
						page--;
					}
				}
			}
		}

		private static void ListPageDown(ref int pos, ref int page, int itemCount)
		{   // Метод производящий перемотку списка сохранений вниз
			if (pos < itemCount-1)
			{
				pos++;
				if (pos > page)
				{
					if (page < itemCount-1)
					{
						page++;
					}
				}
			}
		}

		private static string GetTranscription(string item)
		{   // Вспомогательный метод получения описания бэкапов для формирования списка бэкапов

			int length = 22;

			if (item.EndsWith("[F]")) return "Полная копия".PadRight(length);
			else if (item.EndsWith("[N]")) return "Создание нового файла".PadRight(length);
			else if (item.EndsWith("[D]")) return "Удаление файла".PadRight(length);
			else if (item.EndsWith("[C]")) return "Изменение файла".PadRight(length);
			else if (item.EndsWith("[R]")) return "Переименование файла".PadRight(length);
			else if (item.EndsWith("[U]")) return "Удаление дирректории".PadRight(length);
			else if (item.EndsWith("[I]")) return "Создание дирректории".PadRight(length);
			else if (item.EndsWith("[M]")) return "Переименование дирректории".PadRight(length);

			return default;
		}
	}
}
