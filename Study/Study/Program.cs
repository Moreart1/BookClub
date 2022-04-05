using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Study
{
    class Program
    {
        /// <summary>
        /// Метод возвращает имя диска, на который хочет перейти пользователь.
        /// </summary>
        /// <param name="driveNames">Массив дисков, которые доступны на устройстве.</param>
        /// <returns>Возвращает строку, имя диска, на который переходит пользователь.</returns>
        private static string ReadDriveName(string[] driveNames)
        {
            try
            {
                string driverName;
                do
                {
                    Console.Write("Введите название диска, на который хотите перейти  >>>");
                    driverName = Console.ReadLine();
                    bool driveNameCorrect = false;
                    foreach (string drive in driveNames)
                    {
                        if (drive == driverName)
                        {
                            driveNameCorrect = true;
                        }
                    }
                    if (driveNameCorrect)
                    {
                        break;
                    }
                    Console.WriteLine("Имя диска введено некорректно!!!");
                } while (true);
                return driverName;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Произошла ошибка при выполнении операции: {e.Message}");
                return DriveInfo.GetDrives()[0].Name.ToString();
            }
        }

        /// <summary>
        /// Метод предоставляет пользователю информацию о дисках, запрашивает имя диска и перходит на него.
        /// </summary>
        private static void GoToDrive()
        {
            try
            {
                DriveInfo[] availableDrives = DriveInfo.GetDrives();
                string[] driverNames = new string[availableDrives.Length];
                int position = 0;
                Console.WriteLine("Список доступных дисков:");
                foreach (DriveInfo currentDrive in availableDrives)
                {
                    driverNames[position++] = currentDrive.Name;
                    Console.Write($"\tДиск {currentDrive.Name} ({currentDrive.DriveFormat}) ");
                    if (currentDrive.TotalSize > 0)
                    {
                        double partOfUsedSapce = (double)currentDrive.TotalFreeSpace / (double)currentDrive.TotalSize;
                        Console.WriteLine("[" + new string('|', 20 - (int)(partOfUsedSapce * 20)) +
                            new string(' ', (int)(partOfUsedSapce * 20)) + $"]свободно {currentDrive.TotalFreeSpace / 1024 / 1024 / 1024}" +
                            $"/{currentDrive.TotalSize / 1024 / 1024 / 1024} гигабайт ({(partOfUsedSapce * 100).ToString("F2")}%).");
                    }
                    else
                    {
                        Console.WriteLine("Диск имеет размер 0 байт.");
                    }
                }
                Directory.SetCurrentDirectory(ReadDriveName(driverNames));
            }
            catch (Exception e)
            {
                Console.WriteLine($"Произошла ошибка при выполнении операции: {e.Message}");
            }
        }

        /// <summary>
        /// Метод выводит все файлы и папки, лежащие в директории, в которой сейчас находится пользователь.
        /// </summary>
        private static void PrintFilesAndDirectoriesInCurrentDirectory()
        {
            try
            {
                Console.WriteLine($"Вы находитесь в {Directory.GetCurrentDirectory()}, лежащие здесь папки и файлы:");
                bool isDirectoryEmpty = true;
                foreach (string directoryName in Directory.GetDirectories(Directory.GetCurrentDirectory()))
                {
                    isDirectoryEmpty = false;
                    Console.WriteLine($"\t{directoryName}");
                }
                foreach (string fileName in Directory.GetFiles(Directory.GetCurrentDirectory()))
                {
                    isDirectoryEmpty = false;
                    Console.WriteLine($"\t{fileName}");
                }
                if (isDirectoryEmpty)
                {
                    Console.WriteLine("\tДанная директория пуста.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Произошла ошибка при выполнении операции: {e.Message}");
            }
        }

        /// <summary>
        /// Метод выводит все файлы, лежащие в директории path и ее поддиректориях.
        /// </summary>
        /// <param name="path">Строка, директория, в которой выводятся файлы.</param>
        /// <param name="depth">Целое число, глубина рекурсии.</param>
        private static void RecursiveOutputFiles(string path, int depth)
        {
            try
            {
                if (depth >= 50)
                {
                    return;
                }
                try
                {
                    Directory.GetDirectories(path);
                }
                catch (Exception)
                {
                    return;
                }
                foreach (string directoryName in Directory.GetDirectories(path))
                {
                    RecursiveOutputFiles(directoryName, depth + 1);
                }
                foreach (string fileName in Directory.GetFiles(path))
                {
                    Console.WriteLine($"\t{fileName}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Произошла ошибка при выполнении операции: {e.Message}");
                return;
            }
        }

        /// <summary>
        /// Метод выводит все файлы, удовлетворяющие регулярному выражению и 
        /// лежащие в директории, в которой сейчас находится пользователь.
        /// </summary>
        /// <param name="regularExpression">Строка, регулярное выражение, с помощью которого ищутся файлы.</param>
        private static void PrintFilesByMask(string regularExpression)
        {
            try
            {
                List<string> goodFiles = new List<string>();
                foreach (string fileName in Directory.GetFiles(Directory.GetCurrentDirectory()))
                {
                    if (Regex.IsMatch(Path.GetFileName(fileName), regularExpression))
                    {
                        goodFiles.Add(fileName);
                    }
                }
                Console.WriteLine($"Вы находитесь в {Directory.GetCurrentDirectory()}, " +
                    $"файлы, подходящие под указанную маску:");
                if (goodFiles.Count == 0)
                {
                    Console.WriteLine("\tТаких файлов не найдено.");
                }
                foreach (string fileName in goodFiles)
                {
                    Console.WriteLine($"\t{fileName}");
                }
            }
            catch (RegexParseException)
            {
                Console.WriteLine("Ошибка во введенной маске!!!");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Произошла ошибка при выполнении операции: {e.Message}");
            }
        }

        /// <summary>
        /// Метод печатает на консоль все файлы, лежащие в указанной директории и ее поддиректориях 
        /// и подходящие под указанное регулярное выражение.
        /// </summary>
        /// <param name="path">Строка, директория, в которой будут искаться файлы.</param>
        /// <param name="regularExpression">Строка, регулярное выражение, по которому ищутся файлы.</param>
        /// <param name="depth">Целое число, глубина рекурсии.</param>
        /// <param name="wasMaskError">Ссылка на логическую переменную, была ли замечена ошибка в маске.</param>
        /// <returns>Целое число, суммарное количество выведенных файлов.</returns>
        private static int PrintRecursiveFilesByMask(string path, string regularExpression,
            int depth, out bool wasMaskError)
        {
            try
            {
                wasMaskError = false;
                if (depth >= 50)
                {
                    return 0;
                }
                bool maskError = false;
                int amountFiles = 0;
                try
                {
                    Directory.GetDirectories(path);
                }
                catch (Exception)
                {
                    return 0;
                }
                foreach (string directoryName in Directory.GetDirectories(path))
                {
                    try
                    {
                        amountFiles += PrintRecursiveFilesByMask(directoryName, regularExpression,
                        depth + 1, out maskError);
                        if (maskError)
                        {
                            wasMaskError = true;
                            return 0;
                        }
                    }
                    catch (UnauthorizedAccessException) { }
                    catch (Exception) { }
                }
                foreach (string fileName in Directory.GetFiles(path))
                {
                    try
                    {
                        if (Regex.IsMatch(Path.GetFileName(fileName), regularExpression))
                        {
                            amountFiles++;
                            Console.WriteLine($"\t{fileName}");
                        }
                    }
                    catch (UnauthorizedAccessException) { }
                    catch (Exception) { }
                }
                return amountFiles;
            }
            catch (RegexParseException)
            {
                wasMaskError = true;
                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Произошла ошибка при выполнении операции: {e.Message}");
                wasMaskError = false;
                return 0;
            }
        }

        /// <summary>
        /// Метод, который копирует все файлы из данной директории и ее поддиректорий,
        /// удовлетворяющие регулярному выражению в указанную директорию.
        /// </summary>
        /// <param name="path">Строка, директория, из которой происходит копирование.</param>
        /// <param name="newPath">Строка, директория, в которую происходит копирование файлов.</param>
        /// <param name="regularExpression">Строка, регулярное выражение, по которому ищутся файлы.</param>
        /// <param name="depth">Целое число, глубина рекурсии.</param>
        /// <param name="rewrite">Целое число, 0 - если файл уже есть в директории, куда копируются
        /// файлы, он не будет заменен, 1 - будет заменен.</param>
        /// <param name="wasMaskError">Ссылка на логическую переменную, была ли замечена ошибка в маске.</param>
        private static void RecursiveCopyByMask(string path, string newPath, string regularExpression,
            int depth, int rewrite, out bool wasMaskError)
        {
            wasMaskError = false;
            try
            {
                if (depth >= 50)
                {
                    return;
                }
                bool wasError = false;
                try
                {
                    Directory.GetDirectories(path);
                }
                catch (Exception)
                {
                    return;
                }
                foreach (string currentDirectory in Directory.GetDirectories(path))
                {
                    RecursiveCopyByMask(currentDirectory, newPath, regularExpression,
                        depth + 1, rewrite, out wasError);
                    if (wasError)
                    {
                        wasMaskError = true;
                        return;
                    }
                }
                foreach (string fileName in Directory.GetFiles(path))
                {
                    if (Regex.IsMatch(Path.GetFileName(fileName), regularExpression) &&
                        (!File.Exists(Path.Combine(newPath, fileName)) || rewrite == 1))
                    {
                        if (File.Exists(Path.Combine(newPath, Path.GetFileName(fileName))))
                        {
                            File.Delete(Path.Combine(newPath, Path.GetFileName(fileName)));
                        }
                        File.Copy(fileName, Path.Combine(newPath, Path.GetFileName(fileName)), rewrite == 1);
                    }
                }
            }
            catch (RegexParseException)
            {
                wasMaskError = true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Произошла ошибка при выполнении операции: {e.Message}");
                return;
            }
        }

        /// <summary>
        /// Метод переходит в указанную директорию.
        /// </summary>
        /// <param name="possiblePath">Строка, директория, в которую происходит переход.</param>
        private static void GoToDirectory(string possiblePath)
        {
            try
            {
                Directory.SetCurrentDirectory(Path.Combine(Directory.GetCurrentDirectory(), possiblePath));
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Пользователь не имеет права доступа к данной директории!!!");
            }
            catch (Exception)
            {
                Console.WriteLine("Данная директория не распознана!!!");
            }
        }

        /// <summary>
        /// Метод переводит текст из одной кодировки в другую.
        /// </summary>
        /// <param name="text">Текст, который переводится из одной кодировки в другую.</param>
        /// <param name="from">Кодировка, из которой переводится текст.</param>
        /// <param name="to">Кодировка, в которую переводится текст.</param>
        /// <returns>Текст, уже в новой кодировке (в той же, если кодировки совпадают).</returns>
        private static string ConvertTextBetweenEncodings(string text, Encoding from, Encoding to)
        {
            try
            {
                if (from == to)
                {
                    return text;
                }
                byte[] textInBinaryFrom = from.GetBytes(text);
                byte[] textInBinaryTo = Encoding.Convert(from, to, textInBinaryFrom);
                string textConverted = to.GetString(textInBinaryTo);
                return textConverted;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Произошла ошибка при выполнении операции: {e.Message}");
                return "";
            }
        }

        /// <summary>
        /// Метод считывает содержимое файла в нужной кодировке и возвращает его.
        /// </summary>
        /// <param name="path">Путь к файлу, из которого считывается текст.</param>
        /// <param name="fileEncoding">Кодировка, в которой нужно считывать файл.</param>
        /// <returns>Содержимое файла в виде одной строки.</returns>
        private static string GetFileText(string path, Encoding fileEncoding)
        {
            try
            {
                StreamReader fileReader = new StreamReader(path, fileEncoding,
                    detectEncodingFromByteOrderMarks: false);
                string fileText = fileReader.ReadToEnd();
                fileReader.Close();
                return ConvertTextBetweenEncodings(fileText, fileEncoding, Encoding.UTF8);
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("У вас нет прав на чтение данного файла!!!");
                return "У вас нет прав на чтение данного файла!!!";
            }
            catch (Exception e)
            {
                Console.WriteLine($"Произошла ошибка при выполнении операции: {e.Message}");
                return "";
            }
        }

        /// <summary>
        /// Метод выводит содержимое текстового файла в указанной кодировке.
        /// </summary>
        /// <param name="filePath">Строка, путь к читаемому файлу.</param>
        /// <param name="fileEncoding">Кодировка, в которой читается файл.</param>
        private static void PrintTextFile(string filePath, Encoding fileEncoding)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    Console.WriteLine("Запрашиваемый файл не найден");
                    return;
                }
                if (new FileInfo(filePath).Length > (1 << 20))
                {
                    Console.WriteLine("Файл весит более 1 мб, файловый не будет выводить его содержимое.");
                    return;
                }
                Console.WriteLine("Содержимое файла:");
                Console.WriteLine(GetFileText(filePath, fileEncoding));
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("У вас нет прав на чтение данного файла!!!");
            }
            catch (OutOfMemoryException)
            {
                Console.WriteLine("Файл слишком большой, чтобы работать с ним в данном файловом менеджере.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Произошла ошибка при выполнении операции: {e.Message}");
            }
        }

        /// <summary>
        /// Метод возвращает кодировку, соответствующую ее названию.
        /// </summary>
        /// <param name="encodingName">Строка, название кодировки.</param>
        /// <returns>Encoding, соответствующая кодировка.</returns>
        private static Encoding GetEncoding(string encodingName)
        {
            try
            {
                switch (encodingName)
                {
                    case "utf8":
                        return Encoding.UTF8;
                    case "ascii":
                        return Encoding.ASCII;
                    case "latin1":
                        return Encoding.Latin1;
                    case "unicode":
                        return Encoding.Unicode;
                    default:
                        return null;
                }
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Произошла ошибка при выполнении операции: {e.Message}");
                return null;
            }
        }

        /// <summary>
        /// Метод создает текстовый файл в указанной кодировке.
        /// </summary>
        /// <param name="fileName">Строка, путь к файлу.</param>
        /// <param name="currentEnconding">Encoding, кодировка, в которой создастся файл.</param>
        private static void WriteTextFile(string fileName, Encoding currentEnconding)
        {
            try
            {
                if (fileName == null || fileName == "")
                {
                    Console.WriteLine("Имя файла не может быть пустым!!!");
                    return;
                }
                if (fileName.Contains(Path.DirectorySeparatorChar) &&
                    !Directory.Exists(Path.GetDirectoryName(fileName)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(fileName));
                }
                if (File.Exists(fileName))
                {
                    int rewrite = -1;
                    do
                    {
                        Console.Write("Текстовый файл с таким именем уже " +
                            "существует, переписать его содержимое(y/n)?>>>");
                        string inputString = Console.ReadLine();
                        if (inputString == "y")
                        {
                            rewrite = 1;
                        }
                        if (inputString == "n")
                        {
                            rewrite = 0;
                        }
                    } while (rewrite == -1);
                    if (rewrite == 0)
                    {
                        Console.WriteLine("Файл не будет переписан, операция закончена.");
                        return;
                    }
                    if (rewrite == 1)
                    {
                        File.Delete(fileName);
                        Console.WriteLine("Файл будет переписан.");
                    }
                }
                File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), fileName), ConvertTextBetweenEncodings(
                    "This file was created with the best file manager (according to developer of file manager).",
                    Encoding.UTF8, currentEnconding), currentEnconding);
                Console.WriteLine("Файл успешно создан.");
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("У вас нет прав на изменение данного файла!!!");
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Имя создаваемого файла некорректно!!!");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Произошла ошибка при выполнении операции: {e.Message}");
            }
        }

        /// <summary>
        /// Метод копирующий файл из одной директории в другую.
        /// </summary>
        /// <param name="pathFrom">Строка, путь, откуда копируется файл.</param>
        /// <param name="pathTo">Строка, путь, куда копируется файл.</param>
        private static void CopyFile(string pathFrom, string pathTo)
        {
            try
            {
                if (pathTo == null || pathTo.Length == 0 || pathFrom == null || pathFrom.Length == 0)
                {
                    Console.WriteLine("Имя файла не может быть пустым!!!");
                    return;
                }
                if (!File.Exists(pathFrom))
                {
                    Console.WriteLine("Копируемый файл не найден, операция не была выполнена.");
                    return;
                }
                if (new DirectoryInfo(pathFrom).FullName == new DirectoryInfo(pathTo).FullName)
                {
                    Console.WriteLine("Пути откуда копируется файл и куда копируется" +
                        " файл совпадают, копирование не будет произведено.");
                    return;
                }
                if (File.Exists(pathTo))
                {
                    int rewrite = -1;
                    string inputString;
                    do
                    {
                        Console.Write("В директории, в которую вы хотите скопировать" +
                            " файл, уже есть файл с таким именем. Заменить его(yes/no)?>>>");
                        inputString = Console.ReadLine();
                        if (inputString == "yes")
                        {
                            rewrite = 1;
                        }
                        if (inputString == "no")
                        {
                            rewrite = 0;
                        }
                    } while (rewrite == -1);
                    if (rewrite == 0)
                    {
                        Console.WriteLine("Файл не будет заменен, операция закончена.");
                        return;
                    }
                }
                if (Path.GetFileName(pathTo) == null || Path.GetFileName(pathTo).Length == 0)
                {
                    Console.WriteLine("Путь к копии файла задан неверно!!!");
                    return;
                }
                string upperPathTo = Directory.GetParent(pathTo).ToString();
                if (!Directory.Exists(upperPathTo))
                {
                    Directory.CreateDirectory(upperPathTo);
                }
                File.Copy(pathFrom, pathTo, true);
                Console.WriteLine("Файл успешно скопирован.");
            }
            catch (IOException e)
            {
                Console.WriteLine($"Произошла ошибка при копировании:{e.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Произошла ошибка при выполнении операции: {e.Message}");
            }
        }

        /// <summary>
        /// Метод перемещает файл из одной директории в другую.
        /// </summary>
        /// <param name="pathFrom">Строка, путь, откуда происходит перемещение.</param>
        /// <param name="pathTo">Строка, путь, куда происходит перемещение.</param>
        private static void MoveFile(string pathFrom, string pathTo)
        {
            try
            {
                if (pathFrom == null || pathFrom.Length == 0 || pathTo == null || pathTo.Length == 0)
                {
                    Console.WriteLine("Имя файла не может быть пустым!!!");
                    return;
                }
                if (!File.Exists(pathFrom))
                {
                    Console.WriteLine("Копируемый файл не найден, операция не была выполнена.");
                    return;
                }
                if (new DirectoryInfo(pathFrom).FullName == new DirectoryInfo(pathTo).FullName)
                {
                    Console.WriteLine("Пути откуда копируется файл и куда переносится" +
                        " файл совпадают, копирование не будет произведено.");
                    return;
                }
                if (File.Exists(pathTo))
                {
                    int rewrite = -1;
                    do
                    {
                        Console.Write("В директории, в которую вы хотите скопировать" +
                            " файл, уже есть файл с таким именем. Заменить его(y/n)?>>>");
                        string inputString = Console.ReadLine();
                        if (inputString == "y")
                        {
                            rewrite = 1;
                        }
                        if (inputString == "n")
                        {
                            rewrite = 0;
                        }
                    } while (rewrite == -1);
                    if (rewrite == 0)
                    {
                        Console.WriteLine("Файл не будет заменен, операция закончена.");
                        return;
                    }
                }
                if (Path.GetFileName(pathTo) == null || Path.GetFileName(pathTo).Length == 0)
                {
                    Console.WriteLine("Новый путь к файлу задан неверно!!!");
                    return;
                }
                string upperPathTo = Directory.GetParent(pathTo).ToString();
                if (!Directory.Exists(upperPathTo))
                {
                    Directory.CreateDirectory(upperPathTo);
                }
                File.Move(pathFrom, pathTo, true);
                Console.WriteLine("Файл успешно перемещен.");
            }
            catch (IOException e)
            {
                Console.WriteLine($"Произошла ошибка при перемещении:{Environment.NewLine}{e.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Произошла ошибка при выполнении операции: {e.Message}");
            }
        }

        /// <summary>
        /// Метод удаляет файл по заданной директории и имени файла.
        /// </summary>
        /// <param name="filePath">Строка, путь к файлу, который удаляется.</param>
        private static void DeleteFile(string filePath)
        {
            try
            {
                if (filePath == null || filePath.Length == 0)
                {
                    Console.WriteLine("Имя файла не может быть пустым!!!");
                    return;
                }
                if (!File.Exists(filePath))
                {
                    Console.WriteLine("Файл не был найден и, следовательно, не был удален.");
                }
                else
                {
                    File.Delete(filePath);
                    Console.WriteLine("Файл успешно удален.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Произошла ошибка при выполнении операции: {e.Message}");
            }
        }

        /// <summary>
        /// Метод выводит в консоль объединение содержимого списка текстовых файлов.
        /// </summary>
        /// <param name="mergingFiles">Список путей к текстовым файлам, содержимое которых конкотинируется.</param>
        private static void MergeFiles(List<(string, Encoding)> files, string resultFile)
        {
            try
            {
                if (resultFile == null || resultFile.Length == 0)
                {
                    Console.WriteLine("Имя файла не может быть пустым!!!");
                    return;
                }
                long totalSize = 0;
                StringBuilder mergedText = new StringBuilder("");
                foreach ((string, Encoding) currentFile in files)
                {
                    if (!File.Exists(currentFile.Item1))
                    {
                        Console.WriteLine($"Файл {currentFile.Item1} не найден. Действие операции прервано.");
                        return;
                    }
                    totalSize += new FileInfo(currentFile.Item1).Length;
                    mergedText.Append(GetFileText(currentFile.Item1, currentFile.Item2));
                }
                if (totalSize > (1 << 20))
                {
                    Console.WriteLine("Суммарный объём всех объединяемых файлов " +
                        "больше 1 мб, объединение файлов не будет произведено.");
                    return;
                }
                if (resultFile.Contains(Path.DirectorySeparatorChar) &&
                    !Directory.Exists(Path.GetDirectoryName(resultFile)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(resultFile));
                }
                if (File.Exists(resultFile))
                {
                    File.Delete(resultFile);
                }
                Console.WriteLine(mergedText.ToString());
                File.WriteAllText(resultFile, mergedText.ToString(), Encoding.UTF8);
                mergedText.Clear();
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("У вас нет прав на чтение одного из указанных файлов!!!");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Произошла ошибка при выполнении операции: {e.Message}");
            }
        }

        /// <summary>
        /// Метод решает задачу о наибольшей общей подпоследовательности, возвращает список пар целых чисел,
        /// таких, что в паре (i,j) i-ый символ первой строки равен j-символу второй строки, первые и вторые 
        /// элементы каждой пары возрастают с ростом индекса в списке, а размер списка максимален.
        /// </summary>
        /// <param name="firstText">Первая строка.</param>
        /// <param name="secondText">Вторая строка.</param>
        /// <returns>Список кортежей (елое число, целое число).</returns>
        private static List<(int, int)> GetListOfLCSIndexes(string firstText, string secondText)
        {
            try
            {
                int[,] lcs = new int[firstText.Length, secondText.Length];
                (int, int)[,] parent = new (int, int)[firstText.Length, secondText.Length];
                for (int indexRow = 1; indexRow < lcs.GetLength(0); indexRow++)
                    for (int indexColumn = 1; indexColumn < lcs.GetLength(1); indexColumn++)
                    {
                        if (firstText[indexRow] == secondText[indexColumn])
                        {
                            lcs[indexRow, indexColumn] = lcs[indexRow - 1, indexColumn - 1] + 1;
                            parent[indexRow, indexColumn] = (indexRow - 1, indexColumn - 1);
                        }
                        else if (lcs[indexRow - 1, indexColumn] > lcs[indexRow, indexColumn - 1])
                        {
                            lcs[indexRow, indexColumn] = lcs[indexRow - 1, indexColumn];
                            parent[indexRow, indexColumn] = (indexRow - 1, indexColumn);
                        }
                        else
                        {
                            lcs[indexRow, indexColumn] = lcs[indexRow, indexColumn - 1];
                            parent[indexRow, indexColumn] = (indexRow, indexColumn - 1);
                        }
                    }
                int currentI = firstText.Length - 1, currentJ = secondText.Length - 1;
                List<(int, int)> matchedIndexes = new List<(int, int)>();
                while (currentI != 0 && currentJ != 0)
                {
                    if (parent[currentI, currentJ] == (currentI - 1, currentJ - 1))
                    {
                        matchedIndexes.Add((currentI, currentJ));
                    }
                    (currentI, currentJ) = parent[currentI, currentJ];
                }
                matchedIndexes.Reverse();
                return matchedIndexes;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Произошла ошибка при выполнении операции: {e.Message}");
                return null;
            }
        }

        /// <summary>
        /// Метод выводит операции, которые нужно провести с первой строкой, чтобы из нее получить вторую.
        /// </summary>
        /// <param name="matchedIndexes">Список кортежей, образующих наибольшую общую подпоследовательность.</param>
        /// <param name="firstText">Первая строка.</param>
        /// <param name="secondText">Вторая строка.</param>
        private static void OutputUnmatchedSubstrings(List<(int, int)> matchedIndexes,
            string firstText, string secondText)
        {
            try
            {
                int currentI = 0, currentJ = 0;
                foreach ((int, int) pairedIndexes in matchedIndexes)
                {
                    currentI++;
                    currentJ++;
                    int copyI = currentI, copyJ = currentJ;
                    StringBuilder firstUnmatch = new StringBuilder("");
                    StringBuilder secondUnmatch = new StringBuilder("");
                    while (currentI != pairedIndexes.Item1 && currentI + 1 < firstText.Length)
                    {
                        firstUnmatch.Append(firstText[currentI]);
                        currentI++;
                    }
                    while (currentJ != pairedIndexes.Item2 && currentJ + 1 < secondText.Length)
                    {
                        secondUnmatch.Append(secondText[currentJ]);
                        currentJ++;
                    }
                    if (firstUnmatch.ToString() == secondUnmatch.ToString()) continue;
                    if (firstUnmatch.Length == 0)
                    {
                        if (secondUnmatch.Length != 0)
                        {
                            Console.WriteLine($"Добавить после {copyI - 1} '{secondUnmatch}'");
                        }
                    }
                    else
                    {
                        if (secondUnmatch.Length == 0)
                        {
                            Console.WriteLine($"Удалить подстроку {copyI}-{currentI - 1}, '{firstUnmatch}'");
                        }
                        else
                        {
                            Console.WriteLine($"Заменить символы {copyI}-{currentI - 1} " +
                                $"'{firstUnmatch}' на '{secondUnmatch}'");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Произошла ошибка при выполнении операции: {e.Message}");
            }
        }

        /// <summary>
        /// Метод выводит различия между двумя текстовыми файлами.
        /// </summary>
        /// <param name="firstFile">Строка, путь к первому файлу.</param>
        /// <param name="secondFile">Строка, путь ко второму файлу.</param>
        private static void GetFilesDifferences(string firstFile, string secondFile,
            Encoding firstEncoding, Encoding secondEncoding)
        {
            try
            {
                if (!File.Exists(firstFile))
                {
                    Console.WriteLine($"Файла {firstFile} не существует, выполнение операции прекращено.");
                    return;
                }
                if (!File.Exists(secondFile))
                {
                    Console.WriteLine($"Файла {secondFile} не существует, выполнение операции прекращено.");
                    return;
                }
                if (new FileInfo(firstFile).Length > (1 << 10) || new FileInfo(secondFile).Length > (1 << 10))
                {
                    Console.WriteLine("Один из текстовых файлов содержит больше" +
                        " 1000 символов, операция не будет выполнена.");
                    return;
                }
                string firstText = $"@{File.ReadAllText(firstFile)}@";
                string secondText = $"@{File.ReadAllText(secondFile)}@";
                if (firstText == secondText)
                {
                    Console.WriteLine("Содержимое текстовых файлов одинаково.");
                    return;
                }
                if (firstText.Length > 1002 || secondText.Length > 1002)
                {
                    Console.WriteLine("Один из текстовых файлов содержит больше" +
                        " 1000 символов, операция не будет выполнена.");
                    return;
                }
                List<(int, int)> matchedIndexes = GetListOfLCSIndexes(firstText, secondText);
                if (matchedIndexes == null)
                {
                    Console.WriteLine("Что-то пошло не так.");
                    return;
                }
                OutputUnmatchedSubstrings(matchedIndexes, firstText, secondText);
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("У вас нет прав на чтение одного из указанных файлов!!!");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Произошла ошибка при выполнении операции: {e.Message}");
            }
        }

        /// <summary>
        /// Метод считывает команду с консоли.
        /// </summary>
        /// <returns>Возвращает строку, первую правильно введенную команду.</returns>
        private static string ReadComand()
        {
            string inputString;
            do
            {
                Console.Write($"{Directory.GetCurrentDirectory()}>>>");
                inputString = Console.ReadLine();
                if (inputString == "cd" || inputString == "drive" || inputString == "ls" ||
                    inputString == "read" || inputString == "create" || inputString == "copy" ||
                    inputString == "move" || inputString == "delete" || inputString == "merge" ||
                    inputString == "ls all" || inputString == "find" || inputString == "find all" ||
                    inputString == "copy all" || inputString == "diff" || inputString == "help" ||
                    inputString == "exit")
                    break;
                Console.WriteLine("Команда не распознана!!!");
            } while (true);
            return inputString;
        }

        /// <summary>
        /// Метод считывает директорию и переходит в нее.
        /// </summary>
        private static void TerminalCD()
        {
            try
            {
                Console.Write("Введите директорию, в которую хотите перейти>>>");
                string needingDirectory = Console.ReadLine();
                GoToDirectory(needingDirectory);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Произошла ошибка при выполнении операции: {e.Message}");
            }
        }

        /// <summary>
        /// Метод пеходит в выбранный пользователем диск.
        /// </summary>
        private static void TerminalDrive()
        {
            try
            {
                GoToDrive();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Произошла ошибка при выполнении операции: {e.Message}");
            }
        }

        /// <summary>
        /// Метод показывает все файлы и папки в директории, в которой находится пользователя.
        /// </summary>
        private static void TerminalLS()
        {
            try
            {
                PrintFilesAndDirectoriesInCurrentDirectory();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Произошла ошибка при выполнении операции: {e.Message}");
            }
        }

        /// <summary>
        /// Метод считывает у пользователя кодировку и путь к файлу, и потом выводит его.
        /// </summary>
        private static void TerminalRead()
        {
            try
            {
                Console.Write("Введите кодировку>>>");
                Encoding currentEncoding = GetEncoding(Console.ReadLine());
                if (currentEncoding == null)
                {
                    Console.WriteLine("Кодировка не распознана!!! Выполнение команды прервано.");
                }
                else
                {
                    Console.Write("Введите директорию>>>");
                    PrintTextFile(Console.ReadLine(), currentEncoding);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Произошла ошибка при выполнении операции: {e.Message}");
            }
        }

        /// <summary>
        /// Метод запрашивает у пользователя кодировку и путь к создаваемому 
        /// файлу, потом создает файл в указанной кодировке.
        /// </summary>
        private static void TerminalWrite()
        {
            try
            {
                Console.Write("Введите кодировку>>>");
                Encoding currentEncoding = GetEncoding(Console.ReadLine());
                if (currentEncoding == null)
                {
                    Console.WriteLine("Кодировка не распознана!!! Выполнение команды прервано.");
                }
                else
                {
                    Console.Write("Введите директорию>>>");
                    WriteTextFile(Console.ReadLine(), currentEncoding);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Произошла ошибка при выполнении операции: {e.Message}");
            }
        }

        /// <summary>
        /// Метод запрашивает у пользователя путь к копируемому файлу и 
        /// новый путь и имя к файлу при копировании и копирует файл.
        /// </summary>
        private static void TerminalCopy()
        {
            try
            {
                Console.Write("Директория из которой копируется файл(вместе с именем файла)>>>");
                string pathFrom = Console.ReadLine();
                Console.Write("Директория в которую копируется файл" +
                    "(вместе с именем файла, можно задать новое имя)>>>");
                string pathTo = Console.ReadLine();
                CopyFile(pathFrom, pathTo);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Произошла ошибка при выполнении операции: {e.Message}");
            }
        }

        /// <summary>
        /// Метод запрашивает у пользователя путь к перемещаемому файлу и путь
        /// и имя куда перемещается файл и потом перемещает файл.
        /// </summary>
        private static void TerminalMove()
        {
            try
            {
                Console.Write("Директория из которой переносится файл(вместе с именем файла)>>>");
                string pathFrom = Console.ReadLine();
                Console.Write("Директория в которую переносится файл" +
                    "(вместе с именем файла, можно задать новое имя)>>>");
                string pathTo = Console.ReadLine();
                MoveFile(pathFrom, pathTo);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Произошла ошибка при выполнении операции: {e.Message}");
            }
        }

        /// <summary>
        /// Метод запрашивает у пользователя путь к файлу и потом удаляет его.
        /// </summary>
        private static void TerminalDelete()
        {
            try
            {
                Console.Write("Введите директоирию(вместе с именем файла)>>>");
                DeleteFile(Console.ReadLine());
            }
            catch (Exception e)
            {
                Console.WriteLine($"Произошла ошибка при выполнении операции: {e.Message}");
            }
        }

        /// <summary>
        /// Метод запрашивает у пользователя пути к файлам, а потом выводит объединение их содержимого в консоль.
        /// </summary>
        private static void TerminalMerge()
        {
            try
            {
                Console.Write("Введите файл, в который хотите записать объединение содержимого файлов>>>");
                string resultFile = Console.ReadLine();
                Console.WriteLine("Вводите файлы по одному в строке, чтобы прекратить" +
                    " ввод файлов вместо ввода очередного файла нажмите enter.");
                List<(string, Encoding)> files = new List<(string, Encoding)>();
                while (true)
                {
                    Console.Write("Введите адрес файла/пустую строку>>>");
                    string currentFile = Console.ReadLine();
                    if (currentFile.Length == 0 || currentFile == "\n" || currentFile == "\r" ||
                        currentFile == Environment.NewLine.ToString())
                        break;
                    Console.Write("Введите кодировку, в которой должен быть считан данный файл>>>");
                    string fileEncoding = Console.ReadLine();
                    if (GetEncoding(fileEncoding) == null)
                    {
                        Console.WriteLine("Введенная кодировка не распознана, " +
                            "выполнение данной операции остановлено!!!");
                        return;
                    }
                    files.Add((currentFile, GetEncoding(fileEncoding)));
                }
                MergeFiles(files, resultFile);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Произошла ошибка при выполнении операции: {e.Message}");
            }
        }

        /// <summary>
        /// Метод выводит все файлы, лежащие в директории, в которой сейчас находится пользователь и ее поддиректориях.
        /// </summary>
        private static void TerminalRecursiveOutputFiles()
        {
            try
            {
                Console.WriteLine($"Вы находитесь в {Directory.GetCurrentDirectory()}, все лежащие " +
                    $"здесь файлы(в том числе в поддиректориях данной директории и их поддиректоиях):");
                RecursiveOutputFiles(Directory.GetCurrentDirectory(), 0);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Произошла ошибка при выполнении операции: {e.Message}");
            }
        }

        /// <summary>
        /// Метод запрашивает у пользователя регулярное выражение и выводит все файлы, 
        /// лежащие в данной директории и удовлетворяющие регулярному выражению.
        /// </summary>
        private static void TerminalFindFilesByMask()
        {
            try
            {
                Console.Write("Введите регулярное выражение, по которому хотите найти файлы>>>");
                string regularExpression = Console.ReadLine();
                PrintFilesByMask(regularExpression);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Произошла ошибка при выполнении операции: {e.Message}");
            }
        }

        /// <summary>
        /// Метод запрашивает у пользователя регулярное выражение, потом выводит все файлы, лежащие 
        /// в данной директории и ее поддиректориях и удовлетворяющие регулярному выражению.
        /// </summary>
        private static void TerminalFindRecursiveFilesByMask()
        {
            try
            {
                Console.Write("Введите регулярное выражение, по которому хотите найти файлы>>>");
                string regularExpression = Console.ReadLine();
                bool wasMaskError = false;
                int amountFiles = PrintRecursiveFilesByMask(Directory.GetCurrentDirectory(),
                    regularExpression, 0, out wasMaskError);
                if (wasMaskError)
                {
                    Console.WriteLine("Ошибка во введенной маске!!!");
                }
                else if (amountFiles == 0)
                {
                    Console.WriteLine("По заданной маске не найдено ни одного файла.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Произошла ошибка при выполнении операции: {e.Message}");
            }
        }

        /// <summary>
        /// Метод запрашивает у пользователя директорию, откуда копируются файлы, регулярное выражение, директорию,
        /// куда копируются файлы и нужно ли переписывать файлы в случае, если их имена повторяются и копирует все
        /// файлы, удовлетворяющие заданному регулярному выражению, из одной указанной директории в другую указанную
        /// директорию.
        /// </summary>
        private static void TerminalRecursiveCopyAllFiles()
        {
            try
            {
                Console.Write("Введите директорию, из которой будут копироваться файлы>>>");
                string oldPath = Console.ReadLine();
                Console.Write("Введите маску, по которой будет производиться копирование файлов>>>");
                string regularExpression = Console.ReadLine();
                Console.Write("Введите директорию, в которую будут скопированы файлы>>>");
                string newPath = Console.ReadLine();
                int rewrite = -1;
                string inputString;
                do
                {
                    Console.Write("Заменять ли файлы при совпадении имен(y/n)?>>>");
                    inputString = Console.ReadLine();
                    if (inputString == "y")
                    {
                        rewrite = 1;
                    }
                    if (inputString == "n")
                    {
                        rewrite = 0;
                    }
                } while (rewrite == -1);
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                    Console.WriteLine("Указанной директории для копирования не существовало, она была создана.");
                }
                bool wasMaskError = false;
                RecursiveCopyByMask(oldPath, newPath, regularExpression, 0, rewrite, out wasMaskError);
                if (wasMaskError)
                {
                    Console.WriteLine("Ошибка во введенной маске файла.");
                }
                else
                {
                    Console.WriteLine($"Файлы успешно скопированы в {newPath}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Произошла ошибка при выполнении операции: {e.Message}");
            }
        }

        /// <summary>
        /// Метод запрашивает у пользователя пути к двум файлам, а потом выводит то, какие 
        /// действия нужно сделать с первым файлом, чтобы получить из него второй файл.
        /// </summary>
        private static void TerminalGetFilesDiffirences()
        {
            try
            {
                Console.Write("Введите директорию и имя первого файла>>>");
                string firstFile = Console.ReadLine();
                Console.Write("Введите кодировку, в которой хотите прочитать файл>>>");
                string firstEncodingName = Console.ReadLine();
                Encoding firstEncoding = GetEncoding(firstEncodingName);
                if (firstEncoding == null)
                {
                    Console.WriteLine("Введенная кодировка не распознана, выполнение данной операции остановлено.");
                    return;
                }
                Console.Write("Введите директорию и имя второго файла>>>");
                string secondFile = Console.ReadLine();
                Console.Write("Введите кодировку, в которой хотите прочитать файл>>>");
                string secondEncodingName = Console.ReadLine();
                Encoding secondEncoding = GetEncoding(secondEncodingName);
                if (secondEncoding == null)
                {
                    Console.WriteLine("Введенная кодировка не распознана, выполнение данной операции остановлено.");
                    return;
                }
                GetFilesDifferences(firstFile, secondFile, firstEncoding, secondEncoding);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Произошла ошибка при выполнении операции: {e.Message}");
            }
        }

        /// <summary>
        /// Метод выводит информацию о том, как работать с файловым менеджером.
        /// </summary>
        private static void TerminalHelp()
        {
            Console.WriteLine("Эта программа - простой файловый менеджер. Он умеет выполнять следующие действия " +
                "(везде вводится вначале команда, потом дополнительные аргументы на новых строках):");
            Console.WriteLine("\tcd - перейти в указанную директорию, поддерживает абсолютный и относительный путь" +
                " (аргумент - путь к директории);");
            Console.WriteLine("\tdrive - переключение между дисками, получение информации о дисках (аргумент - " +
                "имя диска, на который вы хотите переключиться);");
            Console.WriteLine("\tls - вывод всех файлов и папок в директории, в которой Вы сейчас находитесь;");
            Console.WriteLine("\tls all - вывод всех файлов в данной директории (в том числе и в поддиректориях);");
            Console.WriteLine("\tread - вывод содержимого текстового " +
                "файла на консоль (аргументы - кодировка, путь к файлу);");
            Console.WriteLine("\tcreate - создание пустого текстового файла(аргументы -" +
                " кодировка для файла, имя файла (сразу путь и имя));");
            Console.WriteLine("\tcopy - копирование файла (аргументы - путь и имя файла, откуда Вы копируете; путь и" +
                " имя файла, куда Вы копируете). Обратите внимание, что вы можете таким образом скопировать файл под" +
                " новым именем, но копирование файла в ту же директорию, где он и находится, запрещено;");
            Console.WriteLine("\tcopy all - копирует все файлы в данной директории и ее поддиректориях в указанную " +
                "Вами директорию (аргументы - директория, из которой проиходит копирование; регулярное выражение, " +
                "определяющее, нужно ли копировать файл; директория, куда копируются все файлы; флаг, заменять ли " +
                "повторяющиеся файлы при копировании);");
            Console.WriteLine("\tmove - перемещает файл из одной директории в другую (аргументы - директория, откуда" +
                " переносится файл и директория, куда переносится файл);");
            Console.WriteLine("\tdelete - удаляет файл (аргумент - директория и имя файла, который удалится);");
            Console.WriteLine("\tmerge - объединяет содержимое нескольких текстовых файлов и выводит его" +
                " в консоль(аргументы - имена и пути файлов, которые вы хотите объеденить);");
            Console.WriteLine("\tfind - ищет все файлы в данной директории, которые соответствуют" +
                " указанному регулярному выражению (аргумент - регулярное выражение);");
            Console.WriteLine("\tfind all - ищет все файлы в данной директории и ее поддиректориях, " +
                "удовлетворяющие указанному регулярному выражению (аргумент - регулярное выражение);");
            Console.WriteLine("\tdiff - показывает различия между файлами(что нужно сделать с первым файлом, " +
                "чтобы получить второй) (аргументы - два файла, между которыми происходит сравнение);");
            Console.WriteLine("\thelp - выводит данное информационное меню;");
            Console.WriteLine("\texit - завершение работы программы;");
            Console.WriteLine($"{Environment.NewLine}При выборе кодировки предоставляется 4 варианта: utf8, " +
                $"ascii, latin1, unicode. Для выбора кодировки нужно ввести ее название, как указано здесь;");
            Console.WriteLine($"{Environment.NewLine}Регулярные выражения для поиска/копирования файлов " +
                $"задаются в их обычном виде (например, регулярное выражение .*\\.docx? найдет файлы .docx и .doc;");
        }

        static void Main(string[] args)
        {

            Console.WriteLine("Добро пожаловать в файловый менеджер. Если что-то непонятно, введите 'help'.");
            bool useProgram = true;
            while (useProgram)
            {
                try
                {
                    switch (ReadComand())
                    {
                        case "cd":
                            TerminalCD();
                            break;
                        case "drive":
                            TerminalDrive();
                            break;
                        case "ls":
                            TerminalLS();
                            break;
                        case "read":
                            TerminalRead();
                            break;
                        case "create":
                            TerminalWrite();
                            break;
                        case "copy":
                            TerminalCopy();
                            break;
                        case "copy all":
                            TerminalRecursiveCopyAllFiles();
                            break;
                        case "move":
                            TerminalMove();
                            break;
                        case "delete":
                            TerminalDelete();
                            break;
                        case "merge":
                            TerminalMerge();
                            break;
                        case "ls all":
                            TerminalRecursiveOutputFiles();
                            break;
                        case "find":
                            TerminalFindFilesByMask();
                            break;
                        case "find all":
                            TerminalFindRecursiveFilesByMask();
                            break;
                        case "diff":
                            TerminalGetFilesDiffirences();
                            break;
                        case "help":
                            TerminalHelp();
                            break;
                        case "exit":
                            useProgram = false;
                            break;
                        default:
                            Console.WriteLine("Команда не распознана!!!");
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Произошла ошибка при выполнении операции: {e.Message}");
                }

            }
        }
    }
}

