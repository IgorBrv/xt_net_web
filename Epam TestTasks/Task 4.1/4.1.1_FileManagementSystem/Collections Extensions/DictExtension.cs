using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagementSystem
{
	public static class DictExtension
	{
		public static Dictionary<string, string> DictPairReplace(this Dictionary<string, string> dict)
		{   // Вспомогательный метод расширения меняющий в словаре пару ключ:значения местами

			Dictionary<string, string> temp = new Dictionary<string, string>();

			foreach (string key in dict.Keys)
			{
				temp.Add(dict[key], key);
			}

			return temp;
		}
	}
}
