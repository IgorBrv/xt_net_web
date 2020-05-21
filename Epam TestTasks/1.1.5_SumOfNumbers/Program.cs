using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace _1._1._5_SumOfNumbers
{
	class Calc_multsum
	{
		private int treshold;
		public Calc_multsum(int th)
		{
			treshold = th;
		}
		public long Method1(int treshold)
		{  // Метод 3: Поиск чисел кратных 3 и 5 путём перебора
			long sum = 0;
			for (int i = 1; i < treshold; i++)
				if (i % 3 == 0 || i % 5 == 0) { sum += i; }
			return sum;
		}

		public long Method2(int treshold)
		{  // Метод 2: Поиск чисел кратных 3 и 5 путём перемножения с отсеиванием повторений кратных обоим чисел при помощи дополнительной проверки
			long sum = 0;
			int num = 1;
			while (true)
			{
				int prod = 3 * num;
				if (prod < treshold) { sum += prod; }
				else break;
				num++;
			}
			num = 1; while (true)
			{
				int prod = 5 * num;
				if (prod < treshold) { if (prod % 3 != 0) { sum += prod; } }
				else break;
				num++;
			}
			return sum;
		}

		public long Method3(int treshold)
		{   // Метод 1: Поиск чисел кратных 3 и 5 путём перемножения с отсеиванием повторений кратных обоим чисел путём добавления в hashset.
			// Не состоятелен из-за больного потребления памяти, приведён как пример
			HashSet<long> set = new HashSet<long>();
			foreach (int i in new int[] { 3, 5 })
			{
				int mult = 1;
				while (true)
				{
					int prod = mult * i;
					if (prod >= treshold) break; else set.Add(prod);
					mult++;
				}
			}
			return set.Sum();
		}

		public long Method4(int treshold, long sum = 0)
		{  // Метод 4: Поиск чисел кратных 3 и 5 путём перебора в рекурсии
			treshold -= 1;
			if (treshold == 2) return sum;
			else if (treshold % 3 == 0 || treshold % 5 == 0) { sum += treshold; }
			return Method4(treshold, sum);
		}
	}

	class Program
	{  //Напишите программу, которая выводит на экран сумму всех чисел меньше 1000, кратных 3 или 5.
		static void Main(string[] args)
		{
			int treshold = 1000;  //99999999

			Console.BackgroundColor = ConsoleColor.Green;
			Console.ForegroundColor = ConsoleColor.Black;
			Console.WriteLine($"\n ВЫЧИСЛЕНИЕ СУММЫ ВСЕХ ЧИСЕЛ МЕНЬШЕ {treshold}, КРАТНЫХ 3 ИЛИ 5 \n\n"); ; ;
			Console.ResetColor();

			long result;
			Stopwatch stopWatch = new Stopwatch();
			Calc_multsum calc = new Calc_multsum(treshold);

			stopWatch.Start();
			result = calc.Method1(treshold);
			stopWatch.Stop();
			Console.BackgroundColor = ConsoleColor.DarkBlue;
			Console.WriteLine($" Метод 1, результат: {result}, время выполнения: {stopWatch.Elapsed} ");
			Console.ResetColor();
			Console.WriteLine("\nДанный  метод  ищет  кратные 3 и 5 числа путём простого перебора\n" +
						 	  "от 1 до порога циклом while с проверкой конструкцией if.\n\n");

			stopWatch.Start();
			result = calc.Method2(treshold);
			stopWatch.Stop();
			Console.BackgroundColor = ConsoleColor.DarkBlue;
			Console.WriteLine($" Метод 2, результат: {result}, время выполнения: {stopWatch.Elapsed} ");
			Console.ResetColor();
			Console.WriteLine("\nДанный метод ищет кратные  3 и 5  числа путём перемножения 3 и 5\n" +
							  "на  другие  натуральные  числа  до  достижения порогового числа,\n" +
							  "исключая  повторное  прибавление  чисел  кратных  и 3 и 5  путём\n" +
							  "дополнительной проверки (число * 5 / 3).\n\n");

			stopWatch.Start();
			result = calc.Method4(treshold);
			stopWatch.Stop();
			Console.BackgroundColor = ConsoleColor.DarkBlue;
			Console.WriteLine($" Метод 4, результат: {result}, время выполнения: {stopWatch.Elapsed} ");
			Console.ResetColor();
			Console.WriteLine("\nДанный  метод ищет кратные 3  и  5 числа путём простого перебора\n" +
						 	  "от порога до 0 рекурсивным циклом.\n\n");


			Console.Read();
		}
	}
}
