using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SnakeGame
{
	class Microbe : AbstractMovableEatableAgingUnit
	{
		private char direction;
		private int divider = 0;
		private readonly int baseX;
		private readonly int baseY;
		private readonly char axis;
		public override char Sign { get; protected set; } = 'ж';

		public Microbe(int PosX, int PosY, int age, ObjectDeath objectDeath, char[,] drawBuffer) : base(PosX, PosY, age, objectDeath)
		{
			Profit = -1;

			switch (Rand.Get(1, 3))
			{   // выбор рандомного направления движения, происходит при создании объекта
				case 1:
					if (PosX == 0) this.PosX = 1;
					else if (PosX == drawBuffer.GetLength(1) - 1) this.PosX = drawBuffer.GetLength(1) - 2;
					axis = 'x';
					break;
				case 2:
					if (PosY == 0) this.PosY = 1;
					else if (PosY == drawBuffer.GetLength(0) - 1) this.PosY = drawBuffer.GetLength(0) - 2;
					axis = 'y'; break;
			}
			switch (Rand.Get(1, 3))
			{
				case 1:
					direction = '-'; break;
				case 2:
					direction = '+'; break;
			}

			// Вспомогательные координаты, используемые для реалзации движения объекта
			baseX = this.PosX;
			baseY = this.PosY;
		}

		public override void Move(char[,] field)
		{   // Реализация интерфейса IMovable, микроб движется по рандомной оси (x, y) определяемой в конструкторе объекта в два раза медленнее змейки.
			if (divider < 1)
			{
				field[PosY, PosX] = ' ';

				switch (axis)
				{
					case 'x':
						switch (direction)
						{
							case '-':
								if (PosX >= baseX)
								{
									PosX--;
									if (PosX < baseX) direction = '+';
								}
								break;
							case '+':
								if (PosX <= baseX)
								{
									PosX++;
									if (PosX > baseX) direction = '-';
								}
								break;
						}
						break;
					case 'y':
						switch (direction)
						{
							case '-':
								if (PosY >= baseY)
								{
									PosY--;
									if (PosY < baseY) direction = '+';
								}
								break;
							case '+':
								if (PosY <= baseY)
								{
									PosY++;
									if (PosY > baseY) direction = '-';
								}
								break;
						}
						break;
				}
				divider++;
			}
			else divider = 0;

			field[PosY, PosX] = Sign;
		}

	}
}
