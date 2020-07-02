using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dynamic_Array;

namespace Dynamic_Array_Demonstration
{
	class Program
	{
		static void Main(string[] args)
		{
			List<string> list = new List<string> { "Один", "два" };
			list.AsReadOnly();
			int coynt = list.Count;

			int[] array = new int[3] { 1, 2, 3 };

			int?[] array2 = new int?[3];


			Console.WriteLine("!!!" + ReferenceEquals(array, array2));


			Console.WriteLine(string.Join(", ", array));
			Console.WriteLine(string.Join(", ", array2));

			list[1] = "123";
			foreach (string i in list)
			{
				Console.WriteLine(i);
			}

			string[] array3 = new string[3] { "один", "два", "три" };

			DynamicArray<string> da = new DynamicArray<string>(array3);
			DynamicArray<string> da2 = new DynamicArray<string>();
			da.Add("четыре");
			da2 = (DynamicArray<string>)da.Clone();
			Console.WriteLine("!!!!!" + da[3]);
			Console.WriteLine("!!!!!" + da2[3]);
			Console.WriteLine("!!!!!" + da2.Contains("четыре"));

			Test<string> tst = new Test<string>("Один", "Два", "Три");

			foreach (string i in tst)
			{
				Console.WriteLine(i);
			}

			Console.ReadLine();
		}
	}

	class Test<T> : IEnumerator<T>, IEnumerable<T>
	{



		T[] baseArray = new T[3];

		public Test(T a, T b, T c)
		{
			baseArray[0] = a;
			baseArray[1] = b;
			baseArray[2] = c;
		}

		private T item;
		private int curIndex = -1;


		public T Current => item;

		object IEnumerator.Current => Current;

		public void Dispose()
		{
			// На docs.microsoft в данной ситуации сказанно оставить пустую реализацию
		}

		public bool MoveNext()
		{
			if (++curIndex >= baseArray.Length)
			{
				return false;
			}
			else
			{
				item = baseArray[curIndex];
			}
			return true;
		}

		public void Reset()
		{
			curIndex = -1;
		}

		public IEnumerator<T> GetEnumerator()
		{
			Console.WriteLine("!!! это");
			return this;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return baseArray.GetEnumerator();
		}
	}
}
