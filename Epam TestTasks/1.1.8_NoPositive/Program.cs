using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NoPositive
{  // Написать программу, которая заменяет все положительные элементы в трёхмерном массиве на
   // нули. Число элементов в массиве и их тип определяются разработчиком.
	class Program
	{
		static void Main(string[] args)
		{
			Random rand = new Random();
			bool positve = false;
			while (true)
			{
				int[,,] array3D = new int[rand.Next(2, 5), rand.Next(2, 5), rand.Next(2, 5)];
				//int[,,] array3D = new int[,,] { { { 1, 2, 3 }, { 4, 5, 6 } }, { { 7, 8, 9 }, { 10, 11, 12 } }, { { 13, 14, 15 }, { 16, 17, 18 } } };
				ArrayTools3D artools = new ArrayTools3D(array3D);

				artools.RandomFill();  // Заполнение массива рандомными цифрами

				Console.BackgroundColor = ConsoleColor.Green;
				Console.ForegroundColor = ConsoleColor.Black;
				Console.WriteLine($"\n ПРОГРАММА, КОТОРАЯ ЗАМЕНЯЕТ ВСЕ ПОЛОЖИТЕЛЬНЫЕ ЭЛЕМЕНТЫ В ТРЁХМЕРНОМ МАССИВЕ НА НУЛИ \n\n"); ; ;
				Console.ResetColor();

				if (positve)
				{
					artools.Draw("СЛУЧАЙНЫЙ ТРЁХМЕРНЫЙ МАССИВ ЧИСЕЛ");
					artools.RemovePositive();
					artools.Draw("ТРЁХМЕРНЫЙ МАССИВ БЕЗ ПОЛОЖИТЕЛЬНЫХ ЧИСЕЛ");
				}
				else
				{
					artools.Draw("СЛУЧАЙНЫЙ ТРЁХМЕРНЫЙ МАССИВ ЧИСЕЛ");
					artools.RemoveNegative();
					artools.Draw("ТРЁХМЕРНЫЙ МАССИВ БЕЗ ОТРИЦАТЕЛЬНЫХ ЧИСЕЛ");
				}

				Console.Write("\n\nНажмите ENTER для обновления массива,'exit' для выхода, '1' для смены режима: ");
				string input = Console.ReadLine().Trim().ToLower();
				if (input == "exit") break;
				else if (input == "1") { if (positve) positve = false; else positve = true; }
				Console.Clear();
			}
		}
	}

	class ArrayTools3D
	{
		private delegate void Operation(int x, int y, int z);  // Используется для передачи в метод цикла различных функций
		private Random rand = new Random();
		private int[,,] array3D;

		public ArrayTools3D(int[,,] a3D)
		{
			array3D = a3D;
		}
		public void Draw(string str = "")
		{  // Большой и страшный метод отрисовывающий трёхмерный массив
			string filler = "";
			if (str != "")  // Отрисовка заголовка
			{
				filler = new string(' ', 42-(str.Length/2));
				Console.BackgroundColor = ConsoleColor.Yellow;
				Console.ForegroundColor = ConsoleColor.Black;
				Console.WriteLine($"{filler}{str}{filler}\n");
				Console.ResetColor();
			}
			
			int value = 0;
			string indent = "";
			for (int i = 0; i < array3D.GetLength(0); i++) // Цикл отрисовки массива
			{
				for (int j = 0; j < array3D.GetLength(1); j++)
				{
					Console.Write(indent + "(");
					for (int k = 0; k < array3D.GetLength(2); k++)
					{
						string elem = array3D[i, j, k].ToString();
						if (elem.Length < 3) { elem = elem.PadLeft(3); }
						if (k == array3D.GetLength(2) - 1) Console.Write($"{elem}");
						else Console.Write($"{elem},");
					}
					Console.WriteLine(")");
				}
				value += (4 * array3D.GetLength(2)) + 1;
				indent = new string(' ', value);
			}
			Console.BackgroundColor = ConsoleColor.Yellow;
			Console.ForegroundColor = ConsoleColor.Black;
			filler = new string('=', 28);
			Console.WriteLine($"\n{filler}====> ИЗМЕРЕНИЯ МАССИВА ====>{filler}");
			Console.ResetColor();
		}
		public void RemovePositive()
		{  // Метод удаляющий положительные числа в трёхмерном массиве, создаёт делегат функции и передаёт его в цикл обрабатывающий массив
			Operation remove_positive = RemovePositive;
			Array3DLoop(remove_positive);
		}
		public void RemoveNegative()
		{  // Метод удаляющий отрицательные числа в трёхмерном массиве, создаёт делегат функции и передаёт его в цикл обрабатывающий массив
			Operation remove_negative = RemoveNegative;
			Array3DLoop(remove_negative);
		}
		public void RandomFill()
		{  // Метод заполняющий массив случайными числами, создаёт делегат функции и передаёт его в цикл обрабатывающий массив
			Operation random_fill = RandomFill;
			Array3DLoop(random_fill);
		}

		private void Array3DLoop(Operation operation)
		{  // Цикл обрабатывающий массив
			for (int i = 0; i < array3D.GetLength(0); i++)
			{
				for (int j = 0; j < array3D.GetLength(1); j++)
				{
					for (int k = 0; k < array3D.GetLength(2); k++)
					{
						operation(i, j, k);
					}
				}
			}
		}
		private void RemovePositive(int x, int y, int z)
		{
			if (array3D[x, y, z] > 0) array3D[x, y, z] = 0;
		}
		private void RemoveNegative(int x, int y, int z)
		{
			if (array3D[x, y, z] < 0) array3D[x, y, z] = 0;
		}
		private void RandomFill(int x, int y, int z)
		{
			array3D[x, y, z] = rand.Next(-99, 100);
		}
	}
}
