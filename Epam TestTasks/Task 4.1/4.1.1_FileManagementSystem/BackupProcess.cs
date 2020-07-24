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
		string workDirectory;
		string backupPath;
		public BackupProcess(string path)
		{
			workDirectory = path;
			backupPath = path + @"\_backup";
			if (!Directory.Exists(backupPath))
			{
				Directory.CreateDirectory(backupPath);
			}
			FullBackup();
		}

		private void FullBackup()
		{
			string[] files = Directory.GetFiles(workDirectory, "*.txt", SearchOption.AllDirectories);
			string pathForCurrentBackup = $@"{backupPath}\{DateTime.Now:[dd.MM.yyyy.HH.mm.ss]}";
			Directory.CreateDirectory(pathForCurrentBackup);
			Dictionary<string, string> Map = new Dictionary<string, string>();

			int count = 0;

			foreach (string file in files)
			{
				string newname = $@"{pathForCurrentBackup}\[{count}]{string.Join("", file.Skip(file.RFind('\\') + 1)).Replace(".txt", ".bak")}";
				File.Copy(file, newname);
				Map.Add(newname, file);
				count++;
			}
		}
	}
}
