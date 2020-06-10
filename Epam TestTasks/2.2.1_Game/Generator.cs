using ConsoleDraw;
using SnakeGame.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SnakeGame
{
	class Generate
	{   // Класс генерирующий объекты на игровом поле.
		private readonly char[,] drawBuffer;
		private readonly List<int[]> freeCells;
		private readonly ObjectDeath objectDeath;
		private readonly List<AbstractUnit> units;

		public Generate(char[,] drawBuffer, List<AbstractUnit> units, ObjectDeath objectDeath, List<int[]> freeCells)
		{
			this.units = units;
			this.freeCells = freeCells;
			this.drawBuffer = drawBuffer;
			this.objectDeath = objectDeath;
		}

		public void Units()
		{	// Метод генерирующий единичные юниты на игровом поле
			int count = 0;
			int tryings = 0;
			int healCount = 0;
			int poisonedMicrobCount = 0;

			foreach (AbstractUnit unit in units)
			{	// Пересчитаем колличество живых юнитов не являющихся кирпичами
				if (!(unit is Brick))
				{
					count++;
					if (unit is Heal) healCount++;
					else if (unit is PoisonedMicrobe) poisonedMicrobCount++;
				}
			}

			if (freeCells.Count > 40)
			{
				while (count < 15)
				{
					if (tryings > 30) break; // Ограничиваем число попыток создания элементов, если змейка будет настолько большой что заполонит всё поле не оставив свободного места

					int[]position = freeCells[Rand.Get(0, freeCells.Count - 1)];

					if (CheckPosition(position))	// Проверим безопасность координатов
					{
						switch (Rand.Get(1, 5))
						{
							case 1:
								units.Add(new Microbe(position[1], position[0], Rand.Get(10, 50), objectDeath, drawBuffer));
								count++;
								break;
							case 2:
								units.Add(new FreeLink(position[1], position[0], Rand.Get(10, 50), objectDeath));
								count++;
								break;
							case 3:
								if (healCount < 1)
								{
									units.Add(new Heal(position[1], position[0], Rand.Get(10, 50), objectDeath, drawBuffer));
									healCount++;
									count++;

								}
								break;
							case 4:
								if (poisonedMicrobCount < 3)
								{
									units.Add(new PoisonedMicrobe(position[1], position[0], Rand.Get(10, 50), objectDeath, drawBuffer));
									poisonedMicrobCount++;
									count++;
								}
								break;
						}
					}
					tryings++;
				}
			}
		}

		public void Walls()
		{   // Метод генерирующий стены на игровом поле
			int count = 10;
			while (count>0)
			{
				int snakePosX = (int)Math.Round((double)(drawBuffer.GetLength(1) - 1) / 2);
				int snakePosY = (int)Math.Round((double)(drawBuffer.GetLength(0) - 1) / 2);
				int posX = Rand.Get(0, drawBuffer.GetLength(0) - 1);
				int posY = Rand.Get(0, drawBuffer.GetLength(1) - 1);
				int lenght = Rand.Get(1, 4);
				for (int i = posX-lenght; i < posX+lenght; i++)
				{
					if (i >= 0 && i <= drawBuffer.GetLength(0) - 1)
					{
						for (int j = posY - lenght; j < posY + lenght; j++)
						{
							if (j >= 0 && i < drawBuffer.GetLength(1) - 2)
							{
								if (j < snakePosX + 7 && j > snakePosX - 7 && i < snakePosY + 7 && i > snakePosY - 7) continue;
								units.Add(new Brick(j, i));
							}
						}
					}
				}
				count--;
			}
		}


		private bool CheckPosition(int[] position)
		{   // Сложная проверка координат, чтобы микробы не грызли стены и не спавнились у змейки перед носом

			bool[] sucsess = new bool[2];

			if (position[0] > 0 && position[0] < drawBuffer.GetLength(0)-1)
			{
				if (drawBuffer[position[0] + 1, position[1]] == '!' && drawBuffer[position[0] - 1, position[1]] == '!')
				{
					sucsess[0] = true;
				}
			}
			else if (position[0] == 0)
			{
				if (drawBuffer[position[0] + 2, position[1]] == '!')
				{
					sucsess[0] = true;
				}
			}
			else if (position[0] == drawBuffer.GetLength(0) - 1)

			{
				if (drawBuffer[position[0] - 2, position[1]] == '!')
				{
					sucsess[0] = true;
				}
			}

			if (position[1] > 0 && position[1] < drawBuffer.GetLength(1) - 1)
			{
				if (drawBuffer[position[0], position[1] + 1] == '!' && drawBuffer[position[0], position[1] - 1] == '!')
				{
					sucsess[1] = true;
				}
			}
			else if (position[1] == 0)
			{
				if (drawBuffer[position[0], position[1] + 2] == '!')
				{
					sucsess[1] = true;
				}
			}
			else if (position[1] == drawBuffer.GetLength(1) - 1)
			{
				if (drawBuffer[position[0], position[1] - 2] == '!')
				{
					sucsess[1] = true;
				}
			}

			if (sucsess[0] && sucsess[1]) return true;
			return false;
		}

	}
}
