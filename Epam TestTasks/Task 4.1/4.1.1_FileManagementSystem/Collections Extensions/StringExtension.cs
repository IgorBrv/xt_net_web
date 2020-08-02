
namespace FileManagementSystem
{
	public static class StringExtension
	{	// Вспомогательные методы расширения для работы со строками

		// Вспомогательные переменные для RFind и RFindNext
		private static int pos = -1;
		private static string strbak;

		public static int RFind(this string str, char x)
		{	// Метод осуществляющий поиск символа в строке начиная с конца (справа)

			strbak = str;

			for (int i = str.Length-1; i >= 0; i--)
			{
				if (str[i] == x)
				{
					pos = i;
					return i;
				}
			}

			pos = -1;
			return -1;
		}

		public static int RFindNext(this string str, char x)
		{   // Метод осуществляющий поиск СЛЕДУЮЩЕГО символа в строке начиная с конца (справа)

			if (pos == -1 || str != strbak)
			{
				return RFind(str, x);
			}

			for (int i = pos-1; i >= 0; i--)
			{
				if (str[i] == x)
				{
					pos = i;
					return i;
				}
			}

			pos = -1;
			return -1;
		}

		public static string RTakePart(this string str, int posB, int posA)
		{	// Метод возвращающий подстроку из строки по заданным "координатам"

			if (posA > posB)
			{
				int temp = posA;
				posA = posB;
				posB = temp;
			}

			return str.Substring(posA + 1, posB - posA - 1);
		}
	}
}
