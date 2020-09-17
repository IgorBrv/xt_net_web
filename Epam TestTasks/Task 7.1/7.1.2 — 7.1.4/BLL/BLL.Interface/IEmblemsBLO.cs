using System;
using Entities;
using System.IO;

namespace InterfacesBLL
{
	public interface IEmblemsBLO
	{
		string CreateEmblem(IHaveID item, string ext, BinaryReader br);
		bool ElementHasEmblem(IHaveID item);
		string GetEmblemPath(IHaveID item);
		bool RemoveEmblem(IHaveID item);
	}
}
