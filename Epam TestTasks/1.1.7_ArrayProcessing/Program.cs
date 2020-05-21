using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace ArrayProcessing
{
	class Program
	{
		static void Main(string[] args)
		{
			Random rand = new Random();
			while (true)
			{
				int[] lst = new int[rand.Next(14, 24)];
				for (byte i = 0; i < lst.Length; i++) { lst[i] = rand.Next(100); }
				Console.BackgroundColor = ConsoleColor.Cyan;
				Console.ForegroundColor = ConsoleColor.Black;
				Console.WriteLine(" Массив чисел случйной длины со случайным наполнением:\n");
				Console.ResetColor();
				Console.WriteLine(string.Join(", ", lst));

				lst = quicksort(lst.ToList()).ToArray();
				Console.BackgroundColor = ConsoleColor.Cyan;
				Console.ForegroundColor = ConsoleColor.Black;
				Console.WriteLine("\n\n Массив чисел, отсортированый при помощи алгоритма быстрой сортировки:\n");
				Console.ResetColor();
				Console.WriteLine(string.Join(", ", lst));

				Console.BackgroundColor = ConsoleColor.Cyan;
				Console.ForegroundColor = ConsoleColor.Black;
				Console.Write("\n\n Минимальный элемент массива:");
				Console.ResetColor();
				Console.WriteLine(" " + lst[0]);

				Console.BackgroundColor = ConsoleColor.Cyan;
				Console.ForegroundColor = ConsoleColor.Black;
				Console.Write("\n\n Максимальный элемент массива:");
				Console.ResetColor();
				Console.WriteLine(" " + lst[lst.Length-1]);

				Console.Read();
				Console.Clear();
			}
		}

		static List<int> quicksort(List<int> array)
		{
			if (array.Count < 2) return array;
			else
			{
				List<int> less = new List<int>();
				List<int> greater = new List<int>();
				for (int i = 1; i < array.Count; i++)
				{
					if (array[i] <= array[0]) { less.Add(array[i]); }
					else if (array[i] > array[0]) { greater.Add(array[i]); }
				}
				List<int> leftpart = new List<int> (quicksort(less));
				leftpart.Add(array[0]);
				return leftpart.Concat(quicksort(greater)).ToList();
			}
		}
	}
}
