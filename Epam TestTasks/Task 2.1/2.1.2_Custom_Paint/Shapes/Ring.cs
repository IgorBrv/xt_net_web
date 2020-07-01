using System;

namespace Custom_Paint
{
	class Ring : Circle
	{   // Кольцо
		protected int dOuter;	// Внутренний диаметр кольца

		public Ring(int x, int y, int d, int dOuter, Colors color) : base(x, y, d, color)
		{
			if (dOuter > d)
			{
				throw new Exception("Внутренний диаметр кольца не может быть больше внешнего!");
			}

			this.dOuter = dOuter;
			about[0] = "Кольцо".PadRight(14);
			about.Add($" Внутренний диаметр: {dOuter};");
		}
		public override double GetArea()
		{	// Метод возвращающий площадь фигуры
			return Math.Round(Math.PI * (Math.Pow(this.dOuter, 2) - Math.Pow(this.dOuter, 2)), 2);
		}
	}
}
