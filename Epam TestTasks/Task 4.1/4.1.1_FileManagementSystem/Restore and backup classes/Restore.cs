using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileManagementSystem
{
	static class Restore
	{   // Класс содержащий в себе методы восстановления отдельного файла или целого состояния из сохранённых копий.


		public static FileObject RestoreFile(string workDirectory, string fileName, string date)
		{	// Метод восстанавливающий состояние необходимого файла до последнего сохранённого слепка изменений для последующего сравнения файла с изменённой версией.

			// Получаем полный список бэкапов:
			List<string> listOfBackups = Directory.GetDirectories($@"{workDirectory}\_backup").OrderByDescending(item => item).ToList();

			// Из списка бэкапов получаем последний полный бэкап содержащий необходимый файл, и список бэкапов произведённых после него:
			listOfBackups = FindLastFullBackup(ref fileName, date, listOfBackups);
			string FullBackup = listOfBackups[0];
			listOfBackups.RemoveAt(0);

			// Прочитаем карту последнего полного бэкапа с файлом, и считаем сохранённую копию файла:
			Dictionary<string, string> map = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText($@"{FullBackup}\map"));
			FileObject file = new FileObject(fileName, File.ReadAllBytes($"{workDirectory}\\{map[fileName]}"));

			// Применим к считанному файлу все изменения произведённые после полного бэкапа, и вернём файл:
			return ApplyChangesToFile(listOfBackups, workDirectory, file);
		}


		public static void RestoreState(string workDirectory, string statePath)
		{   // Метод полностью восстанавливающий конкретное состояние наблюдаемой папки

			// Получаем полный список бэкапов:
			List<string> ListOfBackups = Directory.GetDirectories($@"{workDirectory}\_backup").OrderByDescending(item => item).ToList();

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
			Dictionary<string, string> map = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText($@"{lastFullBackup}\map"));

			// Сформируем список объектов файлов, содержащихся в найденном полном бэкапе:
			List<FileObject> files = map.Select(KeyValuePair => new FileObject(KeyValuePair.Key, File.ReadAllBytes($"{workDirectory}\\{KeyValuePair.Value}"))).ToList();

			// Пройдёмся по списку бэкапов, применим каждый бекап к списку файлов:
			ApplyChangesToFilesList(neededBackups, files, workDirectory);

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

			// Удалим все файлы, которые осталисьь в списке файлов, которые были в папке до начала восстаановления, т.к. эти файлы не были отражены в бекапе:
			foreach (string file in filesInDirectory)
			{
				File.Delete($"{workDirectory}\\{file}");
			}

			// Так же удалим все пустые дирректории:
			foreach (string folder in Directory.GetDirectories(workDirectory, "*", SearchOption.AllDirectories).Where(item => !item.Contains($"{workDirectory}\\_backup")).OrderByDescending(item => item))
			{
				if (Directory.GetFiles(folder).Length == 0)
				{
					Directory.Delete(folder);
				}
			}
		}


		public static void ApplyChangesToFilesList(List<string> neededBackups, List<FileObject> files, string workDirectory, bool onlyNames = false)
		{   // Метод проходящийся по списку обновлений и применяющий их к относящимся файлам

			foreach (string backup in neededBackups)
			{
				if (Directory.GetFiles(backup).Contains($@"{backup}\changes"))
				{
					ChangesObject changes = JsonConvert.DeserializeObject<ChangesObject>(File.ReadAllText($@"{backup}\changes"));

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
					List<ChangesObject> multipleChanges = JsonConvert.DeserializeObject<List<ChangesObject>>(File.ReadAllText($"{backup}\\multipleChanges"));

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
						CMapObject changesMap = JsonConvert.DeserializeObject<CMapObject>(File.ReadAllText($@"{backup}\cmap"));

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
					Dictionary<string, string> map = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText($@"{backup}\map"));

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
					ChangesObject changes = JsonConvert.DeserializeObject<ChangesObject>(File.ReadAllText($@"{backupPath}\changes"));
					if (changes.newPath == fileName)
					{
						fileName = changes.path;
					}
				}
				else if (mark == "[M]")
				{
					List<ChangesObject> multipleChanges = JsonConvert.DeserializeObject<List<ChangesObject>>(File.ReadAllText($"{backupPath}\\multipleChanges"));

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
					Dictionary<string, string> newFileMap = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText($@"{backupPath}\map"));

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
					ChangesObject changes = JsonConvert.DeserializeObject<ChangesObject>(File.ReadAllText($@"{backup}\changes"));

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
					List<ChangesObject> multipleChanges = JsonConvert.DeserializeObject<List<ChangesObject>>(File.ReadAllText($"{backup}\\multipleChanges"));

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
					CMapObject changesMap = JsonConvert.DeserializeObject<CMapObject>(File.ReadAllText($@"{backup}\cmap"));

					if (changesMap.path == file.path)
					{
						file.body = RestoreHandler.Restore(file.body, workDirectory, changesMap);
					}
				}
			}
			return file;
		}
	}
}
