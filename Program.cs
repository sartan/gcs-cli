using System;
using System.IO.Abstractions;
using System.Text.RegularExpressions;
using Google.Cloud.Storage.V1;

namespace DirectoryTraversal
{
    class Program
    {
        static void Main(string[] args)
        {
            var fs = new FileSystem();
            var storageClient = StorageClient.Create();
            var uploader = new GcsUploader(fs, storageClient, "interviewprep");

            if (args.Length != 1)
            {
                Environment.Exit(-1);
            }

            var startPath = NormalizeUserHomePath(args[0]);

            if (!fs.Directory.Exists(startPath))
            {
                return;
            }

            fs.Directory.DoRecursively(startPath, uploader.Upload);
        }


        private static string NormalizeUserHomePath(string path)
        {
            return Regex.Replace(path, "^~", Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
        }
    }
}