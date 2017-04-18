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
    public class TribeFileParserTests
    {
        private string file = "";

        [TestInitialize()]
        public void TestInitialize()
        {
            file = Path.GetTempFileName();
            File.WriteAllBytes(file, ArkDataTests.Properties.Resources._1357824656);
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
            var parser = new TribeFileParser();
            ITribe tribe = parser.Parse(file);
            Assert.IsInstanceOfType(tribe, typeof(Tribe));
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void ParseEmptyPathTest()
        {
            var parser = new TribeFileParser();
            parser.Parse("");
        }

        [TestMethod()]
        [ExpectedException(typeof(FileNotFoundException))]
        public void ParseNonExistingFileTest()
        {
            var parser = new TribeFileParser();
            parser.Parse("C:\\NonExistingFile");
        }

        [TestMethod()]
        public async Task ParserAsyncTest()
        {
            var parser = new TribeFileParser();
            ITribe tribe = await parser.ParseAsync(file);
            Assert.IsInstanceOfType(tribe, typeof(Tribe));
        }
    }
}