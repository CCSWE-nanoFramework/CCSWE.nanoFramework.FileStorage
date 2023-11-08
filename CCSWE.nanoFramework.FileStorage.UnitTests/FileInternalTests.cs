using System.IO;
using System.Text;
using nanoFramework.TestFramework;

namespace CCSWE.nanoFramework.FileStorage.UnitTests
{
    [TestClass]
    public class FileInternalTests: FileTests
    {
        [TestMethod]
        public void FileExists_should_return_false_if_file_does_not_exists()
        {
            ExecuteFileTest(() =>
            {
                Assert.IsFalse(FileInternal.Exists(TestFile));
            });
        }

        [TestMethod]
        public void FileExists_should_return_true_if_file_exists()
        {
            ExecuteFileTest(() =>
            {
                CreateTextFile();

                Assert.IsTrue(FileInternal.Exists(TestFile));
            });
        }

        [TestMethod]
        public void OpenRead_should_open_existing_file()
        {
            ExecuteFileTest(() =>
            {
                CreateBinaryFile();

                using var actual = FileInternal.OpenRead(TestFile);

                AssertBinaryContentEquals(BinaryContent, actual);
            });
        }

        [TestMethod]
        public void OpenRead_should_throw_if_file_does_not_exist()
        {
            ExecuteFileTest(() =>
            {
                Assert.ThrowsException(typeof(IOException), () => { FileInternal.OpenRead(TestFile); });
            });
        }

        [TestMethod]
        public void OpenText_should_open_existing_file()
        {
            ExecuteFileTest(() =>
            {
                CreateTextFile();

                using var actual = FileInternal.OpenText(TestFile);

                AssertTextContentEquals(TextContent, actual);
            });
        }

        [TestMethod]
        public void OpenText_should_throw_if_file_does_not_exist()
        {
            ExecuteFileTest(() =>
            {
                Assert.ThrowsException(typeof(IOException), () => { FileInternal.OpenText(TestFile); });
            });
        }

        [TestMethod]
        public void ReadAllBytes_should_read_all_content_from_file()
        {
            // TODO Implement
            ExecuteFileTest(() =>
            {
                CreateBinaryFile();

                var actual = FileInternal.ReadAllBytes(TestFile);

                AssertBinaryContentEquals(actual);
            });
        }

        [TestMethod]
        public void ReadAllBytes_should_throw_if_file_does_not_exist()
        {
            ExecuteFileTest(() =>
            {
                Assert.ThrowsException(typeof(IOException), () => { FileInternal.ReadAllBytes(TestFile); });
            });
        }

        [TestMethod]
        public void ReadAllText_should_read_all_content_from_file()
        {
            ExecuteFileTest(() =>
            {
                CreateTextFile();

                var actual = FileInternal.ReadAllText(TestFile);

                Assert.AreEqual(TextContent, actual);
            });
        }

        [TestMethod]
        public void ReadAllText_should_throw_if_file_does_not_exist()
        {
            ExecuteFileTest(() =>
            {
                Assert.ThrowsException(typeof(IOException), () => { FileInternal.ReadAllText(TestFile); });
            });
        }

        [TestMethod]
        public void WriteAllBytes_should_create_file()
        {
            ExecuteFileTest(() =>
            {
                FileInternal.WriteAllBytes(TestFile, FileInternal.EmptyBytes);

                AssertFileExists();
            });
        }

        [TestMethod]
        public void WriteAllBytes_should_overwrite_existing_file()
        {
            ExecuteFileTest(() =>
            {
                CreateTextFile();

                var content = nameof(FileStorageTests);
                var bytes = Encoding.UTF8.GetBytes(content);

                FileInternal.WriteAllBytes(TestFile, bytes);

                AssertBinaryContentEquals(bytes);
            });
        }

        [TestMethod]
        public void WriteAllText_should_create_file()
        {
            ExecuteFileTest(() =>
            {
                var content = nameof(FileStorageTests);

                FileInternal.WriteAllText(TestFile, content);

                AssertFileExists();
            });
        }

        [TestMethod]
        public void WriteAllText_should_overwrite_existing_file()
        {
            ExecuteFileTest(() =>
            {
                CreateTextFile();

                var content = nameof(FileStorageTests);

                FileInternal.WriteAllText(TestFile, content);

                AssertTextContentEquals(content);
            });
        }

    }
}
