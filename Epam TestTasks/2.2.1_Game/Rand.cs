using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
	static class Rand
	{	// Класс генератора рандомных чисел
		static readonly Random rand = new Random();
		public static int Get(int i, int j) => rand.Next(i, j);
	}
}
