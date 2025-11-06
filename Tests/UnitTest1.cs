using Examination;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
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
            string html = 
@"<html>
    <head>            
        <title>Test1 Page</title>            
    </head>
    <body>                      
        <div>Content</div>                
    </body>            
</html>";            

            string file = Path.Combine(testFilePath, "test1.html");
            File.WriteAllText(file, html);

            HTMLDocument doc = new HTMLDocument();
            doc.LoadDocument (file);

            Assert.DoesNotThrow(() => doc.LoadDocument(file));
            Assert.That(doc.IsValidHTML, Is.True);
            Assert.That(doc.ChildNodes,Is.Not.Null);
            Assert.That(doc.ChildNodes.Count, Is.EqualTo(1));
        }//Test1_ValidHTML



        [Test]
        public void Test2_HTMLWithAttributes()
        {
            string html = @"<div id=""main"" class=""container"" data-test=""value"">Content</div>";
            string file = Path.Combine(testFilePath, "test2.html");
            File.WriteAllText(file, html);

            HTMLDocument doc = new HTMLDocument();
            doc.LoadDocument(file);


            Assert.DoesNotThrow(() => doc.LoadDocument(file));
            Assert.That(doc.IsValidHTML, Is.True);
            Assert.That(doc.ChildNodes, Is.Not.Null);
            Assert.That(doc.ChildNodes.Count, Is.EqualTo(1));

            HTMLElement divElem = doc.ChildNodes[0] as HTMLElement;
            Assert.That(divElem, Is.Not.Null);
            Assert.That(divElem.Attributes.Count, Is.EqualTo(3));
            Assert.That(divElem.Attributes[0].Name, Is.EqualTo("id"));
            Assert.That(divElem.Attributes[0].Value, Is.EqualTo("main"));
            Assert.That(divElem.Attributes[1].Name, Is.EqualTo("class"));
            Assert.That(divElem.Attributes[1].Value, Is.EqualTo("container"));
            Assert.That(divElem.Attributes[2].Name, Is.EqualTo("data-test"));
            Assert.That(divElem.Attributes[2].Value, Is.EqualTo("value"));
        }//Test2_HTMLWithAttributes



        [Test]
        public void Test3_TableStructure()
        {
            string html = 
@"<table>
    <tr>
        <th>Header</th>
    </tr>
    <tr>
        <td>Data</td>
    </tr>
</table>";
            string file = Path.Combine(testFilePath, "test3.html");
            File.WriteAllText(file, html);

            HTMLDocument doc = new HTMLDocument();
            doc.LoadDocument(file);

            Assert.DoesNotThrow(() => doc.LoadDocument(file));
            Assert.That(doc.IsValidHTML, Is.True);
            Assert.That(doc.ChildNodes, Is.Not.Null);
            Assert.That(doc.ChildNodes.Count, Is.EqualTo(1));

            HTMLElement tableElem = doc.ChildNodes[0] as HTMLElement;
            Assert.That(tableElem, Is.Not.Null);
            Assert.That(tableElem.TagName, Is.EqualTo("table"));
            Assert.That(tableElem.Children.Count, Is.EqualTo(2));

            HTMLElement firstRow = tableElem.Children[0] as HTMLElement;
            Assert.That(firstRow.TagName, Is.EqualTo("tr"));
            Assert.That(firstRow.Children.Count, Is.EqualTo(1));
            Assert.That((firstRow.Children[0] as HTMLElement).TagName, Is.EqualTo("th"));
        }//Test3_TableStructure



        [Test]
        public void Test4_ImageStructure()
        {
            string html = @"<img src=""image.jpg"" alt=""Test Image"" width=""100"" height=""50"">";
            string file = Path.Combine(testFilePath, "test4.html");
            File.WriteAllText(file, html);

            HTMLDocument doc = new HTMLDocument();
            doc.LoadDocument(file);

            Assert.DoesNotThrow(() => doc.LoadDocument(file));
            Assert.That(doc.IsValidHTML, Is.True);
            Assert.That(doc.ChildNodes, Is.Not.Null);
            Assert.That(doc.ChildNodes.Count, Is.EqualTo(1));

            HTMLElement imgElem = doc.ChildNodes[0] as HTMLElement;
            Assert.That(imgElem, Is.Not.Null);
            Assert.That(imgElem.TagName, Is.EqualTo("img"));
            Assert.That(imgElem.Attributes.Count, Is.GreaterThanOrEqualTo(4));
            Assert.That(imgElem.Children.Count, Is.EqualTo(0));
        }//Test4_ImageStructure



        [Test]
        public void Test5_TextNodes() 
        {
            string html = @"<div>Text before <span>inner</span> text after</div>";
            string file = Path.Combine(testFilePath, "test5.html");
            File.WriteAllText(file, html);

            HTMLDocument doc = new HTMLDocument();

            Assert.Throws<Exception>(() => doc.LoadDocument(file));
        }//Test5_TextNodes



        [Test]
        public void Test6_InvalidElement()
        {
            string html = @"<invalid>Test6</invalid>";
            string file = Path.Combine(testFilePath, "test6.html");
            File.WriteAllText(file, html);

            HTMLDocument doc = new HTMLDocument();
            Assert.Throws<Exception>(() => doc.LoadDocument(file));
        }//Test6_InvalidElement



        [Test]
        public void Test7_UnclosedElement()
        {
            string html = @"<div>Test7";
            string file = Path.Combine(testFilePath, "test7.html");
            File.WriteAllText(file, html);

            HTMLDocument doc = new HTMLDocument();
            Assert.Throws<Exception>(() => doc.LoadDocument(file));
        }//Test7_UnclosedElement



        [Test]
        public void Test8_ComplexDocument()
        {
           string html = 
@"<html>
    <head>
        <title>Test8</title>
    </head>
    <body>
        <div id=""header""></div>
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
            string file = Path.Combine(testFilePath, "test8.html");
            File.WriteAllText(file, html);

            HTMLDocument doc = new HTMLDocument();
            doc.LoadDocument(file);

            Assert.DoesNotThrow(() => doc.LoadDocument(file));
            Assert.That(doc.IsValidHTML, Is.True);
            Assert.That(doc.ChildNodes, Is.Not.Null);
            Assert.That(doc.ChildNodes.Count, Is.EqualTo(1));

            HTMLElement htmlElem = doc.ChildNodes[0] as HTMLElement;
            Assert.That(htmlElem.TagName, Is.EqualTo("html"));
            Assert.That(htmlElem.Children.Count, Is.EqualTo(2));
            Assert.That((htmlElem.Children[0] as HTMLElement).TagName, Is.EqualTo("head"));
            Assert.That((htmlElem.Children[1] as HTMLElement).TagName, Is.EqualTo("body"));
        }//Test8_ComplexDocument
        


        [Test]
        public void Test9_UpperCaseAndLowerCaseTags()
        {
            string html = 
@"<HTML>
    <Head>
        <Title>Test</Title>
    </Head>
    <Body>
        <Div>Content</Div>
    </Body>
</HTML>";
            string file = Path.Combine(testFilePath, "test9.html");
            File.WriteAllText(file, html);

            HTMLDocument doc = new HTMLDocument();
            doc.LoadDocument(file);

            Assert.DoesNotThrow(() => doc.LoadDocument(file));
            Assert.That(doc.IsValidHTML, Is.True);
            Assert.That(doc.ChildNodes, Is.Not.Null);


            HTMLElement htmlElem = doc.ChildNodes[0] as HTMLElement;
            HTMLElement headElem = htmlElem.Children[0] as HTMLElement;

            Assert.That(htmlElem.TagName, Is.EqualTo("html"));
            Assert.That(headElem.TagName, Is.EqualTo("head"));
        }//Test9_UpperCaseAndLowerCaseTags



        [Test]
        public void Test10_EmptyDocument()
        {
            string html = "";

            string file = Path.Combine(testFilePath, "test10.html");
            File.WriteAllText(file, html);

            HTMLDocument doc = new HTMLDocument();
            doc.LoadDocument(file);

            Assert.DoesNotThrow(() => doc.LoadDocument(file));
            Assert.That(doc.IsValidHTML, Is.True);
            Assert.That(doc.ChildNodes, Is.Not.Null);
            Assert.That(doc.ChildNodes.Count, Is.EqualTo(0));
        }//Test10_EmptyDocument



        [Test]
        public void Test11_NonExistentFile()
        {
            string file = Path.Combine(testFilePath, "test11.html");
            HTMLDocument doc = new HTMLDocument();
            Assert.Throws<Exception>(() => doc.LoadDocument(file));
        }//Test11_NonExistentFile



        [Test]
        public void Test12_PrintDocument()
        {
            string html = @"<div>Test12</div>";
            string file = Path.Combine(testFilePath, "test12.html");
            File.WriteAllText(file, html);

            HTMLDocument doc = new HTMLDocument();
            doc.LoadDocument(file);

            Assert.DoesNotThrow(() => doc.LoadDocument(file));
            Assert.That(doc.IsValidHTML, Is.True);
            Assert.That(doc.ChildNodes, Is.Not.Null);

            using(var writer = new StringWriter())
            {
                var OrirginalOutput = Console.Out;
                Console.SetOut(writer);

                try
                {
                    doc.PrintDocument();
                    string output = writer.ToString();

                    Assert.That(output, Is.Not.Empty);
                    Assert.That(output, Contains.Substring("div"));
                    Assert.That(output, Contains.Substring("Test12"));
                }
                finally
                {
                    Console.SetOut(OrirginalOutput);
                }//try-finally
            }//using
        }//Test12_PrintDocument
    }
}