using System;
using System.Collections.Generic;
using System.Linq;

namespace Outputlib
{
	public static class NavigationMenu
	{   // Класс создающий интерфейс меню окна консоли с возможностью перемещения по пунктам меню
		//
		// Usage:
		//
		// CreateMenu( ЗАГОЛОВОК ОКНА, ИНФОРМАЦИЯ О СТРАНИЦЕ, LIST СОДЕРЖАЩИЙ СПИСОК ЭЛЕМЕНОВ МЕНЮ, ФЛАГ О ЗАПРЕТЕ/РАЗРЕШЕНИИ УДАЛЕНИЯ, СТАРТОВАЯ ПОЗИЦИЯ, КОЛЛИЧЕСТВО ЗАЩИЩЁННЫХ ОТ УДАЛЕНИЯ ЭЛЕМЕНТОВ МЕНЮ)
		//
		// Возврат: возврат произвводится ввиде массива int из двух элементов. Первый элемент: действие (-1 - выход, 0 - удаление, 1 - выбор). Второй элемент: индекс элемента меню.

		public static int[] CreateMenu(string header, string info, List<string> menuItems, bool allowDeletion = false, int pos = 0, int nonRemovable = -1)
		{
			int page = 21;
			int basesize = 21;
			bool exit = false;
			int selectionLength = 0;

			foreach (string item in menuItems)
			{
				if (item.Length > selectionLength)
				{
					selectionLength = item.Length + 1;
				}
			}

			menuItems = menuItems.Select(item => item.PadRight(selectionLength)).ToList();

			if (page > menuItems.Count())
			{
				page = menuItems.Count() - 1;
				basesize = menuItems.Count() - 1;
			}

			if (pos > menuItems.Count - 1)
			{
				pos = menuItems.Count - 1;
			}

			while (menuItems[pos].Replace(" ", "") == "")
			{
				if (pos < 0)
				{
					break;
				}

				ListPageUp(ref pos, ref page);
			}

			// Предварительная отрисовка элементов окна:

			Console.Clear();
			Output.Print("b", "g", header.PadRight(120));
			Output.Print("b", "c", info.PadRight(120));
			Console.SetCursorPosition(0, 27);
			if (allowDeletion)
			{
				Output.Print("b", "c", $" [ВВЕРХ][ВНИЗ] перемотка{new string(' ', 17)}[ESC] выход{new string(' ', 20)}[DEL] удаление{new string(' ', 20)}[ENTER] выбор".PadRight(120));
			}
			else
			{
				Output.Print("b", "c", $" [ВВЕРХ][ВНИЗ] перемотка{new string(' ', 35)}[ESC] выход{new string(' ', 35)}[ENTER] выбор".PadRight(120));
			}

			while (!exit)
			{
				Console.CursorVisible = false;
				Console.SetCursorPosition(0, 4);

				for (int i = page - basesize; i <= page; i++)
				{
					if (i == pos)
					{
						Output.Print("b", "w", $"  {menuItems[i]}");
					}
					else
					{
						Console.WriteLine($" {menuItems[i]} ");
					}
				}

				ConsoleKeyInfo key = Console.ReadKey();
				{
					switch (key.Key)
					{
						case (ConsoleKey.UpArrow):

							ListPageUp(ref pos, ref page);

							while (menuItems[pos].Replace(" ", "") == "")
							{
								if (pos < 0)
								{
									break;
								}

								ListPageUp(ref pos, ref page);
							}

							break;

						case (ConsoleKey.DownArrow):

							ListPageDown(ref pos, ref page, menuItems.Count());

							while (menuItems[pos].Replace(" ", "") == "")
							{
								if (pos >= menuItems.Count - 1)
								{
									pos = menuItems.Count - 2;
									break;
								}

								ListPageDown(ref pos, ref page, menuItems.Count());
							}

							break;

						case (ConsoleKey.Delete):

							if (pos >= nonRemovable)
							{
								return new int[] { 0, pos };
							}
							break;

						case (ConsoleKey.Enter):

							return new int[] { 1, pos };

						case (ConsoleKey.Escape):

							exit = true;
							break;
					}
				}
			}
			Console.CursorVisible = true;
			return new int[] { -1, -1 };
		}

		private static void ListPageUp(ref int pos, ref int page)
		{   // Метод производящий перемотку списка сохранений вверх
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
			if (pos < itemCount - 1)
			{
				pos++;
				if (pos > page)
				{
					if (page < itemCount - 1)
					{
						page++;
					}
				}
			}
		}
	}
}
