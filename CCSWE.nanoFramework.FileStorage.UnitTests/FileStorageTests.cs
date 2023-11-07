using System;
using System.IO;
using System.Text;
using nanoFramework.TestFramework;

namespace CCSWE.nanoFramework.FileStorage.UnitTests
{
    [TestClass]
    public class FileStorageTests
    {
        private const string TestDrive = @"I:\";
        private static readonly string TestFile = $@"{TestDrive}{nameof(FileStorageTests)}.test";

        private static readonly byte[] BinaryContent = Encoding.UTF8.GetBytes(TextContent);
        private const string TextContent = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.";

        #region Test helper methods
        private static void AssertBinaryContentEquals(byte[] expected)
        {
            AssertFileExists();
            using var stream = new FileStream(TestFile, FileMode.Open);
            AssertBinaryContentEquals(expected, stream);
        }

        private static void AssertBinaryContentEquals(byte[] expected, Stream stream)
        {
            Assert.IsNotNull(expected);
            Assert.IsNotNull(stream);
            Assert.AreEqual(expected!.Length, stream.Length);

            var content = new byte[stream.Length];
            stream.Read(content, 0, content.Length);

            for (var i = 0; i < content.Length; i++)
            {
                Assert.AreEqual(expected[i], content[i]);
            }
        }

        private static void AssertFileDoesNotExist()
        {
            Assert.IsFalse(File.Exists(TestFile), $"{TestFile} exists when it shouldn't.");
        }

        private static void AssertFileExists()
        {
            Assert.IsTrue(File.Exists(TestFile), $"{TestFile} does not exist when it should.");
        }

        private static void AssertTextContentEquals(string expected)
        {
            AssertFileExists();
            using var streamReader = new StreamReader(new FileStream(TestFile, FileMode.Open));
            AssertTextContentEquals(expected, streamReader);
        }

        private static void AssertTextContentEquals(string expected, TextReader streamReader)
        {
            Assert.IsNotNull(streamReader);
            Assert.AreEqual(expected, streamReader.ReadToEnd(), $"{TestFile} does not contain expected content.");
        }

        /// <summary>
        /// Creates the test file (<see cref="TestFile"/>) with the content contained in <see cref="BinaryContent"/>.
        /// </summary>
        /// <remarks>The content is confirmed to ensure a known state for a unit test.</remarks>
        private static void CreateBinaryFile()
        {
            using (var streamWriter = File.Create(TestFile))
            {
                streamWriter.Write(BinaryContent, 0, BinaryContent.Length);
                streamWriter.Close();
            }

            AssertBinaryContentEquals(BinaryContent);
        }

        /// <summary>
        /// Creates the test file (<see cref="TestFile"/>) with the content contained in <see cref="TextContent"/>.
        /// </summary>
        /// <remarks>The content is confirmed to ensure a known state for a unit test.</remarks>
        private static void CreateTextFile()
        {
            using (var streamWriter = new StreamWriter(File.Create(TestFile)))
            {
                streamWriter.Write(TextContent);
                streamWriter.Close();
            }

            AssertTextContentEquals(TextContent);
        }

        /// <summary>
        /// Delete the test file (<see cref="TestFile"/>)
        /// </summary>
        /// <remarks>Deletion is confirmed to ensure a known state for a unit test.</remarks>
        private static void DeleteTestFile()
        {
            if (File.Exists(TestFile))
            {
                File.Delete(TestFile);
            }

            AssertFileDoesNotExist();
        }

        /// <summary>
        /// Deletes test file (<see cref="TestFile"/>), executes the <paramref name="action"/>, and then deletes the test file.
        /// </summary>
        /// <param name="action"></param>
        private static void ExecuteFileTest(Action action)
        {
            try
            {
                DeleteTestFile();

                action();
            }
            finally
            {
                DeleteTestFile();
            }
        }
        #endregion

        [TestMethod]
        public void FileExists_should_return_false_if_file_does_not_exists()
        {
            ExecuteFileTest(() =>
            {
                // Arrange
                var sut = new FileStorage();

                // Act
                var actual = sut.FileExists(TestFile);

                // Assert
                Assert.IsFalse(actual);
            });
        }

        [TestMethod]
        public void FileExists_should_return_true_if_file_exists()
        {
            ExecuteFileTest(() =>
            {
                // Arrange
                CreateTextFile();

                var sut = new FileStorage();

                // Act
                var actual = sut.FileExists(TestFile);

                // Assert
                Assert.IsTrue(actual);
            });
        }

        [TestMethod]
        public void GetDirectories_should_return_directories()
        {
            // Arrange
            var sut = new FileStorage();

            // Act
            var actual = sut.GetDirectories(TestDrive);

            // Assert
            Assert.IsNotNull(actual);
        }

        [TestMethod]
        public void GetFiles_should_return_files()
        {
            ExecuteFileTest(() =>
            {
                // Arrange
                CreateTextFile();

                var sut = new FileStorage();

                // Act
                var actual = sut.GetFiles(TestDrive);

                // Assert
                Assert.IsNotNull(actual);
                Assert.IsTrue(actual.Length > 0);
            });
        }

        [TestMethod]
        public void OpenRead_should_open_existing_file()
        {
            ExecuteFileTest(() =>
            {
                // Arrange
                CreateBinaryFile();

                var sut = new FileStorage();

                // Act
                using var actual = sut.OpenRead(TestFile);

                // Assert
                AssertBinaryContentEquals(BinaryContent, actual);
            });
        }

        [TestMethod]
        public void OpenRead_should_throw_if_file_does_not_exist()
        {
            ExecuteFileTest(() =>
            {
                // Arrange
                var sut = new FileStorage();

                // Act
                Assert.ThrowsException(typeof(IOException), () => { sut.OpenRead(TestFile); });
            });
        }

        [TestMethod]
        public void OpenText_should_open_existing_file()
        {
            ExecuteFileTest(() =>
            {
                // Arrange
                CreateTextFile();

                var sut = new FileStorage();

                // Act
                using var actual = sut.OpenText(TestFile);

                // Assert
                AssertTextContentEquals(TextContent, actual);
            });
        }

        [TestMethod]
        public void OpenText_should_throw_if_file_does_not_exist()
        {
            ExecuteFileTest(() =>
            {
                // Arrange
                var sut = new FileStorage();

                // Act
                Assert.ThrowsException(typeof(IOException), () => { sut.OpenText(TestFile); });
            });
        }

        [TestMethod]
        public void ReadAllBytes_should_read_all_content_from_file()
        {
            // TODO Implement
            ExecuteFileTest(() =>
            {
                // Arrange
                CreateBinaryFile();

                var sut = new FileStorage();

                // Act
                var actual = sut.ReadAllBytes(TestFile);

                // Assert
                AssertBinaryContentEquals(actual);
            });
        }

        [TestMethod]
        public void ReadAllBytes_should_throw_if_file_does_not_exist()
        {
            ExecuteFileTest(() =>
            {
                // Arrange
                var sut = new FileStorage();

                // Act
                Assert.ThrowsException(typeof(IOException), () => { sut.ReadAllBytes(TestFile); });
            });
        }

        [TestMethod]
        public void ReadAllText_should_read_all_content_from_file()
        {
            ExecuteFileTest(() =>
            {
                // Arrange
                CreateTextFile();

                var sut = new FileStorage();

                // Act
                var actual = sut.ReadAllText(TestFile);

                // Assert
                Assert.AreEqual(TextContent, actual);
            });
        }

        [TestMethod]
        public void ReadAllText_should_throw_if_file_does_not_exist()
        {
            ExecuteFileTest(() =>
            {
                // Arrange
                var sut = new FileStorage();

                // Act
                Assert.ThrowsException(typeof(IOException), () => { sut.ReadAllText(TestFile); });
            });
        }

        [TestMethod]
        public void WriteAllBytes_should_create_file()
        {
            ExecuteFileTest(() =>
            {
                // Arrange
                var sut = new FileStorage();

                // Act
                sut.WriteAllBytes(TestFile, FileStorage.EmptyBytes);

                // Assert
                AssertFileExists();
            });
        }

        [TestMethod]
        public void WriteAllBytes_should_overwrite_existing_file()
        {
            ExecuteFileTest(() =>
            {
                // Arrange
                CreateTextFile();

                var content = nameof(FileStorageTests);
                var bytes = Encoding.UTF8.GetBytes(content);
                var sut = new FileStorage();

                // Act
                sut.WriteAllBytes(TestFile, bytes);

                // Assert
                AssertBinaryContentEquals(bytes);
            });
        }

        [TestMethod]
        public void WriteAllText_should_create_file()
        {
            ExecuteFileTest(() =>
            {
                // Arrange
                var content = nameof(FileStorageTests);
                var sut = new FileStorage();

                // Act
                sut.WriteAllText(TestFile, content);

                // Assert
                AssertFileExists();
            });
        }

        [TestMethod]
        public void WriteAllText_should_overwrite_existing_file()
        {
            ExecuteFileTest(() =>
            {
                // Arrange
                CreateTextFile();

                var content = nameof(FileStorageTests);
                var sut = new FileStorage();

                // Act
                sut.WriteAllText(TestFile, content);

                // Assert
                AssertTextContentEquals(content);
            });
        }
    }
}
