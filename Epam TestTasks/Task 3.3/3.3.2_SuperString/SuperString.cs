using System;
using System.Collections.Generic;
using System.Linq;

namespace SuperString
{
    public static class SuperString
	{   // Расширьте строку следующим методом: проверка, на каком языке написано слово в строке.
		// Ограничимся четырьмя вариантами – Russian, English, Number and Mixed.

		public static string CheckLang(this string str)
		{
			// Отфильтруем строку от знаков препинания:
			str = string.Join("", str.Where(i => Char.IsLetterOrDigit(i)));

			// Пересчитаем колличество слов состоящих из цифр, английских или русских букв, подошьём результаты в словарь:
			Dictionary<string, int> types = new Dictionary<string, int>
			{
				{ "Numbers", str.Count(i => Char.IsNumber(i)) },
				{ "English", str.Count(i => i < 1000 && Char.IsLetter(i)) },
				{ "Russian", str.Count(i => i > 1000 && Char.IsLetter(i)) }
			};

			// Вернём тип из словаря, если в словаре присутствуют слова лишь одного типа
			if (types.Count(Pair => Pair.Value > 0) == 1)
			{
				return types.Where(Pair => Pair.Value > 0).FirstOrDefault().Key;
			}

			// Вернём "Mixed", если нескольких типов
			return "Mixed";
		}
	}
}
