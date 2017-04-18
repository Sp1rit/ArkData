using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArkData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ArkData.Tests
{
    [TestClass()]
    public class PlayerFileParserTests
    {
        private string file = "";

        [TestInitialize()]
        public void TestInitialize()
        {
            file = Path.GetTempFileName();
            File.WriteAllBytes(file, ArkDataTests.Properties.Resources._76561197992236997);
        }

        [TestCleanup()]
        public void TestCleanup()
        {
            if (!string.IsNullOrEmpty(file) && File.Exists(file))
                File.Delete(file);
        }

        [TestMethod()]
        public void ParseTest()
        {
            var parser = new PlayerFileParser();
            IPlayer player = parser.Parse(file);
            Assert.IsInstanceOfType(player, typeof(Player));
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void ParseEmptyPathTest()
        {
            var parser = new PlayerFileParser();
            parser.Parse("");
        }

        [TestMethod()]
        [ExpectedException(typeof(FileNotFoundException))]
        public void ParseNonExistingFileTest()
        {
            var parser = new PlayerFileParser();
            parser.Parse("C:\\NonExistingFile");
        }

        [TestMethod()]
        public async Task ParserAsyncTest()
        {
            var parser = new PlayerFileParser();
            IPlayer player = await parser.ParseAsync(file);
            Assert.IsInstanceOfType(player, typeof(Player));
        }
    }
}