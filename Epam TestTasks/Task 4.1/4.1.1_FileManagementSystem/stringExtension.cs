using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FileManagementSystem
{
	public static class stringExtension
	{
		public static int RFind(this string str, char x)
		{
			for (int i = str.Length-1; i >= 0; i--)
			{
				if (str[i] == x)
				{
					return i;
				}
			}
			return -1;
		}
	}
}
