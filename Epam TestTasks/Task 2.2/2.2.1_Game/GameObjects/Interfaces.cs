using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
	public interface IMovable
	{
		void Move(Char[,] field);
	}

	public interface IEatable
	{
		int GetProfit();
	}

	public interface IAging
	{
		int Age { get; set; }
		void ItsTimeToDie();
	}
}
