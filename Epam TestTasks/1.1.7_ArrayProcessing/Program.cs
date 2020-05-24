using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Outputlib;   // Кастомная библиотека вывода текста в цвете


namespace ArrayProcessing
{   //Написать программу, которая генерирует случайным образом элементы массива, определяет для него максимальное и 
    //минимальное значения, сортирует массив и выводит полученный результат на экран.
	class Program
	{  
		static void Main(string[] args)
		{ 
			Random rand = new Random();
			Stopwatch stopWatch = new Stopwatch();
			int input_int = 0;
			while (true)
			{
				int[] lst;
				if (input_int > 2) { lst = new int[input_int]; input_int = 0; }          // Генерация массива динной в введённое число
				else { lst = new int[rand.Next(14, 24)]; }					             // Генерация массива случайной длины
				for (int i = 0; i < lst.Length; i++) { lst[i] = rand.Next(-99, 100); }   // Наполнение массива случайными числами

				Output.Print("b", "g", $"\n ПРОГРАММА, КОТОРАЯ ГЕНЕРИРУЕТ СЛУЧАЙНЫЙ МАССИВ, СОРТИРУЕТ ЕГО И НАХОДИТ MAX/MIN ЗНАЧЕНИЯ \n");

				Output.Print("b", "c", "\n Массив чисел случйной длины со случайным наполнением:\n");
				if (lst.Length <= 300) Console.WriteLine(string.Join(", ", lst));

				Output.Print("b", "c", false, "\n\n Минимальный  элемент массива: ");
				Console.WriteLine($"[{ArrayTools.Min(lst).ToString().PadLeft(3)}]");

				Output.Print("b", "c", false, "\n\n Максимальный элемент массива: ");
				Console.WriteLine($"[{ArrayTools.Max(lst).ToString().PadLeft(3)}]");

				stopWatch.Start();
				Output.Print("b", "c", "\n\n Массив чисел, отсортированый при помощи алгоритма быстрой сортировки:      \n");
				if (lst.Length > 300) ArrayTools.Quicksort(lst);
				else Console.WriteLine(string.Join(", ", ArrayTools.Quicksort(lst)));
				stopWatch.Stop();
				Console.WriteLine($"\nВремя выполнения: {stopWatch.Elapsed}");
				stopWatch.Reset();

				stopWatch.Start();
				Output.Print("b", "c", "\n\n Массив чисел, отсортированый при помощи алгоритма сортировки перестановкой:\n");
				if (lst.Length > 300) ArrayTools.Quicksort(lst);
				else Console.WriteLine(string.Join(", ", ArrayTools.Replacesort(lst)));
				stopWatch.Stop();
				Console.WriteLine($"\nВремя выполнения: {stopWatch.Elapsed}");
				stopWatch.Reset();

				Console.Write("\n\nНажмите ENTER для обновления массива, введите желаемую длину массива или 'exit' для выхода: ");
				string input = Console.ReadLine().Trim().ToLower();
				if (input == "exit") break;
				Int32.TryParse(input.Trim(), out input_int);
				Console.Clear();
			}
		}
	}

	class ArrayTools
	{
		static public int[] Quicksort(int[] array)
		{  // Метод приводящий массив int[] к list<int> для передачи в Quicksort(List<int>)
			return Quicksort(array.ToList()).ToArray();
		}
		static public IEnumerable<int> Quicksort(List<int> array)
		{  // Рекурсивный алгоритм быстрой сортировки. Можно добавить дополнительный промежуточный список для расширения рабочего диапазона (Как в ранних комитах)
			if (array.Count < 2) return array;
			else
			{
				List<int> less    = new List<int>();
				List<int> greater = new List<int>();
				for (int i = 1; i < array.Count; i++)
				{
					if (array[i] < array[0]) less.Add(array[i]);
					else greater.Add(array[i]);
				}
				return Quicksort(less).Concat(array.Take(1)).Concat(Quicksort(greater)); // Quicksort(less) + middle + Quicksort(greater)
			}
		}

		static public int[] Replacesort(int[] array)
		{   // алгоритм сортировки перестановкой
			for (int i = 0; i < array.Length-1; i++)
			{
				for (int j = i+1; j < array.Length; j++)
				{
					if (array[i] > array[j])
					{
						int k = array[i];
						array[i] = array[j];
						array[j] = k;
					}
				}
			}
			return array;
		}

		static public int Max(int[] list)
		{  // Метод, находящий максимальный элемент массива
			int max = list[0];
			foreach (int i in list) { if (i > max) max = i; }
			return max;
		}
		static public int Min(int[] list)
		{  // Метод, находящий минимальный элемент массива
			int min = list[0];
			foreach (int i in list) { if (i < min) min = i; }
			return min;
		}
	}
}
