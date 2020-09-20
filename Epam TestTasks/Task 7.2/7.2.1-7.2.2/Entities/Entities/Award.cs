using System;

namespace Entities
{	// Общие классы

	public class Award : AbstractEntityWithID
	{   // Класс объекта награды

		
		public string title;
		public string emblempath;

		public Award(Guid id, string title, string emblempath = null)
		{
			this.id = id;
			this.title = title;
			this.emblempath = emblempath;
		}
	}
}
