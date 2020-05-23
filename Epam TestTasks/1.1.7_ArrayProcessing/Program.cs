using System;
using System.Collections.Generic;
using System.Linq;
using Outputlib;


namespace ArrayProcessing
{   //Написать программу, которая генерирует случайным образом элементы массива, определяет для него максимальное и 
    //минимальное значения, сортирует массив и выводит полученный результат на экран.
	class Program
	{  
		static void Main(string[] args)
		{ 
			Random rand = new Random();
			while (true)
			{
				int[] lst = new int[rand.Next(14, 24)];  // Генерация массива случайной длины
				for (byte i = 0; i < lst.Length; i++) { lst[i] = rand.Next(-99, 100); }  // Наполнение массива случайными числами

				Output.Print("b", "g", $"\n ПРОГРАММА, КОТОРАЯ ГЕНЕРИРУЕТ СЛУЧАЙНЫЙ МАССИВ, СОРТИРУЕТ ЕГО И НАХОДИТ MAX/MIN ЗНАЧЕНИЯ \n");

				Output.Print("b", "c", "\n Массив чисел случйной длины со случайным наполнением:\n");
				Console.WriteLine(string.Join(", ", lst));

				Output.Print("b", "c", false, "\n\n Минимальный  элемент массива: ");
				Console.WriteLine($"[{ArrayTools.Min(lst).ToString().PadLeft(3)}]");

				Output.Print("b", "c", false, "\n\n Максимальный элемент массива: ");
				Console.WriteLine($"[{ArrayTools.Max(lst).ToString().PadLeft(3)}]");

				lst = ArrayTools.Quicksort(lst);  // Сортировка массива

				Output.Print("b", "c", "\n\n Массив чисел, отсортированый при помощи алгоритма быстрой сортировки:\n");
				Console.WriteLine(string.Join(", ", lst));

				Console.Write("\n\nНажмите ENTER для обновления массива или введите 'exit' для выхода: ");
				string input = Console.ReadLine().Trim().ToLower();
				if (input == "exit") break;
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
		static public List<int> Quicksort(List<int> array)
		{  // Рекурсивный алгоритм быстрой сортировки
			if (array.Count < 2) return array;
			else
			{
				List<int> less    = new List<int>();
				List<int> greater = new List<int>();
				List<int> middle  = new List<int> { array[0] };
				for (int i = 1; i < array.Count; i++)
				{
					if      (array[i] <  middle[0])    less.Add(array[i]);
					else if (array[i] == middle[0])  middle.Add(array[i]);
					else if (array[i] >  middle[0]) greater.Add(array[i]);
				}
				return Quicksort(less).Concat(middle).ToList().Concat(Quicksort(greater)).ToList(); // Quicksort(less) + middle + Quicksort(greater)
			}
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
