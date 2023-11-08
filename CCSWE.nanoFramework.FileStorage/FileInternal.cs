using System;
using System.IO;
using System.Text;

namespace CCSWE.nanoFramework.FileStorage
{
    internal static class FileInternal
    {
        private const int ChunkSize = 2048;// 8192;
        internal static readonly byte[] EmptyBytes = new byte[0];

        /// <summary>
        /// Determines whether the specified file exists.
        /// </summary>
        /// <param name="path">The file to check.</param>
        /// <returns><c>true</c> if the file exists; otherwise <c>false</c>.</returns>
        public static bool Exists(string path) => File.Exists(path);

        /// <summary>
        /// Opens an existing file for reading.
        /// </summary>
        /// <param name="path">The file to be opened for reading.</param>
        /// <returns>A <see cref="FileStream"/> on the specified path.</returns>
        public static FileStream OpenRead(string path) => new(path, FileMode.Open, FileAccess.Read);

        /// <summary>
        /// Opens an existing UTF-8 encoded text file for reading.
        /// </summary>
        /// <param name="path">The file to be opened for reading.</param>
        /// <returns>A <see cref="StreamReader"/> on the specified path.</returns>
        public static StreamReader OpenText(string path) => new(new FileStream(path, FileMode.Open, FileAccess.Read));

        /// <summary>
        /// Opens a binary file, reads the contents of the file into a byte array, and then closes the file.
        /// </summary>
        /// <param name="path">The file to open for reading.</param>
        public static byte[] ReadAllBytes(string path)
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

        /// <summary>
        /// Opens a text file, reads all the text in the file, and then closes the file.
        /// </summary>
        /// <param name="path">The file to open for reading.</param>
        public static string ReadAllText(string path)
        {
            using var streamReader = OpenText(path);
            return streamReader.ReadToEnd();
        }

        /// <summary>
        /// Creates a new file, writes the specified byte array to the file, and then closes the file. If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="path">The file to write to.</param>
        /// <param name="bytes">The bytes to write to the file.</param>
        public static void WriteAllBytes(string path, byte[] bytes)
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

        /// <summary>
        /// Creates a new file, writes the specified string to the file, and then closes the file. If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="path">The file to write to.</param>
        /// <param name="contents">The string to write to the file.</param>
        public static void WriteAllText(string path, string? contents) => WriteAllBytes(path, string.IsNullOrEmpty(contents) ? EmptyBytes : Encoding.UTF8.GetBytes(contents));
    }
}
