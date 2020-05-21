using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace XmasTree
{  // Написать программу, которая запрашивает с клавиатуры число N и выводит на экран изображение ЁЛКИ, состоящее из N элементов:
	class Program
	{
		static void Main(string[] args)
		{
			string str;
			byte num = 0;
			int error = -1;
			Console.Clear();
			while (true)
			{
				Console.BackgroundColor = ConsoleColor.Green;
				Console.ForegroundColor = ConsoleColor.Black;
				Console.WriteLine("\n ОТРИСОВКА ЁЛКИ ИЗ ЗАДАННОГО КОЛЛИЧЕСТВА ЭЛЕМЕНТОВ \n");
				Console.ResetColor();
				//Введём несколько уточняющих принтов отображающихся в случае некорректного ввода
				if (error == 1)
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.Write("К вводу допускаются целые положительные числа [0 < вводимое число < 256] или фраза 'exit'!!!\n");
					Console.WriteLine("\nЁлка из 6 элементов полностью умещается в окно командной строки стандартного размера.");
					Console.WriteLine("Ёлка размером до 58 элементов корректно отрисовывается в окне командной строки стандартного размера.");
					Console.ResetColor();
				}
				else if (error == 0)
				{
					XmasTree triangle = new XmasTree(num);
					triangle.Draw_Numbered();
				}
				Console.WriteLine("\nВведите \"exit\" для выхода ИЛИ желаемое колличество элементов для ОТРИСОВКИ ёлки:\n");
				str = Console.ReadLine().Trim();
				// Осуществим предварительную проверку ввода на пустую строку или на наличие команды выхода:
				if (str.ToLower() == "exit") { Console.Clear(); break; }
				else if (str == "") { Console.Clear(); continue; }
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
		public XmasTree(byte snum)
		{
			strings = snum;
		}
		public void Draw()
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
		public void Draw_Numbered()
		{  // Метод отрисовки ёлки с пронумероваными элементами
			Console.ForegroundColor = ConsoleColor.Green;
			for (int x = 1; x <= strings; x++)
			{
				for (byte i = 0; i < x; i++)
				{
					string str = new string('*', 1 + i * 2);
					string filler = "";
					string num = "   | ";
					filler = new string(' ', strings - 1 - (str.Length - 1) / 2);
					if (i == 0) { num = $"{x}| ".PadLeft(5); }
					Console.WriteLine(num + filler + str);
				}
			}
			Console.ResetColor();
		}
	}
}
