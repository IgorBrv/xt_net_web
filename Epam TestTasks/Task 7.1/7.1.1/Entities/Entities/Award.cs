using System;

namespace Entities
{	// Общие классы

	public class Award
	{	// Класс объекта награды

		public Guid id;
		public string title;

		public Award(Guid id, string title)
		{
			this.id = id;
			this.title = title;
		}
	}
}
