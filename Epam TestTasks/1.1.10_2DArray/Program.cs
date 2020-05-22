using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Array2D
{  // Определить сумму элементов двумерного массива, стоящих на чётных позициях
	class Program
	{
		static Random rand = new Random();
		static void Main(string[] args)
		{
			while (true)
			{
				int[,] array3D = new int[rand.Next(2, 7), rand.Next(2, 7)];
				//int[,] array3D = new int[rand.Next(2, 5), rand.Next(2, 5)];

				Console.BackgroundColor = ConsoleColor.Green;
				Console.ForegroundColor = ConsoleColor.Black;
				Console.WriteLine($"\n ПРОГРАММА ПОДСЧИТЫВАЮЩАЯ СУММУ ЭЛЕМЕНТОВ НАХОДЯЩИХСЯ ПО ЧЁТНЫМ ПОЗИЦИЯМ \n\n");
				Console.ResetColor();

				Console.BackgroundColor = ConsoleColor.Cyan;
				Console.ForegroundColor = ConsoleColor.Black;
				Console.WriteLine($" Случайным образом сгенерированый двумерный массив:".PadRight(73));
				Console.ResetColor();

				MakeSomeMagic(array3D);

				Console.Write("\n\nНажмите ENTER для обновления массива или введите 'exit' для выхода:");
				string input = Console.ReadLine().Trim().ToLower();
				if (input == "exit") break;
				Console.Clear();
			}
		}

		static void MakeSomeMagic(int[,] array3D)
		{  // Большой и страшный метод заполняющий и отрисовывающий двухмерный массив
			List<int> numbers = new List<int>();
			Console.WriteLine("\n");
			for (int i = 0; i < array3D.GetLength(0); i++)
			{
				Console.Write(" ");
				for (int j = 0; j < array3D.GetLength(1); j++)
				{
					array3D[i, j] = rand.Next(-99, 100);
					string value = array3D[i, j].ToString().PadLeft(3);
					Console.Write($"|");
					if ((i + j) % 2 == 0 && (i + j) != 0)
					{
						Console.BackgroundColor = ConsoleColor.Yellow;
						Console.ForegroundColor = ConsoleColor.Black;
						numbers.Add(array3D[i, j]);
					}
					Console.Write($" ({i},{j}) = {value} ");
					Console.ResetColor();
				}
				Console.WriteLine("|");
			}
			Console.BackgroundColor = ConsoleColor.Cyan;
			Console.ForegroundColor = ConsoleColor.Black;
			Console.WriteLine("\n\n" + new string(' ', 73));
			Console.ResetColor();
			Console.WriteLine($"\nСумма элементов, находящихся в ячейках с чётной суммой адресов: {numbers.Sum()}\nЭлементы: {string.Join(", ", numbers)}");
		}
	}
}
