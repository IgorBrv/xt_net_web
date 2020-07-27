using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FileManagementSystem
{
	public static class StringExtension
	{
		private static int pos = -1;
		private static string strbak;

		public static int RFind(this string str, char x)
		{
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
		{
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
		{
			StringBuilder sb = new StringBuilder();
			for (int i = posA+1; i < posB; i++)
			{
				sb.Append(str[i]);
			}
			return sb.ToString();
		}

	}
}
