using System;
using System.IO;
using System.Text;
using nanoFramework.TestFramework;

namespace CCSWE.nanoFramework.FileStorage.UnitTests
{
    [TestClass]
    public class FileStorageTests: FileTests
    {
        [TestMethod]
        public void Exists_should_return_false_if_file_does_not_exists()
        {
            ExecuteFileTest(() =>
            {
                var sut = new FileStorage();
                var actual = sut.Exists(TestFile);

                Assert.IsFalse(actual);
            });
        }

        [TestMethod]
        public void Exists_should_return_true_if_file_exists()
        {
            ExecuteFileTest(() =>
            {
                CreateTextFile();

                var sut = new FileStorage();
                var actual = sut.Exists(TestFile);

                Assert.IsTrue(actual);
            });
        }

        [TestMethod]
        public void GetDirectories_should_return_directories()
        {
            var sut = new FileStorage();
            var actual = sut.GetDirectories(TestDrive);

            Assert.IsNotNull(actual);
        }

        [TestMethod]
        public void GetFiles_should_return_files()
        {
            ExecuteFileTest(() =>
            {
                CreateTextFile();

                var sut = new FileStorage();
                var actual = sut.GetFiles(TestDrive);

                Assert.IsNotNull(actual);
                Assert.IsTrue(actual.Length > 0);
            });
        }

        [TestMethod]
        public void OpenRead_should_open_existing_file()
        {
            ExecuteFileTest(() =>
            {
                CreateBinaryFile();

                var sut = new FileStorage();
                using var actual = sut.OpenRead(TestFile);

                AssertBinaryContentEquals(BinaryContent, actual);
            });
        }

        [TestMethod]
        public void OpenRead_should_throw_if_file_does_not_exist()
        {
            ExecuteFileTest(() =>
            {
                var sut = new FileStorage();

                Assert.ThrowsException(typeof(IOException), () => { sut.OpenRead(TestFile); });
            });
        }

        [TestMethod]
        public void OpenText_should_open_existing_file()
        {
            ExecuteFileTest(() =>
            {
                CreateTextFile();

                var sut = new FileStorage();
                using var actual = sut.OpenText(TestFile);

                AssertTextContentEquals(TextContent, actual);
            });
        }

        [TestMethod]
        public void OpenText_should_throw_if_file_does_not_exist()
        {
            ExecuteFileTest(() =>
            {
                var sut = new FileStorage();

                Assert.ThrowsException(typeof(IOException), () => { sut.OpenText(TestFile); });
            });
        }

        [TestMethod]
        public void ReadAllBytes_should_read_all_content_from_file()
        {
            ExecuteFileTest(() =>
            {
                CreateBinaryFile();

                var sut = new FileStorage();
                var actual = sut.ReadAllBytes(TestFile);

                AssertBinaryContentEquals(actual);
            });
        }

        [TestMethod]
        public void ReadAllBytes_should_throw_if_file_does_not_exist()
        {
            ExecuteFileTest(() =>
            {
                var sut = new FileStorage();

                Assert.ThrowsException(typeof(IOException), () => { sut.ReadAllBytes(TestFile); });
            });
        }

        [TestMethod]
        public void ReadAllText_should_read_all_content_from_file()
        {
            ExecuteFileTest(() =>
            {
                CreateTextFile();

                var sut = new FileStorage();
                var actual = sut.ReadAllText(TestFile);

                Assert.AreEqual(TextContent, actual);
            });
        }

        [TestMethod]
        public void ReadAllText_should_throw_if_file_does_not_exist()
        {
            ExecuteFileTest(() =>
            {
                var sut = new FileStorage();

                Assert.ThrowsException(typeof(IOException), () => { sut.ReadAllText(TestFile); });
            });
        }

        [TestMethod]
        public void WriteAllBytes_should_create_file()
        {
            ExecuteFileTest(() =>
            {
                var sut = new FileStorage();
                sut.WriteAllBytes(TestFile, FileInternal.EmptyBytes);

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
                var sut = new FileStorage();

                sut.WriteAllBytes(TestFile, bytes);

                AssertBinaryContentEquals(bytes);
            });
        }

        [TestMethod]
        public void WriteAllText_should_create_file()
        {
            ExecuteFileTest(() =>
            {
                var content = nameof(FileStorageTests);
                var sut = new FileStorage();

                sut.WriteAllText(TestFile, content);

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
                var sut = new FileStorage();

                sut.WriteAllText(TestFile, content);

                AssertTextContentEquals(content);
            });
        }
    }
}
