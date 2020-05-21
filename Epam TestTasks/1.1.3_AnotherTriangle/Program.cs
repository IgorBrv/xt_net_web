using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Triangle
{  // Написать программу, которая запрашивает с клавиатуры число N и выводит на экран изображение равностороннего треугольника, состоящее из N строк:
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
				Console.WriteLine("\n ОТРИСОВКА ТРЕУГОЛЬНИКА ИЗ ЗАДАННОГО КОЛЛИЧЕСТВА СТРОК \n");
				Console.ResetColor();
				//Введём несколько уточняющих принтов отображающихся в случае некорректного ввода
				if (error == 1)
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.Write("К вводу допускаются целые положительные числа [0 < вводимое число < 256] или фраза 'exit'!!!\n");
					Console.WriteLine("\n24 строка полностью умещаются в окно командной строки стандартного размера.");
					Console.WriteLine("Треугольник размером до 58 строк корректно отрисовывается в окне командной строки стандартного размера.");
					Console.ResetColor();
				}
				else if (error == 0)
				{
					Equi_Triangle triangle = new Equi_Triangle(num);
					triangle.Draw_Numbered();
				}
				Console.WriteLine("\nВведите \"exit\" для выхода ИЛИ желаемое колличество строк для ОТРИСОВКИ треугольника:\n");
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
	class Equi_Triangle
	{
		private byte strings;  // Колличество строк треугольника
		public Equi_Triangle(byte snum)
		{
			strings = snum;
		}
		public void Draw()
		{
			Console.ForegroundColor = ConsoleColor.Green;
			for (byte i = 0; i < strings; i++)
			{
				string str = new string('*', 1 + i*2);
				string filler = new string(' ', strings-1-(str.Length-1)/2);
				Console.WriteLine(filler + str);
			}
			Console.ResetColor();
		}

		public void Draw_Numbered()
		{
			Console.ForegroundColor = ConsoleColor.Green;
			for (byte i = 0; i < strings; i++)
			{
				string num = $"{i+1}| ".PadLeft(5);
				string str = new string('*', 1 + i * 2);
				string filler = new string(' ', strings - 1 - (str.Length - 1) / 2);
				Console.WriteLine(num + filler + str);
			}
			Console.ResetColor();
		}
	}
}
