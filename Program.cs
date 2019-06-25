using System;
using System.IO.Abstractions;
using System.Text.RegularExpressions;

namespace DirectoryTraversal
{
    class Program
    {
        static void Main(string[] args)
        {
            var fs = new FileSystem();

            if (args.Length != 1)
            {
                Environment.Exit(-1);
            }

            var startPath = fs.Path.GetFullPath(NormalizeUserHomePath(args[0]));

            if (!fs.Directory.Exists(startPath))
            {
                return;
            }

            fs.Directory.PerformAction(startPath, GoatifyPath);
        }


        private static string NormalizeUserHomePath(string path)
        {
            return Regex.Replace(path, "^~", Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
        }

        private static void GoatifyPath(string str)
        {
            Console.WriteLine(str.Replace("/", "🐐/"));
        }
    }
}