using InterfaceDAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfacesBLL;
using Entities;

namespace CoreBLL
{
	public class EmblemsBLL : IEmblemsBLO
	{
		private readonly IEmblemsDAO emblemsDao;
		public EmblemsBLL(IEmblemsDAO emblemsDao)
		{
			this.emblemsDao = emblemsDao;
		}

		public string CreateEmblem(IHaveID item, string ext, BinaryReader br)
		{
			return emblemsDao.CreateEmblem(item, ext, br);
		}

		public bool ElementHasEmblem(IHaveID item)
		{
			return emblemsDao.ElementHasEmblem(item);
		}

		public string GetEmblemPath(IHaveID item)
		{
			return emblemsDao.GetEmblemPath(item);
		}

		public bool RemoveEmblem(IHaveID item)
		{
			return emblemsDao.RemoveEmblem(item);
		}
	}
}
