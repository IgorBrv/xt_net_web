using System;
using Entities;
using System.IO;

namespace InterfaceDAL
{
	public interface IEmblemsDAO
	{
		string CreateEmblem(AbstractEntityWithID item, string ext, BinaryReader br);
		bool ElementHasEmblem(AbstractEntityWithID item);
		string GetEmblemPath(AbstractEntityWithID item);
		bool RemoveEmblem(AbstractEntityWithID item);
	}
}
