using System;

namespace Custom_Paint
{
	class Circle : Shapes
	{   // Круг (Координаты круга описывают левый верхний угол, от которого растягивается круг вписаный в условный квадрат)

		protected int d;	// Диаметр круга

		public Circle(int x, int y, int d, Colors color) : base(x, y, color)
		{
			this.d = d;
			about[0] = "Круг".PadRight(14);
			about.Add($" Диаметр: {d};");
		}

		public double GetArea()
		{   // Метод возвращающий площадь фигуры
			return Math.Round(Math.PI * Math.Pow(this.d, 2), 2);
		}
	}
}
