using System;
using Outputlib;
using CStringLib;


namespace Custom_Paint
{
	static class Draw
	{

		public static string Form(int[] mode, string[] strings, bool error = false)
		{	// Универсальная форма отрисовки пользовательского интерфейса, принимает во входных параметрах режим ввода (числа, строки, количество элементов), строки для отображения запроса.
			bool exit = false;
			string input = "";
			while (!exit)
			{
				Output.Print("b", "g", " ПРОГРАММА CUSTOM PAINT" + (new CString(" ") * 97));  // Использование CString исключительно для примера по заданию 2.1.1**

				if (!error)
				{	// Отрисовка запроса действия
					Output.Print("b", "c", $" {strings[0]}".PadRight(120));
				}
				else
				{   // Отрисовка ошибки в случае неверного ввода
					Output.Print("w", "r", $" {strings[1]}".PadRight(120));
					error = false;
				}

				if (strings.Length > 2)  // Отрисовка дополнительных условий ввода, если они имеются во входных параметрах
				{
					Console.WriteLine(strings[2]);
				}

				Console.Write(" ");
				input = Console.ReadLine().Trim();

				switch (mode[0])
				{	// Обработка ввода, взависимости от выбранного режима

					case 0:     // Первый режим, предполагает ввод целых чисел для выбора пунктов меню, вторым элементом в списке режима является ограничитель количества пунктов.
						if (IsDigits(input))
						{
							int temp = Int32.Parse(input);
							if (temp <= mode[2] && temp >= mode[1])
							{
								exit = true;
							}
							else error = true;
						}
						else error = true;
						break;

					case 1:     // Второй режим, предполагает ввод нескольких целых чисел через запятую, вторым элементом в списке режима является желаемое количество элементов
						input = input.Replace(" ", "");
						string[] splitted = input.Split(',');
						if (splitted.Length == mode[1])
							foreach (string i in splitted)
							{
								if (!IsDigits(i))
								{
									error = true;
								}
								int temp = Int32.Parse(i);
								if (temp <= mode[3] && temp >= mode[2])
								{
									exit = true;
								}
								else error = true;
							}
						else error = true;
						if (!error) exit = true;
						break;

					case 2:     // Третий режим, предполагает ввод нескольких слов через пробел, вторым элементом в списке режима является желаемое количество слов
						splitted = input.Split();
						if (splitted.Length <= mode[1])
							foreach (string i in splitted)
							{
								if (!IsLetters(i))
								{
									error = true;
								}
							}
						else error = true;
						if (!error) exit = true;
						break;

					case 3:     // Третий режим, предполагает вывод информации на экран без проверки ввода
						exit = true;
						break;
				}

				Console.Clear();
			}

			return input;
		}

		private static bool IsLetters(string str)
		{	// Встроенный ввспомогательный метод проверки корректности ввода
			bool result = true;
			if (str == "")
			{
				result = false;
				return result;
			}
			foreach (char i in str.Replace(" ", ""))
			{
				if (!Char.IsLetter(i))
				{
					result = false;
					continue;
				}
			}
			return result;
		}

		private static bool IsDigits(string str)
		{	// Встроенный ввспомогательный метод проверки корректности ввода
			bool result = true;
			if (str == "")
			{
				result = false;
				return result;
			}
			foreach (char i in str.Replace(" ", ""))
			{
				if (!Char.IsNumber(i))
				{
					result = false;
					continue;
				}
			}
			return result;
		}
	}
}
