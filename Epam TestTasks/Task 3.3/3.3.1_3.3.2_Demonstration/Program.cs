using System;
using SuperArray;
using SuperString;

namespace Demonstration
{
	class Demonstration
	{
		static void Main()
		{
			string str = "Привет, мир!";
			int[] array2 = { 1, 2, 3, 4, 5 };
			short[] array3 = { 1, 2, 3, 4, 5 };
			double[] array = { 4, 4, 2, 4, 3, 1, 1, 1, 2 };

			// Демонастрация работы Map:
			Console.WriteLine($"Демонстрация работы map. Массив: {string.Join(", ", array2)}, map: {string.Join(", ", array2.Map(i => i * 2))}\n");

			// Демонстрация работы среднего:
			Console.WriteLine($"Демонстрация работы CustomAverage. Массив: {string.Join(", ", array2)}, CustomAverage: {string.Join(", ", array2.CustomAverage())}\n");

			// Демонстрация работы обобщённо-костыльного среднего:
			Console.WriteLine($"Демонстрация работы обобщённого CustomAverage. Массив: {string.Join(", ", array3)}, CustomAverage: {string.Join(", ", array3.CustomAverage())}\n");

			// Демонастрация работы поиска наиболее часто используемого элемента:
			Console.WriteLine($"Демонстрация работы поиска наиболее частого эл-та. Массив: {string.Join(", ", array)}, Наиболее частый эл-т: {string.Join(", ", array.OftenlyUsed())}\n");

			// Демонстрация работы поиска суммы всех элементов:
			Console.WriteLine($"Демонстрация работы поиска суммы всех эл-тов. Массив: {string.Join(", ", array3)}, Сумма элементов: {string.Join(", ", array3.CustomSum())}\n");

			// Демонстрация работы определения языка написания предложения:
			Console.WriteLine($"Демонстрация определения языка написания предложения. Предложение: '{str}', Язык: {str.CheckLang()}");

			Console.ReadLine();
		}
	}
}
