using NUnit.Framework;
using System.Diagnostics;
using System.Linq;

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
        public void TestTriggers()
        {
            Route route = reader.GetDialogByName("defaultDialog").Routes.ElementAt(1);
            Assert.IsTrue(route.HasTriggers);
        }
        [Test]
        public void TestDialogRoutes()
        {
            Assert.IsTrue(reader.GetDialogByName("smokeCakeDialog").RoutesCount == 1);
        }

        [Test]
        public void TestReadFile()
        {
            Assert.IsTrue(reader.ReadScript(scriptPath));
        }
        [Test]
        public void TestReadDialogs()
        {
            Trace.WriteLine(reader.DialogsCount);
            Assert.IsTrue(reader.DialogsCount == 6);
        }
        [Test]
        public void TestRoutesCount()
        {
            Assert.IsTrue(reader.GetDialogByName("defaultDialog").RoutesCount == 1);
        }
        [Test]
        public void ShowAllDialogs()
        {
            foreach(var item in reader.GetDialogs())
            {
                Trace.WriteLine($"{item.Name} = {item.Value}");
            }
            Assert.Pass();
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