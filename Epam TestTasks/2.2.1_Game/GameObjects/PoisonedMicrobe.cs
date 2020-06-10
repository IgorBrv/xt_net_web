using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.GameObjects
{
	class PoisonedMicrobe : Microbe
	{
		public PoisonedMicrobe(int PosX, int PosY, int age, ObjectDeath objectDeath, char[,] drawBuffer) : base(PosX, PosY, age, objectDeath, drawBuffer)
		{
			Sign = 'Ж';
			profit = -3;
		}
	}
}	

