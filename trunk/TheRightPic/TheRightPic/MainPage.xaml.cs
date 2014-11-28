using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Xml.Linq;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.System.Display;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace NowUSeeIt
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.

            // Cập nhật lại điểm hiện có ở đây nhờ lớp Global
            if(false == Global.Launched) // Vừa mới chạy chương trình lên lần đầu
            {
                Global.TopImageList = LoadDisplayImage("img/high/Questions.xml", "img/high/");
                Global.BottomImageList = LoadDisplayImage("img/low/Questions.xml", "img/low/");

                if (false == ApplicationData.Current.LocalSettings.Values.ContainsKey("Points"))
                {
                    ApplicationData.Current.LocalSettings.Values["Points"] = 0;
                }           

                if (!ApplicationData.Current.LocalSettings.Values.ContainsKey("AnsweredQuestionsCount"))
                    ApplicationData.Current.LocalSettings.Values["AnsweredQuestionsCount"] = 0;


                Global.CurrentPoint = (int)ApplicationData.Current.LocalSettings.Values["Points"];
                Global.AnsweredQuestionsCount = (int)ApplicationData.Current.LocalSettings.Values["AnsweredQuestionsCount"];

                Global.Launched = true;
            }
            else
            {
                // Tránh trường hợp bấm vào rồi quay lại vẫn thấy hướng dẫn y như cũ
                txtHowTo.Text = "Cách chơi?";
            }

            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("AnsweredQuestionsCount"))
                btnStart.Content = "Chơi tiếp";


            // Cho biết trạng thái chơi games hiện tại
            lblTotalImages.Text = string.Format("Đang có: {0} hình.", Global.TopImageList.Count * 2);
            lblTotalQuestions.Text = string.Format("Đã trả lời: {0}/{1} câu hỏi.", Global.AnsweredQuestionsCount, Global.TopImageList.Count * 5 * 2);
            lblCurrentScore.Text = string.Format("Điểm: {0}/{1}", Global.CurrentPoint, Global.AnsweredQuestionsCount);
        }

        

        // Nạp danh sách các hình với câu hỏi tương ứng từ tập tin xml
        List<DisplayImage> LoadDisplayImage(string xmlPath, string basePath)
        {
            XDocument doc = XDocument.Load(xmlPath);

            var imgs = doc.Descendants("Image").ToList<XElement>();
            var list = new List<DisplayImage>();

            foreach (var node in imgs)
            {
                DisplayImage img = DisplayImage.Parse(node);
                img.BasePath = basePath;
                list.Add(img);
            }

            return list;
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            // Yêu cầu không tắt màn hình trong suốt thời gian chơi            
            Global.Display.RequestActive();

            // Chuyển qua màn hình hiển thị câu hỏi
            Frame.Navigate(typeof(ShowImagesPage));
        }
        
        private void btnHowTo_Click(object sender, RoutedEventArgs e)
        {
            txtHowTo.Text= "Nhìn thật kĩ hai hình, sau đó trả lời câu hỏi!";
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            var msgBox = new MessageDialog("Bạn có chắc muốn xóa điểm hiện tại và tính lại từ đầu?");
            msgBox.Commands.Add(new UICommand("Có", ResetOK));
            msgBox.Commands.Add(new UICommand("Không", null));
            msgBox.ShowAsync();
        }

        void ResetOK(IUICommand command)
        {
            Global.AnsweredQuestionsCount = 0;
            Global.CurrentPoint = 0;
            ApplicationData.Current.LocalSettings.Values["Points"] = 0;
            ApplicationData.Current.LocalSettings.Values["AnsweredQuestionsCount"] = 0;

            lblTotalQuestions.Text = string.Format("Đã trả lời: {0}/{1} câu hỏi.", 0, Global.TopImageList.Count * 5 * 2);
            lblCurrentScore.Text = string.Format("Điểm: {0}/{1}", 0, 0);

            btnStart.Content = "Bắt đầu";
        }
    }
}
