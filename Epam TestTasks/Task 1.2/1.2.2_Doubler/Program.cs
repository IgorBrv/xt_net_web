using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outputlib;   // Кастомная библиотека вывода текста в цвете

namespace Doubler
{   // Напишите программу, которая удваивает в первой введённой строке все символы, принадлежащие второй введённой строке
	class Program
	{
		static void Main(string[] args)
		{
			bool error = false;
			string input = "";
			string input_two = "";
			while (true)
			{
				Output.Print("b", "g", $"\n ПРОГРАММА УДВАИВАЮЩАЯ В ПЕРВОЙ ВВЕДЁННОЙ СТРОКЕ ВСЕ СИМВОЛЫ ИЗ ВТОРОЙ СТРОКИ \n");

				if (error)
				{   // Секция отображения ошибки при некорректном вводе
					Output.Print("r", "", "Необходимо ввести фразу для обработки и слова для удвоения символов!");
					error = false;
				}
				else if (!error && input.Length > 0)
				{   // Секция обработки ввода 
					Doubler.Fix(input, input_two);
				}
				else Console.WriteLine();

				// Секция ввода
				Console.WriteLine("\nВведите строку для обработки или 'exit' для выхода, затем введите слова для удвоения символов:\n");
				Console.Write("Строка для обработки: ");
				input = Console.ReadLine();
				if (input.Trim().ToLower() == "exit") break;
				else if (input.Trim() == "") { error = true; Console.Clear(); continue; }

				Console.Write("Cлово  для  удвоения: ");
				input_two = Console.ReadLine();
				if (input.Trim() == "" || input_two.Trim() == "") error = true;

				Console.Clear();
			}
		}
	}
	class Doubler
	{
		static public void Fix(string input, string input_two)
		{
			Console.WriteLine($"Фраза   для  обработки: {input}");
			Console.WriteLine($"Источник букв удвоения: {input_two}");
			input_two = input_two.ToLower().Replace(" ", "");
			StringBuilder sb = new StringBuilder();
			foreach (char i in input) // переберём все буквы строки в цикле, добавим их в список обработки необходимое кол-во раз.
			{
				if (input_two.Contains(i.ToString().ToLower())) sb.Append(new string(i, 2));
				else sb.Append(i.ToString());
			}
			Console.WriteLine($"Фраза  после обработки: {sb.ToString()}");
		}
	}
}
