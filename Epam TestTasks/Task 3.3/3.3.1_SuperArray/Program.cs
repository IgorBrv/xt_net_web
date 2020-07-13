using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SuperArray
{
	class Program
	{
		static void Main(string[] args)
		{
			int[] array = { 1, 2, 3 };
			// Демонастрация работы Map:
			array.Map(i => i * 2);
			Console.WriteLine(string.Join(", ", array));

			// Демонстрация работы среднего:
			//Console.WriteLine(array.CustomAverage());

			// Демонстрация работы 

			char a = 'a';
			char b = 'b';

			Console.WriteLine("!!!" + (int)'а');

			char c = (char)(a + b);
			Console.WriteLine(c);

			string str = "Привет, мир!";

			Console.WriteLine($"string: '{str}'; type: {str.CheckLang()}");

			Console.ReadLine();

		}
	}
}
