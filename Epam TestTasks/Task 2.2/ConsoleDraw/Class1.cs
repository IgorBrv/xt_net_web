using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Outputlib;

namespace ConsoleDraw
{
	public class Draw
	// Класс Draw отрисовывает весь текст приложения и принимает ввод. Вся работа с консолью происходит тут.
	{
		private readonly string title;

		public Draw(string title)
		{
			this.title = title;
		}

		public string Form(int[] mode, string[] strings, bool error = false)
		{   // Универсальная форма отрисовки пользовательского интерфейса, принимает во входных параметрах режим ввода (числа, строки, количество элементов), строки для отображения запроса.
			string input = "";
			bool exit = false;
			while (!exit)
			{
				Output.Print("b", "g", title.PadRight(120));

				if (!error)
				{   // Отрисовка запроса действия
					Output.Print("b", "c", $" {strings[0]}".PadRight(120));
				}
				else
				{   // Отрисовка ошибки в случае неверного ввода
					Output.Print("w", "r", $" {strings[1]}".PadRight(120));
					error = false;
				}

				if (strings.Length > 2)  // Отрисовка дополнительных условий ввода, если они имеются во входных параметрах
				{
					string[] splitted = strings[2].Split('\n');
					int maxLength = splitted[0].Length;

					foreach (string str in splitted)
					{
						if (str.Length > maxLength) maxLength = str.Length;
					}

					foreach (string str in splitted)
					{
						if (str.StartsWith("[cl]"))
						{
							string toprint = string.Join("", str.Skip(8));
							Output.Print($"{str[5]}", $"{str[6]}", toprint.PadRight(maxLength - 6));
						}
						else
						{
							Console.WriteLine(str);
						}
					}
				}

				Console.Write(" ");

				input = Console.ReadLine().Trim();

				exit = Input.Get(ref mode, ref error, input);  // Запуск класса проверяющего ввод по заданным условиям

				Console.Clear();
			}
			return input;
		}
	}
}
