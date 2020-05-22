using System;
using Outputlib;   // Кастомная библиотека вывода текста в цвете


namespace XmasTree
{   // Написать программу, которая запрашивает с клавиатуры число N и выводит на экран изображение ЁЛКИ, состоящее из N элементов:
	class Program
	{
		static void Main(string[] args)
		{
			string str;
			byte num = 0;
			int error = -1;
			bool numbered = true;
			Console.Clear();
			while (true)
			{
				Output.Print("b", "g", "\n ОТРИСОВКА ЁЛКИ ИЗ ЗАДАННОГО КОЛЛИЧЕСТВА ЭЛЕМЕНТОВ \n");
				//Введём несколько уточняющих принтов отображающихся в случае некорректного ввода
				if (error == 1)
				{
					Output.Print("r", "", new string[]
						{"К вводу допускаются целые положительные числа [0 < вводимое число < 256] или фраза 'exit'!!!\n",
						 "Ёлка из 6 элементов полностью умещается в окно командной строки стандартного размера.",
						 "Ёлка размером до 58 элементов корректно отрисовывается в окне командной строки стандартного размера."});
				}
				else if (error == 0)
				{
					XmasTree triangle = new XmasTree(num);
					triangle.Draw(numbered);
				}
				Console.Write("\nВведите \"exit\", \"s\" для смены отрисовки ИЛИ колличество строк для ОТРИСОВКИ треугольника:  ");
				str = Console.ReadLine().Trim().ToLower();

				// Осуществим предварительную проверку ввода на пустую строку или на наличие команды выхода:
				if      (str == "exit") { Console.Clear(); break; }
				else if (str == ""    ) { Console.Clear(); continue; }
				else if (str == "s"   ) { if (numbered) numbered = false; else numbered = true; }
				// Осуществим дальнейшую обработку ввода:
				else
				{
					if (Byte.TryParse(str, out num) && num != 0) error = 0;
					else error = 1;
				}
				Console.Clear();
			}

		}
	}
	class XmasTree
	{
		private byte strings;  // Колличество строк треугольника
		public XmasTree(byte snum) => strings = snum;
		public void Draw(bool numbered) { if (numbered) Draw_Numbered(); else Draw_Unnambered(); }
		private void Draw_Unnambered()
		{  // Метод простой отрисовки ёлки
			Console.ForegroundColor = ConsoleColor.Green;
			for (int x = 1; x <= strings; x++)
			{
				for (byte i = 0; i < x; i++)
				{
					string str = new string('*', 1 + i * 2);
					string filler = new string(' ', strings - 1 - (str.Length - 1) / 2);
					Console.WriteLine(filler + str);
				}
			}
			Console.ResetColor();
		}
		private void Draw_Numbered()
		{  // Метод отрисовки ёлки с пронумероваными элементами
			Console.ForegroundColor = ConsoleColor.Green;
			for (int x = 1; x <= strings; x++)
			{
				for (byte i = 0; i < x; i++)
				{
					string str = new string('*', 1 + i * 2);
					string num = "   | ";
					string filler = new string(' ', strings - 1 - (str.Length - 1) / 2);
					if (i == 0) { num = $"{x}| ".PadLeft(5); }
					Console.WriteLine(num + filler + str);
				}
			}
			Console.ResetColor();
		}
	}
}
