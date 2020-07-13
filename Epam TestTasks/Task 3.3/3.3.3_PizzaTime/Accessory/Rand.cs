using System;

namespace PizzaTime
{
	static class Rand
	{
		static readonly Random rand = new Random();

		public static int GetRandom(int a)
		{
			return rand.Next(a);
		}

		public static int GetRandom(int a, int b)
		{
			return rand.Next(a, b);
		}
	}
}
