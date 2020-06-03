using System.Collections.Generic;


namespace Custom_Paint
{
	class User
	{   // Класс объекта пользователя

		public List<Shapes> Shapes { get; set; }

		public string Name { get; }

		public User(string name)
		{	// Конструктор класса
			Name = name;
			Shapes = new List<Shapes>();
		}

	}
}
