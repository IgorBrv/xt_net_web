using System;
using Entities;
using System.IO;

namespace InterfaceDAL
{
	public interface IEmblemsDAO
	{
		string CreateEmblem(IHaveID item, string ext, BinaryReader br);
		bool ElementHasEmblem(IHaveID item);
		string GetEmblemPath(IHaveID item);
		bool RemoveEmblem(IHaveID item);
	}
}
