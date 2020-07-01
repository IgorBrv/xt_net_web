using System;
using ValidatorLib;
using Outputlib;   // Кастомная библиотека вывода текста в цвете

namespace Lowercase
{   // программу, которая заменяет первую букву первого слова в предложении на заглавную.
	// В качестве окончания предложения можете считать только «.|?|!». 
	class Program
	{
		static void Main(string[] args)
		{
			bool error = false;
			string input = "";
			while (true)
			{
				Output.Print("b", "g", $"\n ПРОГРАММА ЗАМЕНЯЮЩАЯ ПЕРВУЮ БУКВУ В ПРЕДЛОЖЕНИИ НА ЗАГЛАВНУЮ \n");

				if (error)
				{   // Секция отображения ошибки при некорректном вводе
					Output.Print("r", "", "Необходимо ввести фразу для обработки!");
					error = false;
				}
				else if (!error && input.Length > 0)
				{   // Секция обработки ввода 

					Output.Print("b", "c", "", $" ВВЕДЁННАЯ ФРАЗА:".PadRight(62), "");
					Console.WriteLine($"{input}");
					Output.Print("b", "c", "", $" ФРАЗА ПОСЛЕ ОБРАБОТКИ:".PadRight(62), "");
					Console.WriteLine($"{Validator.Fix(input)}");

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
}
