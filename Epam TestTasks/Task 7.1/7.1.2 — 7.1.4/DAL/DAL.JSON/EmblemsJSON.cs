using InterfaceDAL;
using System;
using Entities;
using System.IO;

namespace JsonDAL
{
	public class EmblemsJSON : IEmblemsDAO
	{
		private readonly string path;
		private readonly DALJson dalJson;

		public EmblemsJSON(string path)
		{
			this.path = path;
			this.dalJson = JsonDAL.Get(path);
		}

		public string CreateEmblem(IHaveID item, string ext, BinaryReader br)
		{
			Guid emblemId = Guid.NewGuid();
			string savePath = $"{path}\\images\\avatars\\";

			if (!Directory.Exists(savePath))
			{
				Directory.CreateDirectory(savePath);
			}

			try
			{
				using (br)
				{
					int lenght = (int)br.BaseStream.Length;
					byte[] file = br.ReadBytes(lenght);
					File.WriteAllBytes($"{savePath}{emblemId}.{ext}", file);
				}

				Data data = dalJson.LoadAll();

				if (data.emblemsList.ContainsKey(item.id)) {

					File.Delete($"{path}{dalJson.emblemsList[item.id].Path.Substring(1)}");
					data.emblemsList[item.id].Path = $"./images/avatars/{emblemId}.{ext}";
				}
				else
				{
					data.emblemsList.Add(item.id, new Emblem($"./images/avatars/{emblemId}.{ext}"));
				}

				if (dalJson.SaveAll(data))
				{
					dalJson.emblemsList = data.emblemsList;
					return $"./images/avatars/{emblemId}.{ext}";
				}

				return null;
			}
			catch (IOException)
			{
				return null;
			}
		}

		public bool RemoveEmblem(IHaveID item)
		{
			if (dalJson.emblemsList.ContainsKey(item.id))
			{
				try
				{
					File.Delete($"{path}{dalJson.emblemsList[item.id].Path.Substring(1)}");
				}
				catch (IOException)
				{
					//TODO
				}

				Data data = dalJson.LoadAll();
				data.emblemsList.Remove(item.id);

				if (dalJson.SaveAll(data))
				{
					dalJson.emblemsList.Remove(item.id);
					return true;
				}
			}
			return false;
		}

		public string GetEmblemPath(IHaveID item)
		{
			if (dalJson.emblemsList.ContainsKey(item.id))
			{
				return dalJson.emblemsList[item.id].Path;
			}
			return null;
		}

		public bool ElementHasEmblem(IHaveID item)
		{
			return dalJson.emblemsList.ContainsKey(item.id);
		}
	}
}
