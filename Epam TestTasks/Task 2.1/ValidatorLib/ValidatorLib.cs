using System;
using System.Collections.Generic;
using Outputlib;


namespace ValidatorLib
{
	public class Validator
	{
		static public string Fix(string input, char additional = '.')
		{   //first letter of sentence fixer

			List<char> dividers = new List<char>{ '.', '?', '!' };

			if (additional != '.')
			{
				dividers.Add(additional);
			}

			byte count = 0;
			bool key = true;
			char[] str = input.ToCharArray();  // разбиваем строку на массив символов
			for (int i = 0; i < str.Length; i++)
			{   // проходимся по всем символам циклом
				if (dividers.Contains(str[i]))
				{   // если встречаем знак окончания предложения ?/!/./.../?!
					key = true;
					count = 0;
				};
				if (key)
				{
					if (Char.IsLetter(str[i]))
					{   // начинаем искать, следует ли за ним буква
						str[i] = Char.ToUpper(str[i]);
						count = 0;
						key = false;
					}  // на протяжении трёх последующих символов (Ограничение на глубину поиска можно снять):
					else if (count > 2)
					{
						count = 0;
						key = false;
					}
					else count++;
				}
			}
			return string.Join("", str);
		}
	}
}
