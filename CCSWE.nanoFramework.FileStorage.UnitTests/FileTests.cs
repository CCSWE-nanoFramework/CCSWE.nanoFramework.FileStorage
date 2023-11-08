using nanoFramework.TestFramework;
using System;
using System.IO;
using System.Text;

namespace CCSWE.nanoFramework.FileStorage.UnitTests
{
    public abstract class FileTests
    {
        protected const string TestDrive = @"I:\";
        protected static readonly string TestFile = $@"{TestDrive}{nameof(FileStorageTests)}.test";

        protected static readonly byte[] BinaryContent = Encoding.UTF8.GetBytes(TextContent);
        protected const string TextContent = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.";

        #region Test helper methods
        protected static void AssertBinaryContentEquals(byte[] expected)
        {
            AssertFileExists();
            using var stream = new FileStream(TestFile, FileMode.Open);
            AssertBinaryContentEquals(expected, stream);
        }

        protected static void AssertBinaryContentEquals(byte[] expected, Stream stream)
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

        protected static void AssertFileDoesNotExist()
        {
            Assert.IsFalse(File.Exists(TestFile), $"{TestFile} exists when it shouldn't.");
        }

        protected static void AssertFileExists()
        {
            Assert.IsTrue(File.Exists(TestFile), $"{TestFile} does not exist when it should.");
        }

        protected static void AssertTextContentEquals(string expected)
        {
            AssertFileExists();
            using var streamReader = new StreamReader(new FileStream(TestFile, FileMode.Open));
            AssertTextContentEquals(expected, streamReader);
        }

        protected static void AssertTextContentEquals(string expected, TextReader streamReader)
        {
            Assert.IsNotNull(streamReader);
            Assert.AreEqual(expected, streamReader.ReadToEnd(), $"{TestFile} does not contain expected content.");
        }

        /// <summary>
        /// Creates the test file (<see cref="TestFile"/>) with the content contained in <see cref="BinaryContent"/>.
        /// </summary>
        /// <remarks>The content is confirmed to ensure a known state for a unit test.</remarks>
        protected static void CreateBinaryFile()
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
        protected static void CreateTextFile()
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
        protected static void DeleteTestFile()
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
        protected static void ExecuteFileTest(Action action)
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
    }
}
