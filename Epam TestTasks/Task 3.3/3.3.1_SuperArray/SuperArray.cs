using System;
using System.Collections.Generic;
using System.Linq;

namespace SuperArray
{
	public static class SuperArray
	{   // Расширьте массивы чисел методом, производящим действия с каждым конкретным элементом. Действие должно передаваться в метод с помощью делегата.
		// Кроме указанного функционала выше, добавьте методы расширения для поиска:
		// - суммы всех элементов;
		// - среднего значения в массиве;
		// - самого часто повторяемого элемента;


		public static int[] Map(this int[] array, Func<int, int> func)
		{   // Метод расширения, применяющий функцию передающуюся через делегат к каждому члену массива int

			if (array == null || func == null)
			{
				throw new ArgumentNullException("One of aruments is null!");
			}

			return array.Select(item => func(item)).ToArray();
		}

		public static double[] Map(this double[] array, Func<double, double> func)
		{   // Метод расширения, применяющий функцию передающуюся через делегат к каждому члену массива double

			if (array == null || func == null)
			{
				throw new ArgumentNullException("One of aruments is null!");
			}

			return array.Select(item => func(item)).ToArray();
		}

		public static int CustomSum(this int[] array)
		{   // Метод расширения возвращающий сумму элементов массива int[]
			// return array.Sum();

			if (array == null)
			{
				throw new ArgumentNullException("Input array is null!");
			}

			int sum = 0;
			array.Select(item => sum += item);

			return sum;
		}

		public static double CustomSum(this double[] array)
		{   // Метод расширения возвращающий сумму элементов для массива double[]
			// return array.Sum();

			if (array == null)
			{
				throw new ArgumentNullException("Input array is null!");
			}

			double sum = 0;
			array.Select(item => sum += item);

			return sum;
		}

		public static double CustomAverage(this int[] array)
		{   // Метод расширения возвращающий среднее значение для массива int[]

			if (array == null)
			{
				throw new ArgumentNullException("Input array is null!");
			}

			return array.CustomSum() / array.Length;
		}

		public static double CustomAverage(this double[] array)
		{   // Метод расширения возвращающий среднее значение для массива double[]

			if (array == null)
			{
				throw new ArgumentNullException("Input array is null!");
			}

			return array.CustomSum() / array.Length;
		}

		public static T OftenlyUsed<T>(this T[] array) where T: struct
		{   // Наиболее часто используемый элемент. Группирует элементы, производит сортирову по колличеству членов в группу
			// и возвращает наименьший элемент из наиболее часто используемых. 

			if (array == null)
			{
				throw new ArgumentNullException("Input array is null!");
			}

			return array.OrderBy(item => item).GroupBy(item => item).OrderByDescending(item => item.Count()).FirstOrDefault().Key;
		}


		// Далее идут супер-обощённые костыли:

		public static double CustomSum<T>(this T[] array) where T: struct
		{   // Сверхкостыльный обобщённый метод расширения для возврата суммы элементов массива
			List<Type> allowedTypes = new List<Type>() { typeof(Int16[]), typeof(Int32[]), typeof(Single[]), typeof(Double[]) };

			if (array == null)
			{
				throw new ArgumentNullException("Input array is null!");
			}

			if (!allowedTypes.Contains(array.GetType()))
			{
				return default;
			}

			double sum = 0;

			foreach (string num in array.Select(item => item.ToString()))
			{
				Double.TryParse(num, out double temp);
				sum += temp;
			}

			return sum;
		}

		public static double CustomAverage<T>(this T[] array) where T: struct
		{   // Сверхкостыльный обобщённый метод расширения для возврата среднего значения массива

			List<Type> allowedTypes = new List<Type>() { typeof(Int16[]), typeof(Int32[]), typeof(Single[]), typeof(Double[]) };

			if (!allowedTypes.Contains(array.GetType()))
			{
				return default;
			}
			else
			{
				return CustomSum<T>(array) / array.Length;
			}
		}
	}
}
