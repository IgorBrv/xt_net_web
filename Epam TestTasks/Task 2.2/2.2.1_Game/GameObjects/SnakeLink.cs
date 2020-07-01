using System;
using System.Collections.Generic;
using System.Linq;
using Outputlib;
using ConsoleDraw;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;

namespace SnakeGame
{
	class SnakeLink : AbstractMovableUnit
	{	// Класс объекта звена змейки, реализует интерфейс IMovable
		public int newPosX;
		public int newPosY;
		public override char Sign { get; protected set; } = 'o';
		public SnakeLink(int PosX, int PosY) : base(PosX, PosY) { }

		public void AddPosition(int newPosX, int newPosY)
		{	// Вспомогательный метод для реализации движения в составе змейки
			this.newPosX = newPosX;
			this.newPosY = newPosY;
		}
		public override void Move(Char[,] field)
		{
			field[PosY, PosX] = ' ';
			PosX = newPosX;
			PosY = newPosY;
			field[PosY, PosX] = Sign;
		}

	}

}
