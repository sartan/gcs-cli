using System;
using System.IO.Abstractions;

namespace DirectoryTraversal
{
    public static class DirectoryExtensions
    {
        public static void PerformAction(this IDirectory directoryObject, string path, Action<string> action)
        {
            action.Invoke(path);

            foreach (var file in directoryObject.GetFiles(path))
            {
                action.Invoke(file);
            }

            foreach (var dir in directoryObject.GetDirectories(path))
            {
                PerformAction(directoryObject, dir, action);
            }
        }
    }
}