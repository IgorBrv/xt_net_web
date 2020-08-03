using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FileManagementSystem
{
	static class Restore
	{   // Класс содержащий в себе методы восстановления отдельного файла или целого состояния из сохранённых копий.


		public static FileObject RestoreFile(string workDirectory, string fileName, string date)
		{	// Метод восстанавливающий состояние необходимого файла до последнего сохранённого слепка изменений для последующего сравнения файла с изменённой версией.

			// Получаем полный список бэкапов:
			List<string> listOfBackups = Directory.GetDirectories($@"{workDirectory}\_backup").OrderByDescending(item => item).ToList();

			if (listOfBackups.Count <= 1)
			{	// Простая проверка на наличие бекапов в списке
				return null;
			}

			// Из списка бэкапов получаем последний полный бэкап содержащий необходимый файл, и список бэкапов произведённых после него:

			try
			{   // Исключим недоступность карт изменений из-за параллельного создания нескольких резервных копий:

				listOfBackups = FindLastFullBackup(ref fileName, date, listOfBackups);
			}
			catch (FileNotFoundException)
			{
				return null;
			}

			string fullBackup = listOfBackups[0];
			listOfBackups.RemoveAt(0);

			
			// Удостоверимся, что полный бэкап не является ссылкой на другой полный бэкап:
			if (Directory.GetFiles(fullBackup).Contains($"{fullBackup}\\flink"))
			{
				while (!Directory.GetFiles(fullBackup).Contains($"{fullBackup}\\map"))
				{
					fullBackup = $"{workDirectory}\\{File.ReadAllText($"{fullBackup}\\flink")}";
				}
			}

			// Прочитаем карту последнего полного бэкапа с файлом, и считаем сохранённую копию файла:

			FileObject file = default;

			try
			{	// Исключим недоступность карты изменений из-за параллельного создания нескольких резервных копий:

				Dictionary<string, string> map = JsonConvert.DeserializeObject<Dictionary<string, string>>(GetMapOrChanges($@"{fullBackup}\map"));
				file = new FileObject(fileName, File.ReadAllBytes($"{workDirectory}\\{map[fileName]}"));
			}
			catch (FileNotFoundException)
			{
				return null;
			}

			// Применим к считанному файлу все изменения произведённые после полного бэкапа, и вернём файл:
			try
			{
				return ApplyChangesToFile(listOfBackups, workDirectory, file);
			}
			catch (FileNotFoundException)
			{
				return null;
			}
		}


		public static bool RestoreState(string workDirectory, string statePath)
		{   // Метод полностью восстанавливающий конкретное состояние наблюдаемой папки

			// Получаем полный список бэкапов:
			List<string> ListOfBackups = default;
			try
			{
				ListOfBackups = Directory.GetDirectories($@"{workDirectory}\_backup").OrderByDescending(item => item).ToList();
			}
			catch (UnauthorizedAccessException)
			{
				return false;
			}

			List<string> neededBackups = new List<string>();
			string lastFullBackup = default;
			bool inRange = false;

			// Производим поиск всех бэкапов ОТ выбранного состояния ДО предшествующего ПОЛНОГО бэкапа:
			foreach (string backup in ListOfBackups)
			{
				if (backup.Contains(statePath))
				{
					if (backup.EndsWith("[F]"))
					{
						lastFullBackup = backup;
						break;
					}

					neededBackups.Add(backup);
					inRange = true;
				}
				else if (inRange)
				{
					if (backup.EndsWith("[F]"))
					{
						lastFullBackup = backup;
						break;
					}

					neededBackups.Insert(0, backup);
				}
			}

			// Считываем карту состояния найденного ПОЛНОГО бэкапа:

			Dictionary<string, string> map = default;
			try
			{	// Удостоверимся, что полный бэкап не является ссылкой на другой полный бэкап:
				if (Directory.GetFiles(lastFullBackup).Contains($"{lastFullBackup}\\flink"))
				{
					while (!Directory.GetFiles(lastFullBackup).Contains($"{lastFullBackup}\\map"))
					{
						lastFullBackup = $"{workDirectory}\\{File.ReadAllText($"{lastFullBackup}\\flink")}";
					}
				}

				map = JsonConvert.DeserializeObject<Dictionary<string, string>>(GetMapOrChanges($@"{lastFullBackup}\map"));
			}
			catch (FileNotFoundException)
			{
				return false;
			}

			// Сформируем список объектов файлов, содержащихся в найденном полном бэкапе:
			List<FileObject> files = map.Select(KeyValuePair => new FileObject(KeyValuePair.Key, File.ReadAllBytes($"{workDirectory}\\{KeyValuePair.Value}"))).ToList();

			// Пройдёмся по списку бэкапов, применим каждый бекап к списку файлов:
			try
			{
				ApplyChangesToFilesList(neededBackups, files, workDirectory);
			}

			catch (FileNotFoundException)
			{
				return false;
			}

			// Сформируем список файлов присутствующих в папке на момент начала восстановления:
			List<string> filesInDirectory = Directory.GetFiles(workDirectory, "*.txt", SearchOption.AllDirectories).Select(item => item.Replace($"{workDirectory}\\", "")).ToList();

			// Сохраним полученные файлы на диск:
			foreach (FileObject file in files.Where(item => item != null))
			{
				string directory = $@"{workDirectory}\{string.Join("", file.path.Take(file.path.RFind('\\')))}";

				if (!Directory.Exists(directory))
				{
					Directory.CreateDirectory(directory);
				}

				// Удалим создаваемый файл из списка файлов которые были в папке до начала восстановления
				if (filesInDirectory.Contains(file.path))
				{
					filesInDirectory.Remove(file.path);
				}

				File.WriteAllBytes($@"{workDirectory}\{file.path}", file.body);
			}

			// Удалим все файлы, которые остались в списке файлов, которые были в папке до начала восстаановления, т.к. эти файлы не были отражены в бекапе:
			foreach (string file in filesInDirectory)
			{
				try
				{
					File.Delete($"{workDirectory}\\{file}");
				}
				catch
				{
					// Nothing (Поздно что-то менять)
				}
			}

			// Так же удалим все пустые дирректории:
			foreach (string folder in Directory.GetDirectories(workDirectory, "*", SearchOption.AllDirectories).Where(item => !item.Contains($"{workDirectory}\\_backup")).OrderByDescending(item => item))
			{
				try
				{
					if (Directory.GetFiles(folder).Length == 0)
					{
						Directory.Delete(folder);
					}
				}
				catch
				{
					// Nothing (Поздно что-то менять)
				}
			}

			return true;
		}


		public static void ApplyChangesToFilesList(List<string> neededBackups, List<FileObject> files, string workDirectory, bool onlyNames = false)
		{   // Метод проходящийся по списку обновлений и применяющий их к относящимся файлам

			foreach (string backup in neededBackups)
			{
				if (Directory.GetFiles(backup).Contains($@"{backup}\changes"))
				{
					ChangesObject changes = JsonConvert.DeserializeObject<ChangesObject>(GetMapOrChanges($@"{backup}\changes"));

					for (int i = 0; i < files.Count; i++)
					{
						if (files[i] != null && files[i].path == changes.path)
						{
							if (changes.action == "remove")
							{
								files[i] = null;
							}
							else
							{
								files[i].path = changes.newPath;
							}
						}
					}
				}
				else if (Directory.GetFiles(backup).Contains($@"{backup}\multipleChanges"))
				{
					List<ChangesObject> multipleChanges = JsonConvert.DeserializeObject<List<ChangesObject>>(GetMapOrChanges($"{backup}\\multipleChanges"));

					foreach (ChangesObject changes in multipleChanges)
					{
						for (int i = 0; i < files.Count; i++)
						{
							if (files[i] != null && files[i].path == changes.path)
							{
								if (changes.action == "remove")
								{
									files[i] = null;
								}
								else
								{
									files[i].path = changes.newPath;
								}
							}
						}
					}
				}
				else if (Directory.GetFiles(backup).Contains($@"{backup}\cmap"))
				{
					if (!onlyNames)
					{
						CMapObject changesMap = JsonConvert.DeserializeObject<CMapObject>(GetMapOrChanges($@"{backup}\cmap"));

						for (int i = 0; i < files.Count; i++)
						{
							if (files[i] != null && changesMap.path == files[i].path)
							{
								files[i].body = RestoreHandler.Restore(files[i].body, workDirectory, changesMap);
							}
						}
					}
				}
				else if (Directory.GetFiles(backup).Contains($@"{backup}\map"))
				{
					Dictionary<string, string> map = JsonConvert.DeserializeObject<Dictionary<string, string>>(GetMapOrChanges($@"{backup}\map"));

					if (!onlyNames)
					{
						foreach (string name in map.Keys)
						{
							files.Add(new FileObject(name, File.ReadAllBytes($"{workDirectory}\\{map[name]}")));
						}
					}
					else
					{
						foreach (string name in map.Keys)
						{
							files.Add(new FileObject(name, null));
						}
					}
				}
			}
		}


		private static List<string> FindLastFullBackup(ref string fileName, string curDate, List<string> ListOfBackups)
		{   // Метод осуществляющий поиск ПОСЛЕДНЕГО полного бэкапа для файла

			List<string> Temp = new List<string>();

			foreach (string backupPath in ListOfBackups)
			{
				string mark = string.Join("", backupPath.Skip(backupPath.RFind('[')));

				if (backupPath.Contains(curDate))
				{
					continue;
				}
				else if (mark == "[R]")
				{
					ChangesObject changes = JsonConvert.DeserializeObject<ChangesObject>(GetMapOrChanges($@"{backupPath}\changes"));
					if (changes.newPath == fileName)
					{
						fileName = changes.path;
					}
				}
				else if (mark == "[M]")
				{
					List<ChangesObject> multipleChanges = JsonConvert.DeserializeObject<List<ChangesObject>>(GetMapOrChanges($"{backupPath}\\multipleChanges"));

					foreach (ChangesObject changes in multipleChanges)
					{
						if (changes.newPath == fileName)
						{
							fileName = changes.path;
						}
					}
				}
				else if (mark == "[N]" || mark == "[I]")
				{
					Dictionary<string, string> newFileMap = JsonConvert.DeserializeObject<Dictionary<string, string>>(GetMapOrChanges($@"{backupPath}\map"));

					if (newFileMap.ContainsKey(fileName))
					{
						Temp.Insert(0, backupPath);
						return Temp;
					}
				}
				else if (mark == "[F]")
				{
					Temp.Insert(0, backupPath);
					return Temp;
				}

				Temp.Insert(0, backupPath);
			}
			return default;
		}


		private static FileObject ApplyChangesToFile(List<string> ListOfBackups, string workDirectory, FileObject file)
		{	// Метод применяющий к ввосстановленному из полного бэкапа файлу изменения из всех последующих найденных бекапов помещённых в список.

			foreach (string backup in ListOfBackups)
			{
				if (Directory.GetFiles(backup).Contains($@"{backup}\changes"))
				{
					ChangesObject changes = JsonConvert.DeserializeObject<ChangesObject>(GetMapOrChanges($@"{backup}\changes"));

					if (changes.path == file.path)
					{
						if (changes.action == "remove")
						{
							return null;
						}
						else
						{
							file.path = changes.newPath;
						}
					}
				}
				else if (Directory.GetFiles(backup).Contains($@"{backup}\multipleChanges"))
				{
					List<ChangesObject> multipleChanges = JsonConvert.DeserializeObject<List<ChangesObject>>(GetMapOrChanges($"{backup}\\multipleChanges"));

					foreach (ChangesObject changes in multipleChanges)
					{
						if (changes.path == file.path)
						{
							if (changes.action == "remove")
							{
								return null;
							}
							else
							{
								file.path = changes.newPath;
							}
						}
					}
				}
				else if (Directory.GetFiles(backup).Contains($@"{backup}\cmap"))
				{
					CMapObject changesMap = JsonConvert.DeserializeObject<CMapObject>(GetMapOrChanges($@"{backup}\cmap"));

					if (changesMap.path == file.path)
					{
						file.body = RestoreHandler.Restore(file.body, workDirectory, changesMap);
					}
				}
			}
			return file;
		}

		private static string GetMapOrChanges(string path)
		{   // Вспомогательный метод реализующий ожидание создания карты изменений в случае её недоступности (может произойти при внесении изменений в несколько файлов параллельно)
			int count = 0;
			while (!File.Exists(path))
			{
				if (count >= 50)
				{
					throw new FileNotFoundException("Map file is not founded");
				}
				Task.Delay(100).Wait();
				count++;
			}
			return File.ReadAllText(path);
		}
	}
}
