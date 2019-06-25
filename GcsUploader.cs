using System;
using System.IO;
using System.IO.Abstractions;
using Google.Cloud.Storage.V1;

namespace DirectoryTraversal
{
    public class GcsUploader
    {
        private readonly StorageClient _gcsClient;
        private readonly IFileSystem _fs;
        private readonly string _bucket;

        public GcsUploader(IFileSystem fs, StorageClient gcsClient, string bucket)
        {
            _gcsClient = gcsClient;
            _fs = fs;
            _bucket = bucket;
        }


        public void Upload(string path)
        {
            if (_fs.Directory.Exists(path))
            {
                UploadDirectory(path);
            }
            else if (_fs.File.Exists(path))
            {
                UploadFile(path);
            }
        }

        private void UploadFile(string filePath)
        {
            using (var f = _fs.File.OpenRead(filePath))
            {
                DoUpload(filePath, f);
            }
        }

        private void UploadDirectory(string dirPath)
        {
            if (!_fs.Directory.Exists(dirPath))
            {
                return;
            }

            if (!dirPath.EndsWith("/"))
            {
                dirPath = dirPath + "/";
            }

            DoUpload(dirPath, Stream.Null);
        }

        private void DoUpload(string path, Stream stream)
        {
            // System.IO.Abstractions doesn't yet support GetRelativePath()
            // https://github.com/System-IO-Abstractions/System.IO.Abstractions/issues/399
            var normalizedRelativePath = Path.GetRelativePath(".", path);

            Console.WriteLine($"Uploading {normalizedRelativePath}");
            _gcsClient.UploadObject(_bucket, normalizedRelativePath, null, stream);
        }
    }
}