using ConsoleDraw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Outputlib;

namespace SnakeGame
{
	class Drawer
	{	// Класс отрисовщика поля. Занимается отрисовкой и в процессе заполняет список пустых ячеек поля.
		private int[] score;
		private readonly char[,] drawBuffer;
		private readonly List<int[]> freeCells;

		public Drawer(char[,] drawBuffer, List<int[]> freeCells)
		{
			score = new int[2];
			this.freeCells = freeCells;
			this.drawBuffer = drawBuffer;
		}

		public void Draw()
		{   // Метод отрисовки и заполнения списка пустых ячеек

			// Отрисовка статистики:
			Console.SetCursorPosition(7, 28);
			Output.Print("b", "g", false, $"{score[0]}  ");
			Console.SetCursorPosition(23, 28);
			Output.Print("b", "g", false, $"{score[1]}  ");

			for (int i = 0; i < drawBuffer.GetLength(0); i++)
			{   // Отрисовываем игровое поле из буффера отрисовки
				for (int j = 0; j < drawBuffer.GetLength(1); j++)
				{
					if (drawBuffer[i, j] != '!')
					{
						Console.SetCursorPosition(j + 1, i + 1);
						switch (drawBuffer[i, j])
						{
							case 'ж': Output.Print("y", "", false, drawBuffer[i, j].ToString()); break;
							case 'Ж': Output.Print("", "r", false, drawBuffer[i, j].ToString()); break;
							case '8': Output.Print("b", "g", false, drawBuffer[i, j].ToString()); break;
							case '▓': Output.Print("w", "r", false, drawBuffer[i, j].ToString()); break;
							default: Console.Write(drawBuffer[i, j]); break;
						}
						if (drawBuffer[i, j] == ' ')
						{   // Заменяем символы пробела, которые оставляют за собой мёртвые элементы на нечитаемые символы
							drawBuffer[i, j] = '!';
						}
					}
					else
					{   // Добавляем адреса пустых ячеек буффера в список адресов пустых ячеек для генерацции в них объектов
						freeCells.Add(new int[] { i, j });
					}
				}
			}
			Console.SetCursorPosition(0, 28);
			Console.WriteLine();
		}

		public void PrepareField(string name)
		{   // Первичная отрисовка игрового поля

			Output.Print("b", "g", name.PadRight(drawBuffer.GetLength(1)+2));

			for (int i = 0; i < drawBuffer.GetLength(0); i++)
			{
				Output.Print("", "g", false, " ");
				for (int j = 0; j < drawBuffer.GetLength(1); j++)
				{
					Console.Write(' ');
				}
				Output.Print("", "g", false, " ");
				Console.WriteLine();
			}

			Output.Print("b", "g", " Счёт:".PadRight(drawBuffer.GetLength(1) + 2));
			Console.SetCursorPosition(15, 28);
			Output.Print("b", "g", false, "Потери:");
			if (drawBuffer.GetLength(1) >= 63)
			{
				Console.SetCursorPosition(54, 28);
				Output.Print("b", "g", false, "ESC: выход");
			}

			if (drawBuffer.GetLength(1) == 117)
			{
				Console.SetCursorPosition(106, 28);
				Output.Print("b", "g", false, "PAUSE: пауза");
			}
			Console.WriteLine();
		}

		public void UpdateScore(int[] score)
		{	// Метод принимающий сообщения о статистике игрового процесса
			this.score = score;
		}
	}
}
