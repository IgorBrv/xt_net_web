using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagementSystem
{
	class BackupProcess
	{
		private readonly string workDirectory;
		private readonly string backupPath;
		private readonly string workDirectoryName;
		public BackupProcess(string path)
		{
			workDirectory = path;
			backupPath = path + @"\_backup";
			workDirectoryName = string.Join("", path.Skip(path.RFind('\\')+1)) + ".";
			if (!Directory.Exists(backupPath))
			{
				Directory.CreateDirectory(backupPath);
			}
		}

		public void FullBackup()
		{
			string[] files = Directory.GetFiles(workDirectory, "*.txt", SearchOption.AllDirectories);
			string pathForCurrentBackup = $@"{backupPath}\{DateTime.Now:[dd.MM.yyyy.HH.mm.ss]}";
			Directory.CreateDirectory(pathForCurrentBackup);
			Dictionary<string, string> Map = new Dictionary<string, string>();

			int count = 0;

			foreach (string file in files)
			{
				string newname;
				string prefix = file.RTakePart(file.RFind('\\'), workDirectory.Length);

				if (prefix == string.Empty)
				{
					newname = $@"{pathForCurrentBackup}\{string.Join("", file.Skip(file.RFind('\\') + 1)).Replace(".txt", ".bak")}";
				}
				else
				{
					newname = $@"{pathForCurrentBackup}\{prefix.Replace('\\', '.')}.{string.Join("", file.Skip(file.RFind('\\') + 1)).Replace(".txt", ".bak")}";
				}

				Console.WriteLine("!!!" + file + "; " + newname);
				File.Copy(file, newname);
				Map.Add(newname, file);
				count++;
			}
		}

		public void SaveChanges()
		{

		}
	}
}
