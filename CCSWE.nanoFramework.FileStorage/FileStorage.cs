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

        /// <inheritdoc />
        public bool FileExists(string path) => File.Exists(path);

        /// <inheritdoc />
        public string[] GetDirectories(string path) => Directory.GetDirectories(path);

        /// <inheritdoc />
        public string[] GetFiles(string path) => Directory.GetFiles(path);

        /// <inheritdoc />
        public StreamReader OpenText(string path) => new(new FileStream(path, FileMode.Open, FileAccess.Read));

        /// <inheritdoc />
        public string ReadAllText(string path)
        {
            using var streamReader = OpenText(path);
            return streamReader.ReadToEnd();
        }

        /// <inheritdoc />
        public void WriteAllText(string path, string? contents)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            File.Create(path);

            if (string.IsNullOrEmpty(contents))
            {
                return;
            }

            var buffer = Encoding.UTF8.GetBytes(contents);

            // TODO: Write in chunks?
            using var stream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite);
            stream.Write(buffer, 0, buffer.Length);
            stream.Close();
        }
    }
}
