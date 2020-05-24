using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Outputlib;   // Кастомная библиотека вывода текста в цвете


namespace SumOfNumbers
{   //Напишите программу, которая выводит на экран сумму всех чисел меньше 1000, кратных 3 или 5.
	class Program
	{
		static void Main(string[] args)
		{
			long result;
			int treshold = 1000;
			Stopwatch stopWatch = new Stopwatch();  // из собственного интереса ввёл таймеры
			Calc_multsum calc = new Calc_multsum();
			while (true)
			{
				Output.Print("b", "g", $"\n ВЫЧИСЛЕНИЕ СУММЫ ВСЕХ ЧИСЕЛ МЕНЬШЕ {treshold}, КРАТНЫХ 3 ИЛИ 5 \n\n");
				{
					stopWatch.Start();
					result = calc.Method1(treshold);
					stopWatch.Stop();
					Output.Print("", "db", $" Метод 1, результат: {result}, время выполнения: {stopWatch.Elapsed} ");
					Console.WriteLine("\nДанный  метод  ищет  кратные 3 и 5 числа путём простого перебора\n" +
									   "от 1 до порога циклом while с проверкой конструкцией if.\n\n");
					stopWatch.Reset();
				}
				{
					stopWatch.Start();
					result = calc.Method2(treshold);
					stopWatch.Stop();
					Output.Print("", "db", $" Метод 2, результат: {result}, время выполнения: {stopWatch.Elapsed} ");
					Console.WriteLine("\nДанный метод ищет кратные  3 и 5  числа путём перемножения 3 и 5\n" +
									  "на  другие  натуральные  числа  до  достижения порогового числа,\n" +
									  "исключая  повторное  прибавление  чисел  кратных  и 3 и 5  путём\n" +
									  "дополнительной проверки (число * 5 / 3).\n\n");
					stopWatch.Reset();
				}
				Console.Write("\nНажмите ENTER для пересчёта времени, введите желаемое пороговое число или 'exit' для выхода: ");
				string input = Console.ReadLine().Trim().ToLower();
				if (input == "exit") break;
				if (!Int32.TryParse(input.Trim(), out treshold)) treshold = 1000;
				Console.Clear();
			}
		}
	}
	class Calc_multsum
	{   // Класс калькулятора
		public long Method1(int treshold)
		{   // Метод 3: Поиск чисел кратных 3 и 5 путём перебора
			long sum = 0;
			for (int i = 1; i < treshold; i++)
				if (i % 3 == 0 || i % 5 == 0) { sum += i; }
			return sum;
		}
		public long Method2(int treshold)
		{   // Метод 2: Поиск чисел кратных 3 и 5 путём перемножения с отсеиванием повторений кратных обоим чисел при помощи дополнительной проверки
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
			// !!!! Не состоятелен из-за большого потребления памяти, написан исключительно из собственного интереса !!!
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
		{   // Метод 4: Поиск чисел кратных 3 и 5 путём перебора в рекурсии. 
			// быстро переполняет стек.
			treshold -= 1;
			if (treshold == 2) return sum;
			else if (treshold % 3 == 0 || treshold % 5 == 0) { sum += treshold; }
			return Method4(treshold, sum);
		}
	}
}
