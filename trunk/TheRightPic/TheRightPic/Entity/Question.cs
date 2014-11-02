using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NowUSeeIt
{
    class Question
    {
        public string Content { get; set; }
        public string LabelA { get; set; }
        public string LabelB { get; set; }
        public string Answer { get; set; }

        public static Question Parse(XElement node)
        {
            string content = node.Attribute("Content").Value;
            string a = node.Attribute("LabelA").Value;
            string b = node.Attribute("LabelB").Value;
            string ans = node.Attribute("Answer").Value;

            return new Question { Content = content, LabelA = a, LabelB = b, Answer = ans };
        }
    }
}
