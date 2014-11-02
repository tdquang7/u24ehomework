using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NowUSeeIt
{
    class Global
    {
        public static bool Launched { get; set; }
        public static int CurrentPoint {get; set;}
        public static int AnsweredQuestionsCount { get; set; }
        public static int TotalQuestions { get; set; }
        public static List<DisplayImage> TopImageList { get; set; }
        public static List<DisplayImage> BottomImageList { get; set; }
    }
}
