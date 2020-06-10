using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
	class Brick : AbstractEatableUnit
	{
		public override char Sign { get; set; } = '▓';
		public Brick(int PosX, int PosY) : base(PosX, PosY)
		{
			profit = 0;
		}
	}
}
