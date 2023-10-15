using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace DokiNameNormalizer
{
    internal class Program
    {
        public static void Main()
        {
            Console.WriteLine("Write path to the directory with theme definitions:");
            string path = Console.ReadLine();

            if (File.Exists(path)) throw new InvalidDataException("You should set directory path, not file path");

            string[] files = Directory.GetFiles(path, "*.master.definition.json", SearchOption.TopDirectoryOnly);

            foreach (string file in files)
            {
                string name = Path.GetFileName(file);

                string newName = char.ToUpper(name[0]) + name.Substring(1).Replace(".master.definition.json", ".json");

                Regex regex = new Regex("(?!\\.\\w*$)\\.");

                MatchCollection matches = regex.Matches(newName);

                foreach (Match match in matches)
                {
                    StringBuilder builder = new StringBuilder(newName);

                    builder[match.Index] = ' ';
                    builder[match.Index + 1] = char.ToUpper(newName[match.Index + 1]);
                    newName = builder.ToString();
                }

                newName = Path.Combine(path, newName);

                Console.WriteLine(newName);
                File.Move(file, newName);
            }
        }
    }
}