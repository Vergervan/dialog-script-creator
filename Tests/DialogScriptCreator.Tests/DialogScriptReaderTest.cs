using NUnit.Framework;

namespace DialogScriptCreator.Tests
{
    [TestFixture]
    public class DialogScriptReaderTest
    {
        private DialogScriptReader reader;
        [SetUp]
        public void Setup()
        {
            reader = new DialogScriptReader();
        }

        [Test]
        public void TestReadFile()
        {
            Assert.IsTrue(reader.ReadScript("test.ds"));
        }
        [Test]
        public void TestReadStringsCount()
        {
            if (!reader.ReadScript("test.ds"))
            {
                Assert.Fail();
            }
            Assert.IsTrue(reader.ScriptStringLength == 6);
        }
    }
}