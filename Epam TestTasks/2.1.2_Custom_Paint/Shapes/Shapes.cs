using System.Collections.Generic;


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


	abstract class Shapes
	{   // Родительский класс всех фигур
		protected int x;  // x, y - координаты
		protected int y;
		protected List<string> about;	// Строка содержащая информацию об объекте

		public Colors Color { get; private set; }

		public Shapes(int x, int y, Colors color)
		{
			this.x = x;
			this.y = y;
			Color = color;
			string coordinates = $"({x}, {y})".PadLeft(12);
			about = new List<string> { "", $"| Координаты: {coordinates};" };
		}

		public string About
		{
			get
			{
				return string.Join("", about);
			}
		}
	}









}
