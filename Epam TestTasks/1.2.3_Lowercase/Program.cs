using System;
using System.Collections.Generic;
using System.Text;
using Outputlib;   // Кастомная библиотека вывода текста в цвете

namespace Lowercase
{   // Напишите программу, которая считает количество слов, начинающихся с маленькой буквы. 
	class Program
	{
		static void Main(string[] args)
		{
			bool error = false;
			string input = "";
			while (true)
			{
				Output.Print("b", "g", $"\n ПРОГРАММА ПОДСЧИТЫВАЮЩАЯ КОЛЛИЧЕСТВО СЛОВ С МАЛЕНЬКОЙ БУКВЫ В СТРОКЕ \n");

				if (error)
				{   // Секция отображения ошибки при некорректном вводе
					Output.Print("r", "", "Необходимо ввести фразу для обработки!");
					error = false;
				}
				else if (!error && input.Length > 0)
				{   // Секция обработки ввода 
					Lowercase.Check(input);
				}
				else Console.WriteLine();

				// Секция ввода
				Console.WriteLine("\nВведите строку для обработки или 'exit' для выхода:\n");
				input = Console.ReadLine();
				if (input == "exit") break;
				else if (input.Trim() == "") error = true;

				Console.Clear();
			}
		}
	}

	class Lowercase
	{
		static public void Check(string input)
		{
			Output.Print("b", "c", "", $" ВВЕДЁННАЯ ФРАЗА:".PadRight(71), "");
			Console.WriteLine($"{input}");
			StringBuilder sb = new StringBuilder();

			for (int i = 0; i < input.Length; i++)
			{   // удалим из строки все знаки пунктуации:
				if (!Char.IsPunctuation(input[i])) sb.Append(input[i]);
				else sb.Append(' ');
			}

			List<string> from_lowercase = new List<string>();
			List<string> from_uppercase = new List<string>();
			string[] words = sb.ToString().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries); // разобьём строку на массив слов

			for (int i = 0; i < words.Length; i++)
			{   // пройдёмся по полученному массиву, и разнесём слова начинающиеся с мал. и большой букв. по спискам
				if (char.IsLower(words[i][0])) from_lowercase.Add(words[i]);
				else from_uppercase.Add(words[i]);
			}

			Output.Print("b", "c", "", $" СТАТИСТИКА:".PadRight(71), "");
			Console.WriteLine($"Слова, начинающиеся с маленькой буквы [{from_lowercase.Count.ToString().PadLeft(2)}]: {string.Join(", ", from_lowercase)}");
			Console.WriteLine($"Слова, начинающиеся с  большой  буквы [{from_uppercase.Count.ToString().PadLeft(2)}]: {string.Join(", ", from_uppercase)}");
		}
	}
}
