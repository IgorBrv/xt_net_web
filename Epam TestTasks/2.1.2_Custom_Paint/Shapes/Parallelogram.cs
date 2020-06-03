using System;

namespace Custom_Paint
{
	class Parallelogram : Rectangle
	{   // Паралеллограмм
		protected int angle;	// угол наклона паралеллограмма

		public Parallelogram(int x, int y, int sideA, int sideB, int angle, Colors color) : base(x, y, sideA, sideB, color)
		{
			this.angle = angle;
			about[0] = "Паралеллограмм".PadRight(14);
			about.Add($" Угол: {angle};");
		}

		public new double GetArea()
		{   // Метод возвращающий площадь фигуры
			return Math.Round(sideA * sideB * Math.Sin(angle), 2);
		}
	}
}
