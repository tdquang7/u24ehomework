using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TheRightPic
{
    class DisplayImage
    {
        public string FileName { get; set; }
        public List<Question> Questions { get; set; }

        public static DisplayImage Parse(XElement node)
        {
            string filename = node.Attribute("FileName").Value;
            var questions = new List<Question>();

            foreach(var child in node.Descendants("Question"))
            {
                questions.Add(Question.Parse(child));
            }

            return new DisplayImage { FileName = filename, Questions = questions};
        }


    }
}
