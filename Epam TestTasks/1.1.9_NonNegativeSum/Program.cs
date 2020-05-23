using System;
using System.Collections.Generic;
using Outputlib;   // Кастомная библиотека вывода текста в цвете

namespace NonNegativeSum
{  // Написать программу, которая определяет сумму неотрицательных элементов в одномерном
   // массиве. Число элементов в массиве и их тип определяются разработчиком.
	class Program
	{
		static void Main(string[] args)
		{
			Random rand = new Random();
			while (true)
			{
				int[] lst = new int[rand.Next(14, 24)];  // Генерация массива случайной длины
				for (byte i = 0; i < lst.Length; i++) { lst[i] = rand.Next(-100, 100); }  // Наполнение массива случайными числами

				Output.Print("b", "g", $"\n ПРОГРАММА, КОТОРАЯ ОПРЕДЕЛЯЕТ СУММУ НЕОТРИЦАТЕЛЬНЫХ ЭЛЕМЕНТОВ В МАССИВЕ \n\n");
				Output.Print("b", "c", " Массив чисел случйной длины со случайным наполнением:\n");
				Console.WriteLine(string.Join(", ", lst));

				int sum = 0;
				List<int> elems = new List<int>();
				foreach (int i in lst) if (i > 0) { sum += i; elems.Add(i); }

				Output.Print("b", "c", false, "\n\n Сумма положительных элементов массива:");
				Console.WriteLine($" {sum}, ({string.Join(", ", elems)})");

				Console.Write("\n\nНажмите ENTER для обновления массива или введите 'exit' для выхода: ");
				string input = Console.ReadLine().Trim().ToLower();
				if (input == "exit") break;
				Console.Clear();
			}
		}
	}
}