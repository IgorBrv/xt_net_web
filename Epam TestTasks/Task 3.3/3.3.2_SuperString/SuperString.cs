using System;
using System.Collections.Generic;
using System.Linq;

namespace SuperString
{
    public static class SuperString
	{   // Расширьте строку следующим методом: проверка, на каком языке написано слово в строке.
		// Ограничимся четырьмя вариантами – Russian, English, Number and Mixed.


		/// <exception cref="ArgumentException">Argument cant be null</exception>
		public static LanguagesList CheckLang(this string str)
		{
			// Отфильтруем строку от знаков препинания:
			str = string.Join("", str.Where(i => Char.IsLetterOrDigit(i)));

			// Добавим проверку на пустоту
			if (str == string.Empty)
			{
				throw new ArgumentException("String is empty or string is not contain any words or numbers!");
			}

			// Проверим все символы строки на принадлежность к одному типу символов:
			if (str.All(i => Char.IsNumber(i)))
			{
				return LanguagesList.Numbers;
			}
			else if (str.All(i => Char.IsLetter(i) && i < 1000))
			{
				return LanguagesList.English;
			}
			else if (str.All(i => Char.IsLetter(i) && i > 1000))
			{
				return LanguagesList.Russian;
			}

			// При наличии разных типов символов вернём Mixed:
			return LanguagesList.Mixed;
		}
	}
}
