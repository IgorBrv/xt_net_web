using System;

namespace Entities
{	// Общие классы

	public class Award : IHaveID
	{   // Класс объекта награды

		
		public string title;

		public Award(Guid id, string title)
		{
			this.id = id;
			this.title = title;
		}
		public Guid id { get; set; }
	}
}
