using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NowUSeeIt
{
    class DisplayImage
    {
        public string BasePath { get; set; } // Đường dẫn chứa hình
        public string FileName { get; set; } // Tên file hình chứa ảnh
        public List<Question> Questions { get; set; } // Danh sách các câu hỏi
                
        public int TrackCount { get; set; } // Theo dõi số lần được hiển thị của hình

        /// <summary>
        /// Chuyển một node xml thành hình ảnh hiển thị với các thông tin liên quan như các câu hỏi và đáp án tương ứng
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
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
