using NUnit.Framework;
using System.Diagnostics;

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
            if (!reader.ReadScript(scriptPath))
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestReadFile()
        {
            Assert.IsTrue(reader.ReadScript(scriptPath));
        }
        [Test]
        public void TestReadDialogs()
        {
            Assert.IsTrue(reader.DialogsCount == 3);
        }
        [Test]
        public void TestRoutesCount()
        {
            Assert.IsTrue(reader.GetDialogByName("defaultDialog").RoutesCount == 1);
        }
        //Для работы Debug в Visual Studio
        [OneTimeSetUp]
        public void StartTest()
        {
            Trace.Listeners.Add(new ConsoleTraceListener());
        }
        [OneTimeTearDown]
        public void EndTest()
        {
            Trace.Flush();
        }
    }
}