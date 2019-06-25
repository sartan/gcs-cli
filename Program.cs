using System;
using System.IO;
using System.Text.RegularExpressions;

namespace DirectoryTraversal
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Environment.Exit(-1);
            }

            var startPath = Path.GetFullPath(NormalizeUserHomePath(args[0]));

            if (!Directory.Exists(startPath))
            {
                return;
            }

            TraverseDirectory(startPath, GoatifyPath);
        }

        private static void TraverseDirectory(string path, Action<string> action)
        {
            action.Invoke(path);

            foreach (var file in Directory.GetFiles(path))
            {
                action.Invoke(file);
            }

            foreach (var dir in Directory.GetDirectories(path))
            {
                TraverseDirectory(dir, action);
            }
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