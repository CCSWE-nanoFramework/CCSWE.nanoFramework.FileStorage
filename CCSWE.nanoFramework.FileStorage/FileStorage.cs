using System;
using System.IO;
using System.Text;

namespace CCSWE.nanoFramework.FileStorage
{
    /// <summary>
    /// Provides access to a file system.
    /// </summary>
    internal class FileStorage: IFileStorage
    {
        private const int ChunkSize = 8192;
        internal static readonly byte[] EmptyBytes = new byte[0];

        /// <inheritdoc />
        public bool FileExists(string path) => File.Exists(path);

        /// <inheritdoc />
        public string[] GetDirectories(string path) => Directory.GetDirectories(path);

        /// <inheritdoc />
        public string[] GetFiles(string path) => Directory.GetFiles(path);

        /// <inheritdoc />
        public FileStream OpenRead(string path) => new(path, FileMode.Open, FileAccess.Read);

        /// <inheritdoc />
        public StreamReader OpenText(string path) => new(new FileStream(path, FileMode.Open, FileAccess.Read));

        /// <inheritdoc />
        public byte[] ReadAllBytes(string path)
        {
            using var stream = OpenRead(path);

            var index = 0;
            var count = (int)stream.Length;
            var bytes = new byte[count];
 
            while (count > 0)
            {
                var read = stream.Read(bytes, index, count > ChunkSize ? ChunkSize : count);
                if (read == 0)
                {
                    throw new Exception("Unexpected end of file");
                }

                index += read;
                count -= read;
            }

            return bytes;
        }

        /// <inheritdoc />
        public string ReadAllText(string path)
        {
            using var streamReader = OpenText(path);
            return streamReader.ReadToEnd();
        }

        /// <inheritdoc />
        public void WriteAllBytes(string path, byte[] bytes)
        {
            Ensure.IsNotNullOrEmpty(nameof(path), path);
            Ensure.IsNotNull(nameof(bytes), bytes);

            File.Create(path);

            if (bytes.Length <= 0)
            {
                return;
            }

            using var stream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite);
            for (var bytesWritten = 0L; bytesWritten < bytes.Length;)
            {
                var bytesToWrite = bytes.Length - bytesWritten;
                bytesToWrite = bytesToWrite < ChunkSize ? bytesToWrite : ChunkSize;

                stream.Write(bytes, (int)bytesWritten, (int)bytesToWrite);
                stream.Flush();

                bytesWritten += bytesToWrite;
            }
        }

        /// <inheritdoc />
        public void WriteAllText(string path, string? contents) => WriteAllBytes(path, string.IsNullOrEmpty(contents) ? EmptyBytes : Encoding.UTF8.GetBytes(contents));
    }
}
