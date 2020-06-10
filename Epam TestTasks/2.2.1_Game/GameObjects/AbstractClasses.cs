using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SnakeGame
{
	abstract class AbstractUnit
	{	// Абстрактный класс объекта на игровом поле. Имеет координаты.
		public int PosX;
		public int PosY;
		abstract public char Sign { get; set; }

		public AbstractUnit(int PosX, int PosY)
		{
			this.PosX = PosX;
			this.PosY = PosY;
		}
	}

	abstract class AbstractEatableUnit : AbstractUnit, IEatable
	{	// Абстрактный класс СЪЕДОБНОГО объекта на игровом поле. Имеет координаты и коэффицент полезности при съедании
		public int profit;

		public AbstractEatableUnit(int PosX, int PosY) : base(PosX, PosY) { }

		public int GetProfit()
		{
			return profit;
		}
	}

	abstract class AbstractMovableUnit : AbstractUnit, IMovable
	{   // Абстрактный класс СЪЕДОБНОГО объекта на игровом поле. Имеет координаты и коэффицент полезности при съедании
		public AbstractMovableUnit(int PosX, int PosY) : base(PosX, PosY) { }

		public abstract void Move(Char[,] field);
	}

	abstract class AbstractMovableEatableUnit : AbstractEatableUnit, IMovable
	{   // Абстрактный класс ДВИЖУЩЕГОСЯ СЪЕДОБНОГО объекта на игровом поле. Имеет координаты и коэффицент полезности при съедании, реализует метод движения
		public AbstractMovableEatableUnit(int PosX, int PosY) : base(PosX, PosY) { }
		public abstract void Move(Char[,] field);
	}


	abstract class AbstractAgingUnit : AbstractUnit, IAging
	{
		protected readonly ObjectDeath objectDeath;

		protected bool deathTrigger = false;
		public int Age { get; set; }
		public AbstractAgingUnit(int PosX, int PosY, int age, ObjectDeath objectDeath) : base(PosX, PosY)
		{
			Age = age;
			this.objectDeath = objectDeath;
			AsyncAgeControl();
		}
		public void ItsTimeToDie()
		{
			deathTrigger = true;
		}

		protected async void AsyncAgeControl()
		{
			await Task.Run(() => AgeControl());
		}

		protected void AgeControl()
		{
			while (Age > 0)
			{
				if (deathTrigger)
				{
					break;
				}
				Thread.Sleep(Age * 100);
				Age--;
			}
			objectDeath(this);
		}
	}


	abstract class AbstractEatableAgingUnit : AbstractAgingUnit, IEatable
	{   // Абстрактный класс СЪЕЖЛЮГЛШЛ СТАРЕЮЩЕГО объекта на игровом поле. Имеет координаты и коэффицент полезности при съедании, имеет счётчик старения

		public int profit;

		public AbstractEatableAgingUnit(int PosX, int PosY, int age, ObjectDeath objectDeath) : base(PosX, PosY, age, objectDeath) { }

		public int GetProfit()
		{
			return profit;
		}
	}

	abstract class AbstractMovableAgingUnit : AbstractAgingUnit, IMovable
	{   // Абстрактный класс ДВИЖУЩЕГОСЯ СТАРЕЮЩЕГО объекта на игровом поле. Имеет координаты, реализует метод движения, имеет счётчик старения
		
		public AbstractMovableAgingUnit(int PosX, int PosY, int age, ObjectDeath objectDeath) : base(PosX, PosY, age, objectDeath) { }

		public abstract void Move(Char[,] field);

	}

	abstract class AbstractMovableEatableAgingUnit : AbstractEatableAgingUnit, IMovable
	{   // Абстрактный класс ДВИЖУЩЕГОСЯ СТАРЕЮЩЕГО СЪЕДОБНОГО объекта на игровом поле. Имеет координаты и коэффицент полезности при съедании, реализует метод движения, имеет счётчик старения
				
		public AbstractMovableEatableAgingUnit(int PosX, int PosY, int age, ObjectDeath objectDeath) : base(PosX, PosY, age, objectDeath) { }

		public abstract void Move(Char[,] field);

	}
}
