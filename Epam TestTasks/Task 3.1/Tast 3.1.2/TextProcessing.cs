using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Text_Analysis
{
	static class TextProcessing
	{
		public static string Begin(string str, bool nonRepeatable)
		{

			StringBuilder temp = new StringBuilder();
			Dictionary<string, int> wordsDict = new Dictionary<string, int>();
			Dictionary<string, int> shortWordsDict = new Dictionary<string, int>();
			bool isWord = false;
			int maxWordLength = 0;
			int maxShortWordLength = 0;

			if (Char.IsLetter(str[str.Length - 1]))
			{
				str += '.';
			}

			foreach (char i in str)
			{   // Простой цикл разбирающий текст на отдельные слова. Не пользуюсь split, т.к. считаю, что таким образом будет точнее, и нет необходимости использовать список разделителей,
				// нет необходимости придумывать какие могут быть разделители, итд.

				if (Char.IsLetter(i))
				{   // Если встречаем в строке букву - добавляем в стрингбилдер и ставим метку о записи слова
					isWord = true;
					temp.Append(i);
				}
				else if (Char.IsDigit(i) && isWord)
				{	// Добавляем цифры только если перед цифрой шла буква
					temp.Append(i);
				}
				else
				{
					if (isWord)
					{   // Если встречаем в строке НЕ буву, и метка о записи слова стоит - собираем стрингбилдер в строку, и добавляем в список длинных ИЛИ коротких слов.
						string word = temp.ToString().ToLower();
						if (word.Length > 3)
						{   // Длинные слова
							if (wordsDict.Keys.Contains(word))
							{
								wordsDict[word] = wordsDict[word] += 1;
							}
							else
							{
								wordsDict.Add(word, 1);
							}
							if (word.Length>maxWordLength)
							{
								maxWordLength = word.Length;
							}
						}
						else
						{   // Короткие слова. Слова до 3 букв. Предполагается, что сюда попадут предлоги, частицы, и подобные черезмерно используемые в речи элементы.
							if (shortWordsDict.Keys.Contains(word))
							{
								shortWordsDict[word] = shortWordsDict[word] += 1;
							}
							else
							{
								shortWordsDict.Add(word, 1);
							}
							if (word.Length > maxShortWordLength)
							{
								maxShortWordLength = word.Length;
							}
						}

						temp.Clear();
						isWord = false;
					}
				}
			}

			if (wordsDict.Count == 0 && shortWordsDict.Count == 0)
			{
				return "Введённый текст пуст!";
			}

			return DictsProcessing(wordsDict, shortWordsDict, maxWordLength, maxShortWordLength, nonRepeatable);
		}

		private static string DictsProcessing(Dictionary<string, int> wordsDict, Dictionary<string, int> shortWordsDict, int maxWordLength, int maxShortWordLength, bool nonRepeatable)
		{   // Метод обрабатывающий полученные списки слов, сортирующий их и выводящий в цельную строку для передачи в draw.

			// Сортировка словарей (Наверное, некорректно, т.к. изначально неупорядоченные сущности, однако, поскольку диктионари
			// реализуется IEnumerable и можно применить orderby, и в данной ситуации отлично работает, решил сократить код (иначе пришлось бы разворачивать ещё в один блок кода) )
			wordsDict = wordsDict.OrderByDescending(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
			shortWordsDict = shortWordsDict.OrderByDescending(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);

			List<string> commonlyUsedShortWord = new List<string>();
			List<string> commonlyUsedWord = new List<string>();
			List<string> shortWordsPairs = new List<string>();
			List<string> finalStrings = new List<string>();
			List<string> wordsPairs = new List<string>();

			int shortWordCount = 0;
			int wordCount = 0;

			foreach (string key in wordsDict.Keys)
			{	// Цикл выводящий в отдельный список наиболее часто повторяемые слова, и выводящий в отдельный список заготовки с парами ключ-значение для будущей строки draw.
				
				if (nonRepeatable && wordsDict[key] == 1)
				{
					continue;
				}	
				if (wordsDict[key] > wordCount)
				{
					commonlyUsedWord.Add($"'{key}'");
					wordCount = (wordsDict[key]);
				}
				else if (wordsDict[key] == wordCount)
				{
					commonlyUsedWord.Add($"'{key}'");
				}

				wordsPairs.Add($"'{key}'".PadRight(maxWordLength+3) + $"| {wordsDict[key]}\n");
			}

			foreach (string key in shortWordsDict.Keys)
			{   // Цикл выводящий в отдельный список наиболее часто повторяемые короткие слова, и выводящий в отдельный список заготовки с парами ключ-значение для будущей строки draw.
				if (nonRepeatable && shortWordsDict[key] == 1)
				{
					continue;
				}
				if (shortWordsDict[key] > shortWordCount)
				{
					commonlyUsedShortWord.Add($"'{key}'");
					shortWordCount = (shortWordsDict[key]);
				}
				else if (shortWordsDict[key] == shortWordCount)
				{
					commonlyUsedShortWord.Add($"'{key}'");
				}

				shortWordsPairs.Add($"'{key}'".PadRight(maxShortWordLength+3) + $"| {shortWordsDict[key]}\n");
			}

			if (wordsPairs.Count == 0 && shortWordsPairs.Count == 0)
			{
				return "Неповторяемых слов не обнаружено!";
			}

			// Далее происходит "подшивание" полученых результатов в единую строку для отрисовки в универсальной библиотеке "draw"

			commonlyUsedWord.Sort();
			commonlyUsedWord = new List<string> { $"{wordCount} раз(а): ", string.Join(", ", commonlyUsedWord) };
			
			if (commonlyUsedShortWord.Count > 0)
			{
				commonlyUsedShortWord.Sort();
				commonlyUsedShortWord = new List<string> { "\n\n[cl][bc]--- Наиболее часто употребляемые \"короткие слова\" (частицы, предлоги итд):\n",
															$"{shortWordCount} раз(а): ", string.Join(", ", commonlyUsedShortWord) };
				commonlyUsedWord = commonlyUsedWord.Concat(commonlyUsedShortWord).ToList();
			}

			commonlyUsedWord.Add("\n\n[cl][bc]--- Список слов:\n");

			finalStrings = commonlyUsedWord.Concat(wordsPairs).ToList();

			if (shortWordsPairs.Count > 0)
			{
				finalStrings.Add("\n[cl][bc]--- Список \"коротких слов\" (частицы, предлоги итд):\n");
				finalStrings = finalStrings.Concat(shortWordsPairs).ToList();
			}

			return string.Join("", finalStrings);
		}
	}
}
