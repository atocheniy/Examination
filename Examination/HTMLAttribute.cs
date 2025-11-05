using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examination
{
    public class HTMLAttribute
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public HTMLAttribute(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
