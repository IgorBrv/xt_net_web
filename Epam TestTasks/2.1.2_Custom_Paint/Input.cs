using System;

namespace Custom_Paint
{
	class Input
	{	// Вспомогательный подкласс проверяющий корректность ввода по заданным в переменной mode критериям
		static public bool Get(ref int[] mode, ref bool error, string input)
		{
			bool exit = false;
			bool success;
			int temp;

			switch (mode[0])
			{   // Обработка ввода, взависимости от выбранного режима

				case 0:     // Первый режим, предполагает ввод целых чисел для выбора пунктов меню, вторым элементом в списке режима является ограничитель количества пунктов.

					success = Int32.TryParse(input, out temp);
					
					if (success)
					{
						if (temp > mode[2] || temp < mode[1])
						{
							error = true;
						}
					}
					else error = true;

					if (!error) exit = true;
					break;

				case 1:     // Второй режим, предполагает ввод нескольких целых чисел через запятую, вторым элементом в списке режима является желаемое количество элементов
					string[] splitted = input.Split(new char[] { ',' , ' '}, StringSplitOptions.RemoveEmptyEntries);

					if (splitted.Length == mode[1])
					{
						foreach (string i in splitted)
						{
							success = Int32.TryParse(i, out temp);
							if (success)
							{
								if (temp > mode[3] || temp < mode[2])
								{
									error = true;
								}
							}
							else
							{ 
								error = true;
							}
						}
					}
					else error = true;

					if (!error) exit = true;
					break;

				case 2:     // Третий режим, предполагает ввод нескольких слов через пробел, вторым элементом в списке режима является желаемое количество слов
					if (input.Length != 0 && input.Split().Length <= mode[1])
					{
						foreach (char i in input.Replace(" ", ""))
						{
							if (!Char.IsLetter(i))
							{
								error = true;
							}
						}
					}
					else error = true;
					if (!error) exit = true;
					break;

				case 3:     // Третий режим, предполагает вывод информации на экран без проверки ввода
					exit = true;
					break;

			}
			return exit;
		}
	}
}
