using System;
using System.Collections.Generic;
using Outputlib;   // Кастомная библиотека вывода текста в цвете


namespace RectAngle
{   // Написать программу, которая определяет площадь прямоугольника со сторонами a и b. Если пользователь вводит некорректные значения (отрицательные или ноль)
    // должно выдаваться сообщение об ошибке. Возможность ввода пользователем строки вида «абвгд» или нецелых чисел игнорировать.
	class Program
	{
		static void Main(string[] args)
		{
			int error = -1;
			List<uint> input_int = new List<uint>();
			Console.Clear();
			while (true)
			{
				Output.Print("b", "g", "\n ВЫЧИСЛЕНИЕ ПЛОЩАДИ ПРЯМОУГОЛЬНИКА \n");
				//Введём несколько уточняющих принтов отображающихся в случае некорректного ввода
				if (error == 1)
				{
					Output.Print("", "r", " К вводу допускаются целые положительные числа [0 < вводимое число < 4294967295] или фраза 'exit'!!! \n");
				}
				else if (error == 2)
				{
					Output.Print("b", "r", " К вводу допускаются только два значения!!! \n");
				}
				else if (error == 0)
				{
					Rectangle rectangle = new Rectangle(input_int);
					Output.Print("b", "c", $" Площадь прямоугольника со сторонами ({rectangle.Sides()}) равна: {rectangle.CalculateArea()} \n");
				}
				else
				{
					Console.WriteLine("\n");
				}

				Console.Write("Введите \"exit\" для выхода ИЛИ стороны прямоугольника через запятую (a, b) для вычисления площади:  ");
				string str = Console.ReadLine().Trim().ToLower();

				// Осуществим предварительную проверку ввода на пустую строку или на наличие команды выхода:
				if      (str == "exit") { Console.Clear(); break;    }
				else if (str == ""    ) { Console.Clear(); continue; }
				else // Осуществим дальнейшую обработку ввода:
				{
					input_int.Clear();
					string[] input_str = str.Split(',');
					if (input_str.Length > 2) { error = 2; Console.Clear(); continue; } // Осуществим проверку на количество введённых элементов
					foreach (string i in input_str) // Разобьём строку на подстроки по символу запятой, и попытаемся сконвертировать в int при помощи TryParce
					{
						uint num;
						if (UInt32.TryParse(i, out num) && num > 0) input_int.Add(num);
					}	// При удачном конвертировании добавим элемент в список input_int

					if (input_int.Count == 2) // При наличии в input_int ровно двух элементов создадим объект "Прямоугольник" и воспользуемся его методом рассчёта площади
					{                         // В данной программе из-за простоты используется всего один объект прямоугольника проинициализированый вначале. 
						if (error != 0) error = 0;
					}
					else if (input_int.Count < 2) error = 1; // При меньшем количестве элементов в input_int очистим экран и выведем уточняющую критерии ввода надпись
				}
				Console.Clear();
			}
		}
	}

	class Rectangle
	{   // Класс описывающий объект "Прямоугольник"
		private uint side_a;
		private uint side_b;
		public Rectangle(List<uint> input)
		{
			side_a = input[0];
			side_b = input[1];
		}
		public ulong CalculateArea() => (ulong)side_a * (ulong)side_b;
		//Метод класса возвращающий площадь прямоугольника

		public string Sides() => $"{side_a}, {side_b}";
		//Метод класса возвращающий стороны прямоугольника в строковом отображении
	}
}
