using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Outputlib;

namespace Averages
{   // Напишите программу, которая определяет среднюю длину слова во введённой текстовой строке.
	// Учтите, что символы пунктуации на длину слов влиять не должны
	class Program
	{
		static Random rand = new Random();
		static void Main(string[] args)
		{
			bool error = false;
			string input = "";
			while (true)
			{
				Output.Print("b", "g", $"\n ПРОГРАММА ПОДСЧИТЫВАЮЩАЯ СРЕДНЮЮ ДЛИНУ СЛОВА ВО ВВЕДЁННОЙ СТРОКЕ \n");
				StringBuilder sb = new StringBuilder();

				if (error)
				{
					Output.Print("r", "", "Необходимо ввести фразу для обработки!");
					error = false;
				}
				else if (!error && input.Length>0)
				{
					Output.Print("b", "c", "", $" ВВЕДЁННАЯ ФРАЗА:".PadRight(66), "");
					Console.WriteLine($"{input}\n");
					int summ = 0;
					int number = 0;
					for (int i = 0; i < input.Length; i++) if (!Char.IsPunctuation(input[i])) sb.Append(input[i]);
					string[] words = sb.ToString().Split();
					for (int i = 0; i < words.Length; i++)
					{
						string word = words[i].Trim();
						if (word != "")
						{
							Console.WriteLine($"{(i + 1).ToString().PadLeft(2)}. Длина: {word.Length.ToString().PadLeft(2)}. Слово: {word}");
							summ += word.Length;
							number++;
						}
					}
					Output.Print("b", "c", "", $" СТАТИСТИКА:".PadRight(66), "");
					Console.WriteLine($"Общее колличество слов: {number}");
					Console.WriteLine("Сумма букв всех слов".PadLeft(22) + $": { summ}");
					Console.WriteLine("Средняя длина слова".PadLeft(22) + $": { Math.Round((double)summ / number)} (Без округления: {(double)summ / number})");
				}
				else Console.WriteLine();

				Console.WriteLine("\nВведите строку для обработки или 'exit' для выхода:\n");
				input = Console.ReadLine();
				if (input == "exit") break;
				else if (input.Trim() == "") error = true;

				Console.Clear();
			}
		}
	}
}
