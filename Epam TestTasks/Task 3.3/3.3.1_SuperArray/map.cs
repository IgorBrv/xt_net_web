using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperArray
{
	public static class IntArrayExtencions
	{
		public static void Map(this int[] array, Func<int, int> func)
		{
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = func(array[i]);
			}
		}

		public static void Map(this double[] array, Func<double, double> func)
		{
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = func(array[i]);
			}
		}

		public static int CustomAverage<T>(this int[] array)
		{
			int sum = 0; 
			foreach (int i in array)
			{
				sum += i;
			}

			return sum/array.Length;
		}

		public static double CustomAverage<T>(this double[] array)
		{
			double sum = 0;
			foreach (double i in array)
			{
				sum += i;
			}

			return sum / array.Length;
		}

		public static string CheckLang(this string str)
		{
			str = string.Join("", str.Where(i => Char.IsLetterOrDigit(i)));
			Dictionary<string, int> types = new Dictionary<string, int>();

			types.Add("Numbers", str.Count(i => Char.IsNumber(i)));
			types.Add("English", str.Count(i => i < 1000 && Char.IsLetter(i)));
			types.Add("Russian", str.Count(i => i > 1000 && Char.IsLetter(i)));

			if (types.Count(Pair => Pair.Value > 0) == 1)
			{
				return types.Where(Pair => Pair.Value > 0).FirstOrDefault().Key;

			}
			return "Mixed";
		}
	}
}
