using System;
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
					Validator.Fix(input);
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
	class Validator
	{
		static public void Fix(string input)
		{   //first letter of sentence fixer
			Output.Print("b", "c", "", $" ВВЕДЁННАЯ ФРАЗА:".PadRight(62), "");
			Console.WriteLine($"{input}");

			byte count = 0;
			bool key = true;
			char[] str = input.ToCharArray();  // разбиваем строку на массив символов
			for (int i = 0; i < str.Length; i++)
			{   // проходимся по всем символам циклом
				if (str[i] == '.' || str[i] == '?' || str[i] == '!')
				{   // если встречаем знак окончания предложения ?/!/./.../?!
					key = true;
					count = 0;
				};
				if (key)
				{
					if (Char.IsLetter(str[i]))
					{   // начинаем искать, следует ли за ним буква
						str[i] = Char.ToUpper(str[i]);
						count = 0;
						key = false;
					}  // на протяжении трёх последующих символов (Ограничение на глубину поиска можно снять):
					else if (count > 2)
					{
						count = 0;
						key = false;
					}
					else count++;
				}
			}

			Output.Print("b", "c", "", $" ФРАЗА ПОСЛЕ ОБРАБОТКИ:".PadRight(62), "");
			Console.WriteLine($"{string.Join("", str)}");
		}
	}
}
