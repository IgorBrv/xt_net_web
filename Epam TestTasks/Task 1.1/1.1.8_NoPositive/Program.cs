using System;
using Outputlib;   // Кастомная библиотека вывода текста в цвете

namespace NoPositive
{  // Написать программу, которая заменяет все положительные элементы в трёхмерном массиве на
   // нули. Число элементов в массиве и их тип определяются разработчиком.
	class Program
	{
		static void Main(string[] args)
		{
			Random rand = new Random();
			bool positve = true;
			while (true)
			{
				int[,,] array3D = new int[rand.Next(2, 5), rand.Next(2, 5), rand.Next(2, 5)];
				//int[,,] array3D = new int[,,] { { { 1, 2, 3 }, { 4, 5, 6 } }, { { 7, 8, 9 }, { 10, 11, 12 } }, { { 13, 14, 15 }, { 16, 17, 18 } } };
				ArrayTools3D artools = new ArrayTools3D(array3D);
				artools.RandomFill();  // Заполнение массива рандомными цифрами

				Output.Print("b", "g", $"\n ПРОГРАММА, КОТОРАЯ ЗАМЕНЯЕТ ВСЕ ПОЛОЖИТЕЛЬНЫЕ ЭЛЕМЕНТЫ В ТРЁХМЕРНОМ МАССИВЕ НА НУЛИ \n\n");

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

				Console.Write("\n\nНажмите ENTER для обновления массива ИЛИ введите 's' для смены режима, 'exit' для выхода: ");
				string input = Console.ReadLine().Trim().ToLower();
				if      (input == "exit") break;
				else if (input == "s"   ) positve = !positve;
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
		{  // Большой и страшный метод отрисовки трёхмерного массива
			string filler;
			if (str != "")  // Отрисовка заголовка
			{
				filler = new string(' ', 42-(str.Length/2));
				Output.Print("b", "y", $"{filler}{str}{filler}\n");
			}
			
			int value = 0;
			string indent = "";
			for (int i = 0; i < array3D.GetLength(0); i++) // Цикл отрисовки массива
			{
				for (int j = 0; j < array3D.GetLength(1); j++)
				{
					Console.Write(indent + "|");
					for (int k = 0; k < array3D.GetLength(2); k++)
					{
						string elem = array3D[i, j, k].ToString().PadLeft(3);
						Console.Write($"{elem}");
						if (k != array3D.GetLength(2) - 1) Console.Write("|");
					}
					Console.WriteLine("|");
				}
				value += (4 * array3D.GetLength(2)) + 1;
				indent = new string(' ', value);
			}
			filler = new string('=', 28);
			Output.Print("b", "y", $"\n{filler}====> ИЗМЕРЕНИЯ МАССИВА ====>{filler}");

		}
		public void RemovePositive() => Array3DLoop(new Operation(RemovePositive));
		// Метод передающий в цикл делегат функции заменяющей в массиве все положительные числа на 0

		public void RemoveNegative() => Array3DLoop(new Operation(RemoveNegative));
		// Метод передающий в цикл делегат функции заменяющей в массиве все отрицательные числа на 0

		public void RandomFill() => Array3DLoop(new Operation(RandomFill));
		// Метод передающий в цикл делегат функции заполняющей массив случайными числами.

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

		private void Array3DLoop(Operation operation)
		{   // Универсальный цикл перебирающий элементы трёхмерного массива
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
	}
}
