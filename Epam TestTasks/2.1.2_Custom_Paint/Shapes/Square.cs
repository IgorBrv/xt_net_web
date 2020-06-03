using System;

namespace Custom_Paint
{
	class Square : Shapes
	{   // Квадрат

		protected int sideA;

		public Square(int x, int y, int sideA, Colors color) : base(x, y, color)
		{
			this.sideA = sideA;
			about[0] = "Квадрат".PadRight(14);
			about.Add($" Сторона: {sideA};");
		}

		public double GetArea()
		{   // Метод возвращающий площадь фигуры
			return Math.Round(Math.Pow(sideA, 2), 2);
		}
	}
}
