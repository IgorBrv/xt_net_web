using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace FileManagementSystem
{
	public static class RestoreHandler
	{	// Метод восстанавливающий состояние файла из бекапа сформированного BackupHandler

		public static byte[] Restore(byte[] file1, string workDirectory, CMapObject differences)
		{
			string type = differences.action;
			int[] adress = differences.changesAdress;
			byte[] result = default;
			byte[] buffer = default;

			if (differences.rawAdress != null)
			{
				buffer = File.ReadAllBytes($"{workDirectory}\\{differences.rawAdress}");
			}

			switch (type)
			{
				case ("replace"):
					result = Replace(file1, adress, buffer);
					break;
				case ("remove"):
					result = Remove(file1, adress);
					break;
				case ("insert"):
					result = Insert(file1, adress, buffer);
					break;
			}

			return result;
		}

		private static byte[] Replace(byte[] file, int[] adress, byte[] buffer)
		{
			List<byte> temp = file.ToList();

			temp.RemoveRange(adress[0], adress[1] - adress[0] + 1);

			temp.InsertRange(adress[0], buffer);

			return temp.ToArray();
		}

		private static byte[] Remove(byte[] file, int[] adress)
		{
			List<byte> temp = file.ToList();

			temp.RemoveRange(adress[0], adress[1] - adress[0] + 1);

			return temp.ToArray();
		}

		private static byte[] Insert(byte[] file, int[] adress, byte[] buffer)
		{
			List<byte> temp = file.ToList();

			temp.InsertRange(adress[0], buffer);

			return temp.ToArray();
		}
	}
}
