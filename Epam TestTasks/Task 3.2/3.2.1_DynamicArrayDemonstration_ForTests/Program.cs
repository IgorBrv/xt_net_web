using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicArrayLib;

namespace DynamicArrayDemonstration_ForTests
{
	class Program
	{
		static void Main(string[] args)
		{
			List<int> lst = new List<int> { 1, 2, 3 };
			DynamicArray<int> da1 = new DynamicArray<int>(lst);
			DynamicArray<int> da2 = new DynamicArray<int>(da1);
			CycledDynamicArray<int> cda1 = new CycledDynamicArray<int>(da1);
			Console.ReadLine();
		}
	}
}
