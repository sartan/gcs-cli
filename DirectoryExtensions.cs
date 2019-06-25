using System;
using System.IO.Abstractions;

namespace DirectoryTraversal
{
    public static class DirectoryExtensions
    {
        private static void DoRecursivelyForFile(this IDirectory directoryObject, string path, Action<string> action)
        {
            foreach (var file in directoryObject.GetFiles(path))
            {
                action.Invoke(file);
            }

            foreach (var dir in directoryObject.GetDirectories(path))
            {
                DoRecursivelyForFile(directoryObject, dir, action);
            }
        }

        private static void DoRecursivelyForDirectory(this IDirectory directoryObject, string path, Action<string> action)
        {
            action.Invoke(path);

            foreach (var dir in directoryObject.GetDirectories(path))
            {
                DoRecursivelyForDirectory(directoryObject, dir, action);
            }
        }

        public static void DoRecursively(this IDirectory directoryObject, string path, Action<string> action, TraversalMode mode = TraversalMode.DirectoryFile)
        {
            if (mode == TraversalMode.File || mode == TraversalMode.DirectoryFile)
            {
                DoRecursivelyForFile(directoryObject, path, action);
            }

            if (mode == TraversalMode.Directory || mode == TraversalMode.DirectoryFile)
            {
                DoRecursivelyForDirectory(directoryObject, path, action);
            }
        }
    }
}