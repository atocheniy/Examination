using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examination
{
    public class HTMLDocument
    {
        private string _htmlString;

        public List<HTMLNode> ChildNodes;

        public bool IsValidHTML;

        public HTMLDocument()
        {
            IsValidHTML = false;
            ChildNodes = new List<HTMLNode>();
        }

        public void LoadDocument(string path)
        {
            try
            {
                var FilePath = path;
                _htmlString = File.ReadAllText(path);
                ParseHTML();
                IsValidHTML = true;
            }
            catch (Exception ex)
            {
                IsValidHTML = false;
                throw new Exception("Error loading HTML document: " + ex.Message);
            }

        }

        public void Clear()
        {
            _htmlString = string.Empty;
            IsValidHTML = false;
        }

        private void ParseHTML()
        {
            string[] allowedTags = { "html", "head", "body", "title", "div", "table", "tr", "td", "th", "img" };

           /* if (!IsValidHTML)
            {
                throw new Exception("Cannot parse invalid HTML document.");
            }*/

            Stack<HTMLElement> stack = new Stack<HTMLElement>();

            HTMLElement root = new HTMLElement("root");
            stack.Push(root);

            int index = 0; 

            while (index < _htmlString.Length)
            {
                int nextTag = _htmlString.IndexOf('<', index);
                if (nextTag == -1) nextTag = _htmlString.Length; 

                string text = _htmlString.Substring(index, nextTag - index);
                if (!string.IsNullOrWhiteSpace(text))
                {
                    HTMLTextNode textNode = new HTMLTextNode(text);
                    stack.Peek().AddChild(textNode);
                }

                index = nextTag; 
                if (index >= _htmlString.Length) break; 

                string remaining = _htmlString.Substring(index); 

                // Регулярное выражение для открывающего тега (<tag ...> или <tag ... />)
                var openMatch = System.Text.RegularExpressions.Regex.Match(
                    remaining, @"^<(?<tag>\w+)(?<attrs>[^>]*)\s*(/?)>");

                // Регулярное выражение для закрывающего тега (</tag>)
                var closeMatch = System.Text.RegularExpressions.Regex.Match(
                    remaining, @"^</(?<tag>\w+)\s*>");

                if (closeMatch.Success)
                {
                    string tagName = closeMatch.Groups["tag"].Value.ToLower();

                    if (!allowedTags.Contains(tagName))
                        throw new Exception($"Тег </{tagName}> не поддерживается.");

                    if (stack.Count <= 1 || stack.Peek().TagName.ToLower() != tagName)
                        throw new Exception($"Несоответствие закрывающего тега </{tagName}>.");

                    stack.Pop(); 
                    index += closeMatch.Length;
                }
                else if (openMatch.Success)
                {
                    string tagName = openMatch.Groups["tag"].Value.ToLower();

                    if (!allowedTags.Contains(tagName))
                        throw new Exception($"Тег <{tagName}> не поддерживается.");

                    string attrsPart = openMatch.Groups["attrs"].Value; 
                    bool selfClosing = openMatch.Groups[3].Value == "/" || tagName == "img";

                    HTMLElement element = new HTMLElement(tagName);

                    if (!string.IsNullOrWhiteSpace(attrsPart))
                        foreach (var attr in ParseAttributes(attrsPart))
                            element.AddAttribute(attr.Name, attr.Value);

                    stack.Peek().AddChild(element);

                    if (!selfClosing)
                        stack.Push(element);

                    index += openMatch.Length; 
                }
                else
                {
                    index++;
                }
            }

            if (stack.Count != 1)
                throw new Exception("Некорректный HTML: незакрытые теги.");

            ChildNodes = root.Children;
        }


        private List<HTMLAttribute> ParseAttributes(string attrs)
        {
            List<HTMLAttribute> list = new List<HTMLAttribute>();

            string[] parts = attrs.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string part in parts)
            {
                string[] pair = part.Split(new char[] { '=' }, 2);
                string name = pair[0].Trim().ToLower();

                if (name == "style") continue;

                string value = pair.Length > 1 ? pair[1].Trim('\"', '\'') : "";
                list.Add(new HTMLAttribute(name, value));
            }
            return list;
        }


        public void PrintDocument()
        {
            if (!IsValidHTML || ChildNodes == null || ChildNodes.Count == 0)
            {
                Console.WriteLine("Документ пуст или не загружен.");
                return;
            }

            foreach (var node in ChildNodes)
            {
                PrintNode(node);
            }
        }


        private void PrintNode(HTMLNode node)
        {
            if (node is HTMLElement element)
            {

                string attrStr = "";
                if (element.Attributes.Count > 0)
                {
                    foreach (var attr in element.Attributes)
                    {
                        attrStr += $" {attr.Name}=\"{attr.Value}\"";
                    }
                }

                Console.WriteLine($"<{element.TagName}{attrStr}>");


                foreach (var child in element.Children)
                {
                    PrintNode(child);
                }


                if (element.TagName != "img")
                    Console.WriteLine($"</{element.TagName}>");
            }
            else if (node is HTMLTextNode textNode)
            {
                
                Console.WriteLine(textNode.Text);
            }
        }



    }
}
