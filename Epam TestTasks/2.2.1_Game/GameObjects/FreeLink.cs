using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Cache;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SnakeGame
{
	class FreeLink : AbstractEatableAgingUnit
	{   // Класс объекта свободного звена, реализует интерфейс IEatable
		
		public override char Sign { get; protected set; } = 'o';

		public FreeLink(int PosX, int PosY, int age, ObjectDeath objectDeath) : base(PosX, PosY, age, objectDeath)
		{
			Profit = 1;
		}

	}
}
