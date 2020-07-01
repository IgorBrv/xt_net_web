using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ConsoleDraw;
using Outputlib;

namespace SnakeGame
{
	class Snake : AbstractMovableUnit
	{   // Класс объекта змейки

		public List<SnakeLink> links;   // Список звеньев змейки
		private readonly int[] stats;   // Статистика по съеденым/потеряным звеньям
		private readonly SetStats setStats;	// Делегат передающий статистику
		private readonly GameOver gameOver;    // Делегат выполняемый в случае смертельной аварии
		private readonly char[,] drawBuffer;	
		private readonly ObjectDeath objectDeath;	// Делегат передающий информацию о съедаемых объектах
		private readonly Dictionary<AbstractUnit, int> touched;	// Список элементов, с которыми произошли соприкосновения для реализации прозрачности остальных звеньев змейки этим элементоам
		private readonly Dictionary<AbstractUnit, int> touchedUpdates;
		private readonly char[] acceptedDirectionValues = { 'u', 'd', 'l', 'r' };

		public override char Sign { get; protected set; } = 'O';

		private char direction = 'u';
		public char Direction
		{	// Безопасное изменение направления движения змейки
			private get
			{
				return direction;
			}
			set
			{
				if (acceptedDirectionValues.Contains(value))
				{
					switch (value)
					{
						case 'u':
							if (direction != 'd')
							{
								direction = value;
							}
							break;
						case 'd':
							if (direction != 'u')
							{
								direction = value;
							}
							break;
						case 'l':
							if (direction != 'r')
							{
								direction = value;
							}
							break;
						case 'r':
							if (direction != 'l')
							{
								direction = value;
							}
							break;
					}
				}
			}
		}

		public Snake(int PosX, int PosY, char[,] drawBuffer, GameOver gameOver, ObjectDeath objectDeath, SetStats setStats) : base(PosX, PosY)
		{	// Конструктор
			this.setStats = setStats;
			this.gameOver = gameOver;
			this.drawBuffer = drawBuffer;
			this.objectDeath = objectDeath;

			stats = new int[2] { 0, 0 };
			touched = new Dictionary<AbstractUnit, int>();
			touchedUpdates = new Dictionary<AbstractUnit, int>();
			links = new List<SnakeLink>() { new SnakeLink(PosX, PosY + 1) };
		}

		public override void Move(Char[,] drawBuffer)
		{	// Реализация интерфейса IMovable

			for (int i = 0; i < links.Count; i++)
			{	// Передача новых координат звеньям змейки
				if (i == 0)
				{
					links[i].AddPosition(PosX, PosY);
				}
				else
				{
					links[i].AddPosition(links[i - 1].PosX, links[i - 1].PosY);
				}

			}

			// Старое положение заменяется на символ пробела в буффере отрисовки для затирания
			drawBuffer[PosY, PosX] = ' ';

			switch (direction)
			{	// Изменения позиции в соответствии с текущим направлением
				case 'u':
					ChangeY(-1); break;
				case 'd':
					ChangeY(1); break;
				case 'l':
					ChangeX(-1); break;
				case 'r':
					ChangeX(1); break;
			}

			// Отрисовка нового положения в буффере отрисовки
			drawBuffer[PosY, PosX] = Sign;

			// Передача команды движения звеньям змейки
			foreach (SnakeLink link in links)
			{
				link.Move(drawBuffer);
				if (PosX == link.PosX && PosY == link.PosY)
				{	// Геймовер при столкновении змейки со своими же звеньями
					gameOver();
				}
			}

			// Система учёта координатов объектов съеденых змейкой
			foreach (AbstractUnit unit in touched.Keys)
			{
				touchedUpdates.Add(unit, touched[unit] - 1);
			}
			foreach (AbstractUnit unit in touchedUpdates.Keys)
			{
				if (touchedUpdates[unit] > 0) touched[unit] = touchedUpdates[unit];
				else touched.Remove(unit);
			}
			touchedUpdates.Clear();

		}

		public void CheckPosition(AbstractUnit unit)
		{	// Метод, сверяющий координаты входного элемента с координатами всех звеньев змейки
			if (unit.PosX == PosX && unit.PosY == PosY)
			{
				touched.Add(unit, links.Count);
				GetProfit(unit);
			}
			else
			{
				for (int i = 0; i < links.Count; i++)
				{
					if (!touched.ContainsKey(unit))
					{
						if (unit.PosX == links[i].PosX && unit.PosY == links[i].PosY)
						{
							touched.Add(unit, links.Count - i + 1);
							GetProfit(unit);
						}
					}
				}
			}
		}

		private void GetProfit(AbstractUnit unit)
		{	// Метод реализующий поедание змейкой подобранного элемента если элемент поедаемый (Реализует интерфейс IEatable)
			if (unit is IEatable)
			{
				int profit = (unit as IEatable).GetProfit();

				if (profit == 0)
				{	// Нулевой профит, сценарий столкновения, змейка меняет направление движения
					if (direction == 'u' || direction == 'd')
					{
						switch (Rand.Get(1, 3))
						{
							case 1:
								direction = 'l'; break;
							case 2:
								direction = 'r'; break;
						}
					}
					else
					{
						switch (Rand.Get(1, 3))
						{
							case 1:
								direction = 'u'; break;
							case 2:
								direction = 'd'; break;
						}
					}
				}
				else if (profit > 0)
				{	// Положительный профит, сценарий подбора нового звена, змейка добавляет к себе количество звеньев указанное в пепременной profit звена.
					for (int i = 0; i < profit; i++)
					{
						links.Add(new SnakeLink(unit.PosX, unit.PosY));
						stats[0] = stats[0] += 1;
						setStats(stats);
					}
				}
				else if (profit < 0)
				{   // Отрицательный профит, сценарий подбора отравы, змейка кушает микроба и теряет количество звеньев указанное в пепременной profit микроба.
					for (int i = 0; i < profit * -1; i++)
					{
						if (links.Count > 0)
						{
							SnakeLink toRemove = links[links.Count - 1];
							drawBuffer[toRemove.PosY, toRemove.PosX] = ' ';
							links.Remove(toRemove);
							stats[1] = stats[1] += 1;
							setStats(stats);
						}
						else
						{	// Геймовер если отравлениеи больше чем количество звеньев
							gameOver();
						}
					}
				}
				objectDeath(unit);	// Съеденый объект умирает
			}
			else
			{	// Если юнит несъедобный
				gameOver();
			}
		}
		private void ChangeX(int i)
		{   // Метод меняющий положения по оси Х
			if (PosX + i < 0)
			{
				PosX = drawBuffer.GetLength(1) - 2;
			}
			else if (PosX + i > drawBuffer.GetLength(1) - 1)
			{
				PosX = 0;
			}
			else PosX += i;
		}

		private void ChangeY(int i)
		{   // Метод меняющий положения по оси Y
			if (PosY + i < 0)
			{
				PosY = drawBuffer.GetLength(0)-1;
			}
			else if (PosY + i > drawBuffer.GetLength(0) - 1)
			{
				PosY = 0;
			}
			else PosY += i;
		}

	}

}
