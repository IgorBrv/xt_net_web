using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Outputlib;
using ConsoleDraw;

namespace SnakeGame
{
	class Runtime
	{
		private int[] stats;
		private bool exit = false;
		private readonly int speed;
		private readonly string name;
		private readonly char[,] drawBuffer;


		private readonly Snake snake;
		private readonly Drawer drawer;
		private readonly GameOver gameOver;
		private readonly Generate generate;
		private readonly SetStats setStats;
		private readonly ObjectDeath objectDeath;
		private readonly List<AbstractUnit> units;
		private readonly List<AbstractUnit> toRemove;
		private readonly List<int[]> freeCells = new List<int[]>();
		private ConsoleKeyInfo key = new ConsoleKeyInfo();

		public Runtime(char[,] drawBuffer, int speed, string name)
		{
			this.name = name;
			this.speed = speed;
			this.drawBuffer = drawBuffer;

			stats = new int[2];
			setStats = UpdateStatistics;
			objectDeath = ObjectDeathFunc;
			gameOver = delegate { exit = true; };
			snake = new Snake((int)Math.Round((double)(drawBuffer.GetLength(1) - 1) / 2), (int)Math.Round((double)(drawBuffer.GetLength(0) - 1) / 2), drawBuffer, gameOver, objectDeath, setStats);
			units = new List<AbstractUnit>() { };
			toRemove = new List<AbstractUnit>() { };
			drawer = new Drawer(drawBuffer, freeCells);
			generate = new Generate(drawBuffer, units, objectDeath, freeCells);
		}

		public void Run()
		{
			GetKeyAsync();	// Запускаем асинхронный метод считывания ввода с клавиатуры
			drawer.PrepareField(name);	// Отрисовываем игровое поле
			Console.CursorVisible = false;  // Отключаем отображение курсора
			generate.Walls();	// Генерируем стены на карту

			while (!exit)	// Запускаем игровой цикл, переменная exit модифицируется при помощи делегата передаваемого в объект змейки
			{

				if (toRemove.Count > 0)
				{	// Проверяем список умерших юнитов, и удаляем их из списка юнитов.
					foreach (AbstractUnit unit in toRemove)
					{
						drawBuffer[unit.PosY, unit.PosX] = ' ';
						units.Remove(unit);
					}
					toRemove.Clear();
				}

				for (int i = 0; i < units.Count; i++)
				{	// Отрисовываем на поле элементы AbstractUnit из сгенерированного списка юнитов Units
					if (units[i] is IMovable)
					{   // Элементы реализующие интерфейс IMovable производят движения и отрисовывают себя сами
						(units[i] as IMovable).Move(drawBuffer);
					}
					else
					{   // Элементы класса AbstractUnit необходимо отрисовать вручную.
						drawBuffer[units[i].PosY, units[i].PosX] = units[i].Sign;
					}
					// Передаём элемент в змейку для проверки координат и реализации механик поедания, в случае если координаты совпадают
					snake.CheckPosition(units[i]);
				}

				snake.Move(drawBuffer);	// Двигаем змейку

				freeCells.Clear();  // Очищаем список пустых ячеек

				drawer.Draw();	// Отрисовываем проделанную работу

				// Проверяем ввод с клавиатуры
				CheckSnakeDirection();

				// Генерируем игровые объекты в пустых ячейках используя список пустых ячеек
				generate.Units();

				// Осуществляем паузу между отрисовками
				Thread.Sleep(speed);

			}

			// Отрисовка экрана GAME OVER в случае завершения цикла while
			
			foreach (AbstractUnit unit in units)
			{	// Убиваем асинхронные процессы живущих юнитов
				if (unit is IAging)
				{
					(unit as IAging).ItsTimeToDie();
				}
			}

			Console.Clear();
			Console.SetCursorPosition(53, 13);
			Output.Print("b", "r", "  GAME OVER  ");
			Console.SetCursorPosition(53, 14);
			Output.Print("w", "", " press enter ");
			Console.SetCursorPosition(53, 27);
			Output.Print("w", "", $"  score: {stats[0]} ");
			Console.ReadKey();
			Console.CursorVisible = true;
		}

		private void CheckSnakeDirection()
		{	// Метод проверяющий состояние переменной ввода с клавиатуры и передающий значение в объект змейки

			switch (key.Key)
			{
				case ConsoleKey.UpArrow:
					snake.Direction = 'u';
					key = new ConsoleKeyInfo();
					break;
				case ConsoleKey.DownArrow:
					snake.Direction = 'd';
					key = new ConsoleKeyInfo();
					break;
				case ConsoleKey.LeftArrow:
					snake.Direction = 'l';
					key = new ConsoleKeyInfo();
					break;
				case ConsoleKey.RightArrow:
					snake.Direction = 'r';
					key = new ConsoleKeyInfo();
					break;
				case ConsoleKey.Escape:
					exit = true;
					break;
			}
		}

		private async void GetKeyAsync()
		{	// Асинхронный метод ввода направления с клавиатуры
			await Task.Run(() => GetKey());
		}

		private void GetKey()
		{   // Асинхронный метод ввода направления с клавиатуры. Завершается по состоянию переменной exit класса.
			while (!exit)
			{
				key = Console.ReadKey();
				Thread.Sleep(50);
			}
		}

		private void ObjectDeathFunc(AbstractUnit unit)
		{	// Метод вызываемый через делегат при смерти или съедании объекта. Добавляет объект в список на удаление и останавливает жизненный цикл живущих объектов
			if (unit is IAging && (unit as IAging).Age>1)
			{
				(unit as IAging).ItsTimeToDie();
			}
			if (!toRemove.Contains(unit))
			{
				toRemove.Add(unit);
			}
		}

		private void UpdateStatistics(int[] stats)
		{	// Метод обновления статистики вызываемый через делегат в змейке
			this.stats = stats;
			drawer.UpdateScore(stats);
		}

	}
}
