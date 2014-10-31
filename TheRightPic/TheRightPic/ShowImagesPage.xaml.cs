using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using System.Xml.Linq;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace TheRightPic
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ShowImagesPage : Page
    {
        const int COUNTDOWN_STARTAT = 8;
        DispatcherTimer timer = new DispatcherTimer() ;
        int current = COUNTDOWN_STARTAT;

        DisplayImage top;
        DisplayImage bottom;
        Random randomizer;

        public ShowImagesPage()
        {
            randomizer = new Random();
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: chuyển việc load xml này về sự kiện start của MainPage
            List<DisplayImage> highQuestions = LoadDisplayImage("high/Questions.xml");
            List<DisplayImage> lowQuestions = LoadDisplayImage("low/Questions.xml");

            // Chọn ngẫu nhiên hai hình để hiển thị trên dưới
            top = highQuestions[randomizer.Next(highQuestions.Count)];
            bottom = lowQuestions[randomizer.Next(lowQuestions.Count)];
            
            // Hiển thị hai hình đã chọn ra
            imgTop.Source = new BitmapImage(new Uri(this.BaseUri, "high/" + top.FileName));
            imgBottom.Source = new BitmapImage(new Uri(this.BaseUri, "low/" + bottom.FileName));

            // Hack, gọi hàm đếm ngược ngay lập tức
            timer_Tick(null, null);

            // Đếm ngược thời gian
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();

        }

        void timer_Tick(object sender, object e)
        {
            if (current == -1)
            {
                timer.Stop();

                // Lựa một trong hai hình để đặt câu hỏi
                DisplayImage chosen = randomizer.Next() % 2 == 0 ? top : bottom;

                // Chuyển qua hiển thị câu hỏi 
                Frame.Navigate(typeof(ShowQuestionPage), chosen);
            }
            else
            {
                // Hiển thị số giây còn lại
                lblTimer.Text = current.ToString() + "s";
                current--;
            }
        }

        // Nạp danh sách các hình với câu hỏi tương ứng từ tập tin xml
        List<DisplayImage> LoadDisplayImage(string xmlPath)
        {
            XDocument doc = XDocument.Load(xmlPath);

            var imgs = doc.Descendants("Image").ToList<XElement>();
            var list = new List<DisplayImage>();

            foreach (var img in imgs)
                list.Add(DisplayImage.Parse(img));

            return list;
        }


        private void GoHome(object sender, RoutedEventArgs e)
        {

        }

    }
}
