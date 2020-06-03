using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Custom_Paint
{
	enum Colors
	{	// Цвета фигур
		Default,
		White,
		Red,
		Orange,
		Yellow,
		Green,
		Blue,
		Violet,
		Black
	}


	class Shapes
	{	// Родительский класс всех фигур
		protected List<string> about;
		public string About
		{
			get
			{
				return string.Join("", about);
			}

		}
	}

	class RoundShape : Shapes
	{   // Круглая форма
		protected int x;  // x, y - координаты центра
		protected int y;
		protected int radius;

		public RoundShape(int x, int y, int radius)
		{
			this.x = x;
			this.y = y;
			this.radius = radius;
			about = new List<string> { "Круглая форма".PadRight(14), $"| Координаты: ({x}, {y}); Радиус: {radius};" };
		}

	}

	class Circle : RoundShape
	{   // Круг
		protected Colors color;

		public Circle(int x, int y, int radius, Colors color) : base(x, y, radius)
		{
			this.color = color;
			about[0] = "Круг".PadRight(14);
			about.Add($" Цвет: {color}");
		}

		public double GetArea()
		{	// Метод возвращающий площадь фигуры
			return Math.Round(Math.PI * Math.Pow(this.radius, 2), 2);
		}
	}

	class Ring : RoundShape
	{   // Кольцо
		protected Colors color;
		protected int innerRadius;

		public Ring(int x, int y, int radius, int innerRadius, Colors color) : base(x, y, radius)
		{
			if (innerRadius > radius)
			{
				throw new Exception("Внутренний радиус кольца не может быть больше внешнего!");
			}

			this.color = color;
			this.innerRadius = innerRadius;
			about[0] = "Кольцо".PadRight(14);
			about.Add($" Внутренний радиус: {innerRadius}; Цвет: {color}");
		}

		public double GetArea()
		{
			return Math.Round(Math.PI * (Math.Pow(this.radius, 2) - Math.Pow(this.innerRadius, 2)), 2);
		}
	}

	class SquareShape : Shapes
	{   // Квадратная форма
		protected int x;  // х, у - координаты расположения левого верхнего угла
		protected int y;
		protected int side_a;

		public SquareShape(int x, int y, int side_a)
		{
			this.x = x;
			this.y = y;
			this.side_a = side_a;
			about = new List<string> { "Квадратная форма".PadRight(14), $"| Координаты: ({x}, {y}); Длина стороны: {side_a};" };
		}
	}

	class RectShape : SquareShape
	{   // Прямоугольная форма
		protected int side_b;

		public RectShape(int x, int y, int side_a, int side_b) : base(x, y, side_a)
		{
			this.side_b = side_b;
			about[0] = "Прямоугольная форма".PadRight(14);
			about.Add($" Длина второй стороны: {side_b};");
		}
	}

	class Rectangle : RectShape
	{   // Прямоугольник
		protected Colors color;

		public Rectangle(int x, int y, int side_a, int side_b, Colors color) : base(x, y, side_a, side_b)
		{
			this.color = color;
			about[0] = "Прямоугольник".PadRight(14);
			about.Add($" Цвет: {color}");
		}

		public double GetArea()
		{   // Метод возвращающий площадь фигуры
			return side_a * side_b;
		}
	}

	class Parallelogram : RectShape
	{   // Паралеллограмм
		protected int angle;
		protected Colors color;

		public Parallelogram(int x, int y, int side_a, int side_b, int angle, Colors color) : base(x, y, side_a, side_b)
		{
			this.angle = angle;
			this.color = color;
			about[0] = "Паралеллограмм".PadRight(14);
			about.Add($" Угол: {angle}; Цвет: {color}");
		}

		public double GetArea()
		{   // Метод возвращающий площадь фигуры
			return Math.Round(side_a * side_b * Math.Sin(angle), 2);
		}
	}

	class Square : SquareShape
	{   // Квадрат
		protected Colors color;

		public Square(int x, int y, int side_a, Colors color) : base(x, y, side_a)
		{
			this.color = color;
			about[0] = "Квадрат".PadRight(14);
			about.Add($" Цвет: {color}");
		}

		public double GetArea()
		{   // Метод возвращающий площадь фигуры
			return Math.Round(Math.Pow(side_a, 2), 2);
		}
	}

}
