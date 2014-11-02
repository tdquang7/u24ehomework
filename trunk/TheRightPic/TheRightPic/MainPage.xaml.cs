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

                // Kiểm tra tập tin config.ini có tồn tại hay không

                //IsolatedStorageFile fileStorage = IsolatedStorageFile.GetUserStoreForApplication();

                if (false == ApplicationData.Current.LocalSettings.Values.ContainsKey("Points"))
                {
                    ApplicationData.Current.LocalSettings.Values["Points"] = 0;
                }
                else
                {
                    // Đồng nghĩa đã từng chơi rồi
                    btnStart.Content = "Continue";
                }
                    

                if (!ApplicationData.Current.LocalSettings.Values.ContainsKey("AnsweredQuestionsCount"))
                    ApplicationData.Current.LocalSettings.Values["AnsweredQuestionsCount"] = 0;


                Global.CurrentPoint = (int)ApplicationData.Current.LocalSettings.Values["Points"];
                Global.AnsweredQuestionsCount = (int)ApplicationData.Current.LocalSettings.Values["AnsweredQuestionsCount"];

                Global.Launched = true;
            }
            else
            {
                txtHowTo.Text = "How to play?";
            }
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
            Frame.Navigate(typeof(ShowImagesPage));
        }
        
        private void btnHowTo_Click(object sender, RoutedEventArgs e)
        {
            txtHowTo.Text= "Watch two pictures carefully, then answer the question!";
        }
    }
}
