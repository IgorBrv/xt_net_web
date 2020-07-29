using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Outputlib;

namespace FileManagementSystem
{
	static class RestoreMenu
	{	// Меню восстановления файлов из бэкапа

		public static void Show(string name, string path)
		{	// Метод занимающийся отрисовкой меню

			bool exit = false;					// Индикатор выхода из меню
			List<string> backupList;			// Список сохранённых состояния
			Console.CursorVisible = false;

			if (Directory.Exists($"{path}\\_backup"))
			{	// Создание списка сохранённых состояний

				backupList = Directory.GetDirectories($"{path}\\_backup").Select(
					item => item.RTakePart(item.RFind('[') - 1, item.RFindNext('['))).OrderByDescending(item => item).ToList();
			}
			else
			{	
				backupList = new List<string>();
			}

			if (backupList.Count == 0)
			{
				backupList.Add(" Не найдено сохранённых копий! ");
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

			// Цикл отрисовки элементов списка осуществляющий перехват ввода с клавиатуры
			while (!exit)
			{
				Console.SetCursorPosition(0, 4);
				for (int i = page - basesize; i <= page; i ++)
				{
					if (i == pos)
					{
						Output.Print("b", "w", $" {backupList[i]} ");
					}
					else
					{
						Console.WriteLine($"{backupList[i]}  ");
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


	}
}
