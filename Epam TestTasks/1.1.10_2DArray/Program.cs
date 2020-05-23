using System;
using System.Collections.Generic;
using System.Linq;
using Outputlib;   // Кастомная библиотека вывода текста в цвете

namespace Array2D
{  // Определить сумму элементов двумерного массива, стоящих на чётных позициях
	class Program
	{
		static Random rand = new Random();
		static void Main(string[] args)
		{
			while (true)
			{
				Output.Print("b", "g", $"\n ПРОГРАММА ПОДСЧИТЫВАЮЩАЯ СУММУ ЭЛЕМЕНТОВ НАХОДЯЩИХСЯ ПО ЧЁТНЫМ ПОЗИЦИЯМ \n\n");
				Output.Print("b", "c", $" Случайным образом сгенерированый двумерный массив:".PadRight(73));

				int[,] array3D = new int[rand.Next(2, 7), rand.Next(2, 7)];
				MakeSomeMagic(array3D);

				Console.Write("\n\nНажмите ENTER для обновления массива или введите 'exit' для выхода: ");
				string input = Console.ReadLine().Trim().ToLower();
				if (input == "exit") break;
				Console.Clear();
			}
		}

		static void MakeSomeMagic(int[,] array3D)
		{  // Большой и страшный метод заполняющий и одновременно отрисовывающий двухмерный массив
			List<int> numbers = new List<int>();
			Console.WriteLine("\n");
			for (int i = 0; i < array3D.GetLength(0); i++)
			{
				Console.Write(" ");
				for (int j = 0; j < array3D.GetLength(1); j++)
				{
					array3D[i, j] = rand.Next(-99, 100);   // Заполняем ячейку рандомным значением
					Console.Write($"|");
					if ((i + j) % 2 == 0 && (i + j) != 0)  // Если позиция ячейки чётная - меняем цвет вывода и добавляем ячейку в список суммирования
					{
						Console.BackgroundColor = ConsoleColor.Yellow;
						Console.ForegroundColor = ConsoleColor.Black;
						numbers.Add(array3D[i, j]);
					}
					Console.Write($" ({i},{j}) = {array3D[i, j].ToString().PadLeft(3)} ");
					Console.ResetColor();
				}
				Console.WriteLine("|");
			}
			Output.Print("b", "c", "\n\n" + new string(' ', 73));
			Console.WriteLine($"\nСумма элементов массива стоящих на чётных позициях: {numbers.Sum()}\nЭлементы: {string.Join(", ", numbers)}");
		}
	}
}
