using System;

namespace Outputlib
{   // Данная библиотека разрабатывалась ещё во времена первого задания и может иметь ошибки
	public class Output
	{	// Класс выполняющий отрисовку цветного текста. Принимает следующие параметры ("цвет текста", "цвет фона", "Отрисовка с новой строки (правда/ложь)", "строки для отрисовки через запятую")
		static public void Print(string fnt, string bck, bool write = true, params string[] strings)
		{
			Draw(fnt, bck, write, strings);
		}

		static public void Print(string fnt, string bck, params string[] strings)
		{
			Draw(fnt, bck, true, strings);
		}

		static private void Draw(string fnt, string bck, bool write = true, params string[] strings)
		{
			ConsoleColor font;
			ConsoleColor background;

			switch (fnt)
			{
				case "r":
					font = ConsoleColor.DarkRed; break;
				case "q":
					font = ConsoleColor.Red; break;
				case "y":
					font = ConsoleColor.Yellow; break;
				case "c":
					font = ConsoleColor.Cyan; break;
				case "g":
					font = ConsoleColor.Green; break;
				case "b":
					font = ConsoleColor.Black; break;
				case "o":
					font = ConsoleColor.DarkYellow; break;
				case "s":
					font = ConsoleColor.DarkBlue; break;
				case "v":
					font = ConsoleColor.DarkMagenta; break;
				default:
					font = ConsoleColor.White; break;
			}
			switch (bck)
			{
				case "r":
					background = ConsoleColor.DarkRed; break;
				case "y":
					background = ConsoleColor.Yellow; break;
				case "c":
					background = ConsoleColor.Cyan; break;
				case "g":
					background = ConsoleColor.Green; break;
				case "w":
					background = ConsoleColor.White; break;
				case "o":
					background = ConsoleColor.DarkYellow; break;
				case "s":
					background = ConsoleColor.DarkBlue; break;
				case "v":
					background = ConsoleColor.DarkMagenta; break;
				default:
					background = ConsoleColor.Black; break;
			}

			Console.BackgroundColor = background;
			Console.ForegroundColor = font;

			if (write)
			{
				foreach (string i in strings)
				{
					Console.WriteLine(i);
				}
			}
			else
			{
				foreach (string i in strings)
				{
					Console.Write(i);
				}
			}
			Console.ResetColor();
		}
	}
}
