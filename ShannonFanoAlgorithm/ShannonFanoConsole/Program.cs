using ShannonFanoAlgorithm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShannonFanoConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var shannonFano = new ShannonFano();
            string path;
            int menuItem = ShowMenu();

            while(menuItem != 5)
            {
                switch(menuItem)
                {
                    case 1:
                        Console.Write("Введите путь к файлу: ");
                        path = Console.ReadLine();
                        if (!IsValidInputPath(path))
                        {
                            Console.WriteLine("Указан неправильный путь к файлу!");
                            break;
                        }
                        shannonFano.CreateShannonFanoEncodingDictionary(path);
                        Console.Write("Введите путь к файлу для сохранения: ");
                        path = Console.ReadLine();
                        if (!IsValidInputPath(path))
                        {
                            Console.WriteLine("Указан неправильный путь к файлу!");
                            break;
                        }
                        File.WriteAllText(path, shannonFano.EncodeString());
                        Console.WriteLine("Успешно закодировано!");
                        Console.WriteLine($"Степень сжатия текста: {shannonFano.CompressionRatio}");
                        break;
                    case 2:
                        Console.Write("Введите путь к файлу: ");
                        path = Console.ReadLine();
                        if (!IsValidInputPath(path))
                        {
                            Console.WriteLine("Указан неправильный путь к файлу!");
                            break;
                        }
                        var decodedText = shannonFano.DecodeString(path);
                        Console.Write("Введите путь к файлу для сохранения: ");
                        path = Console.ReadLine();
                        if (!IsValidInputPath(path))
                        {
                            Console.WriteLine("Указан неправильный путь к файлу!");
                            break;
                        }
                        File.WriteAllText(path, decodedText);
                        Console.WriteLine("Успешно декодировано!");
                        break;
                    case 3:
                        if (shannonFano.EncodingDictionary.Count == 0)
                        {
                            Console.WriteLine("Сначала закодируйте строку!");
                            break;
                        }
                        foreach (var pair in shannonFano.EncodingDictionary)
                        {
                            Console.WriteLine($"[{pair.Key}] - {pair.Value}");
                        }
                        break;
                    case 4:
                        if (shannonFano.EncodingDictionary.Count == 0)
                        {
                            Console.WriteLine("Сначала закодируйте строку!");
                            break;
                        }
                        Console.WriteLine($"Степень сжатия текста: {shannonFano.CompressionRatio}");
                        break;
                    default:
                        Console.WriteLine("Пожалуйста, выберите корректный пункт меню!");
                        break;
                }
                Console.ReadKey();
                menuItem = ShowMenu();
            }
        }
        static int ShowMenu()
        {
            string[] menu = { "1. Закодировать строку",
            "2. Декодировать строку",
            "3. Показать пары символ-ключ",
            "4. Показать степень сжатия",
            "5. Выход"
            };
            int currentMenuItem = 0;
            ConsoleKeyInfo cki;
            do
            {
                Console.Clear();
                for (int i = 0; i < menu.Length; i++)
                {
                    if (currentMenuItem == i) Console.ForegroundColor = ConsoleColor.DarkRed;
                    else Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine(menu[i]);
                }
                cki = Console.ReadKey();
                if (cki.Key == ConsoleKey.UpArrow)
                {
                    currentMenuItem--;
                    if (currentMenuItem < 0) currentMenuItem = menu.Length - 1;
                }
                else if (cki.Key == ConsoleKey.DownArrow)
                {
                    currentMenuItem++;
                    if (currentMenuItem > menu.Length - 1) currentMenuItem = 0;
                }
                else if (cki.Key == ConsoleKey.Enter)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    return currentMenuItem + 1;
                }
            }
            while (true);
        }
        /// <summary>
        /// Проверяет валидность пути к входному файлу
        /// </summary>
        /// <param name="path">Путь, который необходимо проверить</param>
        /// <returns>Результат проверки на валидность</returns>
        private static bool IsValidInputPath(string path)
        {
            // Путь не должен содержать недопустимые символы, должен быть абсолютьным и по данному пути должен существовать файл
            return path.IndexOfAny(Path.GetInvalidPathChars()) == -1 &&
                   Path.IsPathRooted(path) &&
                   File.Exists(path);
        }
    }
}
