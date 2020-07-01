using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Outputlib;

namespace Text_Analysis
{	
	class Program
	{	// Задан английский текст.Ваша задача понять, какие слова автор «любит» больше всего и подловить его на однообразности речи.Или, наоборот, похвалить за разнообразие.
		// Для каждого слова в тексте указать, сколько раз оно встречается.

		static readonly private string title = " Текстовый аналитик";
		static readonly private Draw draw = new Draw(title);		     // Создание объекта универсального отрисовщика интерфейса консоли

		[STAThread]
		static void Main()
		{
			bool exit = false;
			bool nonRepeatable = false;
			Dictionary<bool, string> repeatable = new Dictionary<bool, string> { { true, "ОТКЛ" }, { false, "ВКЛ" } };

			while (!exit)
			{   // Запуск цикла главного меню

				string[] strings = { "Выберите желаемое действие ", "", $"0. Выход\n1. Выбор файла *.txt\n2. Отображение НЕповторяемых слов: {repeatable[nonRepeatable]}\n\nИЛИ введите текст для обработки:\n" };
				string input = draw.Form(new int[] { 3 }, strings);

				switch (input)
				{
					case "":    // Обработка пустого ввода
						break;
					case "0":	// Выход
						exit = true;
						break;
					case "1":	// Открытие файла
						OpenFile(nonRepeatable);						
						break;
					case "2":	// Переключение отображения неповторяемых слов
						nonRepeatable = !nonRepeatable;
						break;
					default:	// Обработка ввода
						string[] procFormStrings = { "Наиболее часто употребляемые слова: ", "", TextProcessing.Begin(input, nonRepeatable) };
						draw.Form(new int[] { 3 }, procFormStrings);
						break;
				}
			}
		}

		private static void OpenFile(bool nonRepeatable)
		{   // Метод выводящий окно выбора файла и обрабатывающий файл

			OpenFileDialog ofd = new OpenFileDialog { Filter = "text *.txt | *.txt" };
			ofd.ShowDialog();

			if (ofd.FileName != string.Empty)
			{
				try
				{
					using (StreamReader sr = new StreamReader(ofd.FileName, Encoding.Default))      // открытие файла в стримридере
					{
						string[] procFormStrings = { "Наиболее часто употребляемые слова: ", "", string.Join("", TextProcessing.Begin(sr.ReadToEnd(), nonRepeatable)) };
						draw.Form(new int[] { 3 }, procFormStrings);
					}
				}
				catch (NotSupportedException)   // обработка исключения "неподдерживаемого файла"
				{
					draw.Form(new int[] { 3 }, new string[] { "", "Файл не поддерживавется!" }, true);
				}
				catch (Exception ex)            // обработка неизвестного исключения
				{
					draw.Form(new int[] { 3 }, new string[] { "", $"Произошла непредвиденная ошибка: {ex.Message}" }, true);
				}
			}
		}
	}
	

}
