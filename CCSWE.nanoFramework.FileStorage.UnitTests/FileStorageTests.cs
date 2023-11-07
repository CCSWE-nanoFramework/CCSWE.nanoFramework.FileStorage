using System;
using System.IO;
using nanoFramework.TestFramework;

namespace CCSWE.nanoFramework.FileStorage.UnitTests
{
    [TestClass]
    public class FileStorageTests
    {
        private const string TestContent = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.";
        private static readonly int TestContentLength = TestContent.Length;
        private const string TestDrive = @"I:\";
        private static readonly string TestFile = $@"{TestDrive}{nameof(FileStorageTests)}.test";

        /// <summary>
        /// Creates the test file (<see cref="TestFile"/>) with the content contained in <see cref="TestContent"/>.
        /// </summary>
        /// <remarks>The content is confirmed to ensure a known state for a unit test.</remarks>
        private static void CreateTestFile()
        {
            DeleteTestFile();

            using (var streamWriter = new StreamWriter(File.Create(TestFile)))
            {
                streamWriter.Write(TestContent);
                streamWriter.Close();
            }

            Assert.IsTrue(File.Exists(TestFile), $"{TestFile} does not exist.");

            using var streamReader = new StreamReader(new FileStream(TestFile, FileMode.Open));
            Assert.AreEqual(TestContent, streamReader.ReadToEnd(), $"{TestFile} does not contain expected content.");
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

            Assert.IsFalse(File.Exists(TestFile), $"{TestFile} still exists.");
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
                CreateTestFile();

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
                CreateTestFile();

                var sut = new FileStorage();

                // Act
                var actual = sut.GetFiles(TestDrive);

                // Assert
                Assert.IsNotNull(actual);
                Assert.IsTrue(actual.Length > 0);
            });
        }

        [TestMethod]
        public void OpenText_should_open_existing_file()
        {
            ExecuteFileTest(() =>
            {
                // Arrange
                CreateTestFile();

                var sut = new FileStorage();

                // Act
                using var actual = sut.OpenText(TestFile);

                // Assert
                Assert.IsNotNull(actual);
                Assert.AreEqual(TestContentLength, actual.BaseStream.Length);
                Assert.AreEqual(TestContent, actual.ReadToEnd());
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
        public void ReadAllText_should_read_all_content_from_file()
        {
            ExecuteFileTest(() =>
            {
                // Arrange
                CreateTestFile();

                var sut = new FileStorage();

                // Act
                var actual = sut.ReadAllText(TestFile);

                // Assert
                Assert.AreEqual(TestContent, actual);
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
                Assert.IsTrue(File.Exists(TestFile));
            });
        }

        [TestMethod]
        public void WriteAllText_should_overwrite_existing_file()
        {
            ExecuteFileTest(() =>
            {
                // Arrange
                CreateTestFile();

                var content = nameof(FileStorageTests);
                var sut = new FileStorage();

                // Act
                sut.WriteAllText(TestFile, content);

                // Assert
                Assert.IsTrue(File.Exists(TestFile));

                using var streamReader = new StreamReader(new FileStream(TestFile, FileMode.Open));
                Assert.AreEqual(content, streamReader.ReadToEnd(), $"{TestFile} does not contain expected content.");
            });
        }
    }
}
