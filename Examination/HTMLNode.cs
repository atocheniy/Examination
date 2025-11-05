using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Examination
{
    public class HTMLElement : HTMLNode
    {
        public string TagName { get; private set; }
        public List<HTMLAttribute> Attributes { get; }
        public List<HTMLNode> Children { get; }

        public HTMLElement(string tagName)
        {
            TagName = tagName;
            Attributes = new List<HTMLAttribute>();
            Children = new List<HTMLNode>();
        }

        public void AddChild(HTMLNode childNode)
        {
            childNode.Parent = this;
            Children.Add(childNode);
        }

        public void AddAttribute(string name, string value)
        {
            Attributes.Add(new HTMLAttribute(name, value));
        }
    }

    public abstract class HTMLNode
    {
        public HTMLElement Parent { get; internal set; }
    }

    public class HTMLTextNode : HTMLNode
    {
        public string Text { get; set; }

        public HTMLTextNode(string text)
        {
            Text = text.Trim();
        }
    }
}
