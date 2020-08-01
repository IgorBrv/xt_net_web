using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FileManagementSystem
{
	public static class MD5Calc
	{	// Вспомогательный класс для вычисления MD5-хеша файлов
		public static string GetMd5(byte[] b)
		{	// Вспомогательный метод рассчёта MD5-хеша

			byte[] data = MD5.Create().ComputeHash(b);

			return string.Join("", data.Select(item => item.ToString("x2")));
		}
	}
}
