using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RectAngle
{ // Написать программу, которая определяет площадь прямоугольника со сторонами a и b. Если пользователь вводит некорректные значения (отрицательные или ноль)
  // должно выдаваться сообщение об ошибке. Возможность ввода пользователем строки вида «абвгд» или нецелых чисел игнорировать.
	class Program
	{
		static void Main(string[] args)
		{
			string str;
			int error = -1;
			string[] input_str;
			List<uint> input_int = new List<uint>();
			Rectangle rectangle = new Rectangle();
			Console.Clear();
			while (true)
			{
				Console.BackgroundColor = ConsoleColor.Red;
				Console.WriteLine("\nВЫЧИСЛЕНИЕ ПЛОЩАДИ ПРЯМОУГОЛЬНИКА\n");
				Console.ResetColor();
				//Введём несколько уточняющих принтов отображающихся в случае некорректного ввода
				if (error == 1)
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("К вводу допускаются целые положительные числа [0 < вводимое число < 4294967295] или фраза 'exit'!!!\n");
					Console.ResetColor();
				}
				else if (error == 2)
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("К вводу допускаются только два значения!!!\n");
					Console.ResetColor();
				}
				else if (error == 0)
				{
					Console.ForegroundColor = ConsoleColor.Cyan;
					Console.WriteLine($"Площадь прямоугольника со сторонами ({rectangle.sides()}) равна: {rectangle.calculateArea()}\n");
					Console.ResetColor();
				}
				Console.WriteLine("\nВведите \"exit\" для выхода ИЛИ стороны прямоугольника через запятую (a, b) для вычисления площади:\n");
				str = Console.ReadLine().Replace(" ", "");
				// Осуществим предварительную проверку ввода на пустую строку или на наличие команды выхода:
				if (str.ToLower() == "exit") { Console.Clear(); break; }
				else if (str == "") { Console.Clear(); continue; }
				// Осуществим дальнейшую обработку ввода:
				else
				{
					input_int.Clear();
					input_str = str.Split(',');
					if (input_str.Length > 2) { error = 2; Console.Clear(); continue; } // Осуществим проверку на количество введённых элементов
					foreach (string i in input_str) // Разобьём строку на подстроки по символу запятой, и попытаемся сконвертировать в int при помощи TryParce
					{
						uint num;
						if (UInt32.TryParse(i, out num) && num > 0) // При удачном конвертировании добавим элемент в список input_int
						{
							input_int.Add(num);
						}
					}

					if (input_int.Count == 2) // При наличии в input_int ровно двух элементов создадим объект "Прямоугольник" и воспользуемся его методом рассчёта площади
					{                         // В данной программе из-за простоты используется всего один объект прямоугольника проинициализированый вначале. 
						if (error != 0) error = 0;
						rectangle.set(input_int);
					}
					else if (input_int.Count < 2) error = 1; // При меньшем количестве элементов в input_int очистим экран и выведем уточняющую критерии ввода надпись
				}
				Console.Clear();
			}

		}
	}

	class Rectangle
	// Класс описывающий объект "Прямоугольник"
	{
		private uint side_a;
		private uint side_b;
		public void set(List<uint> input)
		{
			side_a = input[0];
			side_b = input[1];
		}
		public ulong calculateArea()
		//Метод класса возвращающий площадь прямоугольника
		//Используем ulong на выходе для перекрытия диапазона перемноженных чисел максимальных чисел uint.
		{
			return (ulong)side_a * (ulong)side_b;
		}

		public string sides()
		//Метод класса возвращающий стороны прямоугольника в строковом отображении
		{
			return $"{side_a}, {side_b}";
		}
	}
}
