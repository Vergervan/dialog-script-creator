using NUnit.Framework;
using System.IO;

namespace DialogScriptCreator.Tests
{
    [TestFixture]
    public class DialogScriptReaderTest
    {
        private DialogScriptReader reader;
        private const string scriptPath = "C:\\Users\\Vergervan\\Desktop\\GitRepos\\dialog-script-creator\\Tests\\DialogScriptCreator.Tests\\test.ds";
        [SetUp]
        public void Setup()
        {
            reader = new DialogScriptReader();
        }

        [Test]
        public void TestReadFile()
        {
            Assert.IsTrue(reader.ReadScript(scriptPath));
        }
        [Test]
        public void TestReadStringsCount()
        {
            if (!reader.ReadScript(scriptPath))
            {
                Assert.Fail();
            }
            Assert.IsTrue(reader.ScriptStringLength == 4);
        }
        [Test]
        public void TestReadDialogs()
        {
            if (!reader.ReadScript(scriptPath))
            {
                Assert.Fail();
            }
            Assert.IsTrue(reader.DialogsCount == 3);
        }
    }
}