using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
	public abstract class AbstractEntityWithID
	{
		public Guid id { get; set; }
	}
}
