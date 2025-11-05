
using Examination;

namespace Tests
{
    public class Tests
    {
        private string testFilePath;
        [SetUp]
        public void Setup()
        {
            testFilePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestFiles");
            Directory.CreateDirectory(testFilePath);
        }

        [Test]
        public void Test1_ValidHTML()
        {
            string html = @"<html><head><title>Test Page</title></head><body><div>Content</div></body></html>";
            HTMLDocument doc = new HTMLDocument();

            doc.Load(html);

            Assert.IsNotNull(doc);
        }
    }
}
