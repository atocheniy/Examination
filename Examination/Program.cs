namespace Examination
{
    internal class Program
    {
        static void Main(string[] args)
        {
            HTMLDocument doc1 = new HTMLDocument();
            doc1.Load("test.html");
            PrintNode(doc1);

        }
    }
}
