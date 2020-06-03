using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
