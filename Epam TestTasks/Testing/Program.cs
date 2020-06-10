using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testing
{
	class Program
	{
		static void Main()
		{
			Dictionary<string, int> dict = new Dictionary<string, int> { { "Один", 1 }, { "Два", 2 } };
			for (int i = 0; i < dict.Count; i++)
			{
			}

			foreach (string key in dict.Keys)
			{
				Console.WriteLine(dict[key]);
			}
			Console.ReadLine();
		}
	}

}
