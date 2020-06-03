
namespace Custom_Paint
{
	class Rectangle : Square
	{   // Прямоугольник
		protected int sideB;

		public Rectangle(int x, int y, int sideA, int sideB, Colors color) : base(x, y, sideA, color)
		{
			this.sideB = sideB;
			about[0] = "Прямоугольник".PadRight(14);
			about.Add($" Вторая сторона: {sideB};");
		}

		public new double GetArea()
		{   // Метод возвращающий площадь фигуры
			return sideA * sideB;
		}
	}
}
