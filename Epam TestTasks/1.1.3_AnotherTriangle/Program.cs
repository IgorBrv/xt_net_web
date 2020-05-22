﻿using System;
using Outputlib;   // Кастомная библиотека вывода текста в цвете


namespace AnotherTriangle
{   // Написать программу, которая запрашивает с клавиатуры число N и выводит на экран изображение равностороннего треугольника, состоящее из N строк:
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
				Output.Print("b", "g", "\n ОТРИСОВКА ТРЕУГОЛЬНИКА ИЗ ЗАДАННОГО КОЛЛИЧЕСТВА СТРОК \n");

				if (error == 1)
				{
					Output.Print("r", "", new string[]
						{"К вводу допускаются целые положительные числа [0 < вводимое число < 256] или фраза 'exit'!!!\n",
						 "25 строк полностью умещаются в окно командной строки стандартного размера.",
						 "Треугольник размером до 58 строк корректно отрисовывается в окне командной строки стандартного размера." });
				}
				else if (error == 0)
				{
					Equi_Triangle triangle = new Equi_Triangle(num);
					triangle.Draw(numbered);
				}

				Console.Write("\nВведите \"exit\", \"s\" для смены отрисовки ИЛИ колличество строк для ОТРИСОВКИ треугольника:  ");
				str = Console.ReadLine().Trim().ToLower();

				// Осуществим предварительную проверку ввода на пустую строку или на наличие команды выхода:
				if      (str == "exit") { Console.Clear(); break;    }
				else if (str == ""    ) { Console.Clear(); continue; }
				else if (str == "s"   ) { if (numbered) numbered = false; else numbered = true; }
				else   // Осуществим дальнейшую обработку ввода:
				{
					if (Byte.TryParse(str, out num) && num != 0) error = 0;
					else error = 1;
				}
				Console.Clear();
			}
		}
	}
	class Equi_Triangle
	{   // Класс описывающий равносторонний треугольник
		private byte strings;
		public Equi_Triangle(byte snum) => strings = snum;
		public void Draw(bool numbered) { if (numbered) Draw_Numbered(); else Draw_Unnambered(); }

		private void Draw_Unnambered()
		{   // Метод производящий стандартную отрисовку
			Console.ForegroundColor = ConsoleColor.Green;
			for (byte i = 0; i < strings; i++)
			{
				string str = new string('*', 1 + i*2);
				string filler = new string(' ', strings-1-(str.Length-1)/2);
				Console.WriteLine(filler + str);
			}
			Console.ResetColor();
		}
		private void Draw_Numbered()
		{   // Метод производящий отрисовку с построчным нумерованием
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
