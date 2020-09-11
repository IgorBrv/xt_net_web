using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{

	public class Award
	{
		public Guid id;
		public string title;

		public Award(Guid id, string title)
		{
			this.id = id;
			this.title = title;
		}
	}
}
