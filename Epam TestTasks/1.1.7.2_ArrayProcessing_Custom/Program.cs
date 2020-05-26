using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Outputlib;   // Кастомная библиотека вывода текста в цвете


namespace ArrayProcessing_Custom
{   //Написать программу, которая генерирует случайным образом элементы массива, определяет для него максимальное и 
	//минимальное значения, сортирует массив и выводит полученный результат на экран.
	class Program
	{
		static void Main(string[] args)
		{
			Random rand = new Random();

			int input_int = 0;

			while (true)
			{
				Output.Print("b", "g", $"\n ПРОГРАММА, КОТОРАЯ ГЕНЕРИРУЕТ СЛУЧАЙНЫЙ МАССИВ, СОРТИРУЕТ ЕГО И НАХОДИТ MAX/MIN ЗНАЧЕНИЯ \n");

				int lenght = rand.Next(14, 24);
				if (rand.Next(10) % 2 == 0 || input_int > 2)
				{
					int[] lst;
					if (input_int > 2) { lst = new int[input_int]; input_int = 0; }                 // Генерация массива динной в введённое число
					else { lst = new int[lenght]; }                                                 // Генерация массива случайной длины
					for (int i = 0; i < lst.Length; i++) { lst[i] = rand.Next(-99, 100); }          // Наполнение массива случайными числами
					ArrayProcessing<int> ap = new ArrayProcessing<int>();
					ap.Processing(lst);
				}
				else
				{
					char[] lst = new char[lenght];											         // Генерация массива случайной длины
					for (int i = 0; i < lst.Length; i++) { lst[i] = (char)(rand.Next('a', 'z')); }   // Наполнение массива случайными символами
					ArrayProcessing<char> ap = new ArrayProcessing<char>();
					ap.Processing(lst);
				}

				Console.Write("\n\nНажмите ENTER для обновления массива, введите желаемую длину массива или 'exit' для выхода: ");
				string input = Console.ReadLine().Trim().ToLower();
				if (input == "exit") break;
				Int32.TryParse(input.Trim(), out input_int);
				Console.Clear();
			}
		}
	}

	class ArrayProcessing<T> where T : IComparable<T>
	{
		private Stopwatch stopWatch = new Stopwatch();
		private ArrayTools<T> at = new ArrayTools<T>();
		public void Processing(T[] lst)
		{
			Output.Print("b", "c", "\n Массив чисел случйной длины со случайным наполнением:".PadRight(91) + "\n");
			if (lst.Length <= 300) Console.WriteLine(string.Join(", ", lst));

			Console.Write("\nМинимальный  элемент массива: ");
			Console.WriteLine($"{at.Min(lst).ToString().PadLeft(3)}");

			Console.Write("\nМаксимальный элемент массива: ");
			Console.WriteLine($"{at.Max(lst).ToString().PadLeft(3)}");

			stopWatch.Start();
			Output.Print("b", "c", "\n\n Массив чисел, отсортированый при помощи алгоритма быстрой сортировки:".PadRight(92) + "\n");
			if (lst.Length > 300) at.Quicksort(lst.ToList());
			else Console.WriteLine(string.Join(", ", at.Quicksort(lst.ToList())));
			stopWatch.Stop();
			Console.WriteLine($"\nВремя выполнения: {stopWatch.Elapsed}");
			stopWatch.Reset();

			stopWatch.Start();
			Output.Print("b", "c", "\n\n Массив чисел, отсортированый при помощи алгоритма сортировки перестановкой:".PadRight(92) + "\n");
			if (lst.Length > 300) at.Replacesort(lst);
			else Console.WriteLine(string.Join(", ", at.Replacesort(lst)));
			stopWatch.Stop();
			Console.WriteLine($"\nВремя выполнения: {stopWatch.Elapsed}");
			stopWatch.Reset();

		}
	}

	class ArrayTools<T> where T : IComparable<T>
	{
		public IEnumerable<T> Quicksort(List<T> array)
		{  // Рекурсивный алгоритм быстрой сортировки. Можно добавить дополнительный промежуточный список для расширения рабочего диапазона (Как в ранних комитах)
			if (array.Count < 2) return array;
			else
			{
				List<T> less = new List<T>();
				List<T> greater = new List<T>();
				for (int i = 1; i < array.Count; i++)
				{
					if (array[i].CompareTo(array[0]) < 0) less.Add(array[i]);
					else greater.Add(array[i]);
				}
				return Quicksort(less).Concat(array.Take(1)).Concat(Quicksort(greater)); // Quicksort(less) + middle + Quicksort(greater)
			}
		}
		public T[] Replacesort(T[] array)
		{   // алгоритм сортировки перестановкой
			for (int i = 0; i < array.Length - 1; i++)
			{
				for (int j = i + 1; j < array.Length; j++)
				{
					if (array[i].CompareTo(array[j]) > 0)
					{
						T k = array[i];
						array[i] = array[j];
						array[j] = k;
					}
				}
			}
			return array;
		}
		public T Max(T[] list)
		{  // Метод, находящий минимальный элемент массива
			T min = list[0];
			foreach (T i in list) { if (i.CompareTo(min) > 0) min = i; }
			return min;
		}
		public T Min(T[] list)
		{  // Метод, находящий минимальный элемент массива
			T min = list[0];
			foreach (T i in list) { if (i.CompareTo(min) < 0) min = i; }
			return min;
		}
	}
}
