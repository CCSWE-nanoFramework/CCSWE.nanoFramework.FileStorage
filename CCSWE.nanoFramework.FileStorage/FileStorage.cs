using System.IO;

namespace CCSWE.nanoFramework.FileStorage
{
    /// <summary>
    /// Provides access to a file system.
    /// </summary>
    internal class FileStorage: IFileStorage
    {
        /// <inheritdoc />
        public bool Exists(string path) => File.Exists(path);

        /// <inheritdoc />
        public string[] GetDirectories(string path) => Directory.GetDirectories(path);

        /// <inheritdoc />
        public string[] GetFiles(string path) => Directory.GetFiles(path);

        /// <inheritdoc />
        public FileStream OpenRead(string path) => FileInternal.OpenRead(path);

        /// <inheritdoc />
        public StreamReader OpenText(string path) => FileInternal.OpenText(path);

        /// <inheritdoc />
        public FileStream OpenWrite(string path) => FileInternal.OpenWrite(path);

        /// <inheritdoc />
        public byte[] ReadAllBytes(string path) => FileInternal.ReadAllBytes(path);

        /// <inheritdoc />
        public string ReadAllText(string path) => FileInternal.ReadAllText(path);

        /// <inheritdoc />
        public void WriteAllBytes(string path, byte[] bytes) => FileInternal.WriteAllBytes(path, bytes);

        /// <inheritdoc />
        public void WriteAllText(string path, string? contents) => FileInternal.WriteAllText(path, contents);
    }
}
