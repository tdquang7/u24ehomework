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
        public string Content { get; set; } // Nội dung câu hỏi
        public string LabelA { get; set; }  // Câu trả lời ở bên trái
        public string LabelB { get; set; }  // Câu trả lời ở bên phải
        public string Answer { get; set; }  // Chứa đáp án của câu hỏi
        
        public int TrackCount { get; set; } // Theo dõi câu hỏi này đã được hiển thị bao nhiêu lần

        /// <summary>
        /// Parse một node xml thành một câu hỏi
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
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
