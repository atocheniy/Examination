using Examination;
using System.Xml;
using NUnit.Framework;
using System.IO;

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

            Assert.That(doc.Root, Is.Not.Null);
            Assert.That(doc.Root.ChildNodes.Count, Is.EqualTo(1));
            Assert.That(doc.Root.ChildNodes[0].Name, Is.EqualTo("HTML"));
        }



        [Test]
        public void Test2_HTMLWithAttributes()
        {
            string html = @"<div id=""main"" class=""container"" data-test=""value"">Content</div>";
            HTMLDocument doc = new HTMLDocument();

            doc.Load(html);

            Assert.That(doc.HasError, Is.False);
            HTMLNode divNode = doc.Root.ChildNodes[0];
            Assert.That(divNode.Attributes.Count, Is.EqualTo(3));
            Assert.That(divNode.Attributes[0].Name, Is.EqualTo("id"));
            Assert.That(divNode.Attributes[0].Value, Is.EqualTo("main"));
        }



        [Test]
        public void Test3_TableSttucture()
        {
            string html = @"<table border=""1""><tr><th>Header</th></tr><tr><td>Data</td></tr></table>";
            HTMLDocument doc = new HTMLDocument();

            doc.Load(html);

            Assert.That(doc.HasError, Is.False);
            HTMLNode tableNode = doc.Root.ChildNodes[0];
            Assert.That(tableNode.ChildNodes.Count, Is.EqualTo(2));
            Assert.That(tableNode.Name, Is.EqualTo("table"));
            Assert.That(tableNode.ChildNodes[0].Name, Is.EqualTo("tr"));
        }



        [Test]
        public void Test4_ImageElement()
        {
            string html = @"<img src=""image.jpg"" alt=""Test Image"" width=""100"" height=""50"">";
            HTMLDocument doc = new HTMLDocument();

            doc.Load(html);

            Assert.That(doc.HasError, Is.False);
            HTMLNode imgNode = doc.Root.ChildNodes[0];
            Assert.That(imgNode.Attributes.Count, Is.EqualTo(4));
            Assert.That(imgNode.Name, Is.EqualTo("img"));
        }


        [Test]
        public void Test5_TextNodes()
        {
            string html = @"<div>Text before <span>inner</span> text after</div>";
            HTMLDocument doc = new HTMLDocument();

            doc.Load(html);

            Assert.That(doc.HasError, Is.False);
            HTMLNode divNode = doc.Root.ChildNodes[0];
            Assert.That(divNode.ChildNodes.Count, Is.EqualTo(3));
            Assert.That(divNode.ChildNodes[0].NodeType, Is.EqualTo(HTMLNodeType.Text));
            Assert.That(divNode.ChildNodes[0].Value.Trim(), Is.EqualTo("Text before"));
        }


        [Test]
        public void Test6_InvalidElement()
        {
            string html = @"<invalid>Content</invalid>";
            HTMLDocument doc = new HTMLDocument();

            doc.Load(html);

            Assert.That(doc.HasError, Is.True);
            Assert.That(doc.ErrorCode.Contains("invalid_element"), Is.True);
        }


        [Test]
        public void Test7_UnclosedElement()
        {
            string html = @"<div>Content";
            HTMLDocument doc = new HTMLDocument();

            doc.Load(html);

            Assert.That(doc.HasError, Is.True);
            Assert.That(doc.ErrorCode.Contains("unclosed_element"), Is.True);
        }


        [Test]
        public void Test8_ComplexDocument()
        {
            string html = @"
            <html>
                <head>
                    <title>Test Page</title>
                </head>
                <body>
                    <div id=""header"">
                        <h1>Header</h1>
                    </div>
                    <table class=""data-table"">
                        <tr>
                            <th>Name</th>
                            <th>Age</th>
                        </tr>
                        <tr>
                            <td>John</td>
                            <td>25</td>
                        </tr>
                    </table>
                    <img src=""photo.jpg"" alt=""Photo"">
                </body>
            </html>";

            HTMLDocument doc = new HTMLDocument();
            doc.Load(html);

            Assert.That(doc.HasError, Is.False);
            Assert.That(doc.Root.ChildNodes[0], Is.Not.Null);
            Assert.That(doc.Root.ChildNodes[0].Name, Is.Equals("html"));
        }


        [Test]
        public void Test9_UpperCaseAndLowerCaseTags()
        {
            string html = @"<HTML><Head><Title>Test</Title></Head><Body><Div>Content</Div></Body></HTML>";
            HTMLDocument doc = new HTMLDocument();

            doc.Load(html);

            Assert.That(doc.HasError, Is.False);
            Assert.That(doc.Root.ChildNodes[0].Name, Is.EqualTo("HTML"));
            Assert.That(doc.Root.ChildNodes[0].ChildNodes[0].Name, Is.EqualTo("HEAD"));
        }


        [Test]
        public void Test10_FileLoading()
        {
            string testFile = Path.Combine(testFilePath, "test.html");
            File.WriteAllText(testFile, "<html><body><div>Test Content</div></body></html>");
            HTMLDocument doc = new HTMLDocument();

            doc.Load(testFile);

            Assert.That(doc.HasError, Is.False);
            Assert.That(doc.Root, Is.Not.Null);
            Assert.That(doc.Root.ChildNodes.Count, Is.EqualTo(1));
        }
    }
}
