using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace FileManagementSystem
{
	class BackupHandler
	{   // Модуль занимающийся нахождением и протоколированием изменений между двумя файлами. Используемые внутри скрипты жадные, не очень эффективные
        // , и могут найти разницу только в виде одного куска. Модуль используется для примера возможностей.

        public static void Backup(byte[] file1, byte[] file2, string filePath, string backupPath, string workDir)
        {
            CMapObject differences;

            if (file1.Length >= file2.Length)
            {
                differences = GetDifferencesCaseA(file1, file2, backupPath, filePath, workDir);
            }
            else
            {
                differences = GetDifferencesCaseB(file1, file2, backupPath, filePath, workDir);
            }

            File.WriteAllText($@"{backupPath}\cmap", JsonConvert.SerializeObject(differences));
        }

        private static CMapObject GetDifferencesCaseA(byte[] file1, byte[] file2, string backupPath, string filePath, string workDir)
        {   // Если Файл 1 > Файл 2. Метод используется для нахождения и протоколирования разницы между файлами. Метод не очень эффективен, и представлен как заглушка.
            // Далее происходит некое волшебство.

            int posB = 0;
            int targetLeftBorder = -1;
            int target1RightBorder = -1;
            int target2RightBorder = -1;

            if (file2.Length == 0)
            {
                return new CMapObject(filePath, $"remove", new int[] { 0, file1.Length - 1 });
            }

            for (int i = 0; i <= file2.Length; i++)
            {

                int posA;
                if (i == file2.Length)
                {
                    posA = i;
                    posB = file1.Length - 1;
                    return new CMapObject(filePath, "remove", new int[] { posA, posB }, null);
                }

                if (file2[i] == file1[i])
                {
                    targetLeftBorder = i;
                }
                else
                {
                    posA = i;
                    int offset = 1;

                    for (int x = file2.Length - 1; x >= posA; x--)
                    {
                        if (x > targetLeftBorder && file1[file1.Length - offset] == file2[x])
                        {
                            target1RightBorder = file1.Length - offset;
                            target2RightBorder = x;
                        }
                        else
                        {
                            posB = file1.Length - offset;
                            break;
                        }
                        offset++;
                    }

                    int changesCount = target1RightBorder - targetLeftBorder - 1;

                    if (file1.Length - changesCount == file2.Length)
                    {
                        posB = posA + changesCount - 1;
                        return new CMapObject(filePath, "remove", new int[] { posA, posB }, null);
                    }

                    List<byte> buffer = new List<byte>();

                    for (int z = targetLeftBorder + 1; z < target2RightBorder; z++)
                    {
                        buffer.Add((byte)file2[z]);
                    }

                    File.WriteAllBytes($@"{backupPath}\raw", buffer.ToArray());

                    return new CMapObject(filePath, "replace", new int[] { posA, posB }, $@"{backupPath}\raw".Replace($"{workDir}\\", ""));
                }
            }
            return default;
        }

        private static CMapObject GetDifferencesCaseB(byte[] file1, byte[] file2, string backupPath, string filePath, string workDir)
        {   // Если Файл 1 < Файл 2. Метод используется для нахождения и протоколирования разницы между файлами. Метод не очень эффективен, и представлен как заглушка.
            // Далее происходит некое волшебство.

            int targetLeftBorder = -1;
            int targetRightBorder = -1;
            List<byte> buffer = new List<byte>();
            string prefix;

            if (file1.Length == 0)
            {
                File.WriteAllBytes($@"{backupPath}\raw", file2);
                return new CMapObject(filePath, $"insert", new int[] { 0, 0 }, $@"{backupPath}\raw".Replace($"{workDir}\\", ""));
            }

            for (int i = 0; i < file2.Length; i++)
            {
                int posA;
                int posB;

                if (i == file1.Length)
                {
                    posA = i;
                    posB = i;
                    buffer = file2.Skip(i).ToList();
                    File.WriteAllBytes($@"{backupPath}\raw", buffer.ToArray());
                    return new CMapObject(filePath, "insert", new int[] { posA, posB }, $@"{backupPath}\raw".Replace($"{workDir}\\", ""));

                }

                if (file2[i] == file1[i])
                {
                    targetLeftBorder = i;
                    targetRightBorder = i;
                }

                else
                {
                    posA = i;
                    int offset = 1;

                    for (int x = file1.Length - 1; x >= posA; x--)
                    {
                        if (x > targetLeftBorder && file2[file2.Length - offset] == file1[x])
                        {
                            targetRightBorder = file2.Length - offset;
                        }
                        else
                        {
                            break;
                        }
                        offset++;
                    }

                    posB = posA;
                    prefix = "insert";

                    for (int z = targetLeftBorder + 1; z < targetRightBorder; z++)
                    {
                        buffer.Add((byte)file2[z]);
                    }

                    if (buffer.Count + file1.Length > file2.Length)
                    {
                        prefix = "replace";
                        posB = posA + (buffer.Count + file1.Length - file2.Length) - 1;
                    }

                    File.WriteAllBytes($@"{backupPath}\raw", buffer.ToArray());

                    return new CMapObject(filePath, $"{prefix}", new int[] { posA, posB }, $@"{backupPath}\raw".Replace($"{workDir}\\", ""));
                }
            }
            return default;
        }
    }
}
