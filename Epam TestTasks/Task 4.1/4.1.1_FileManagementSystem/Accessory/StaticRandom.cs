using System;

namespace FileManagementSystem.Accessory
{
	class StaticRandom
	{	// Вспомогательный класс генерирующий случайные числа

		private static readonly Random rand = new Random();

		public static int Get(int range)
		{
			return rand.Next(range);
		}
	}
}
