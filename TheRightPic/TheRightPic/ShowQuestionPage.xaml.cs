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
using Windows.UI;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace TheRightPic
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ShowQuestionPage : Page
    {
        const int COUNTDOWN_STARTAT = 10;
        DispatcherTimer timer = new DispatcherTimer();
        int current = COUNTDOWN_STARTAT;
        Question selectedQuestion;

        public ShowQuestionPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            DisplayImage img = e.Parameter as DisplayImage;
            Random randomizer = new Random();

            selectedQuestion = img.Questions[randomizer.Next(img.Questions.Count)];

            lblQuestion.Text = selectedQuestion.Content;
            btnTop.Content = selectedQuestion.LabelA;
            btnBottom.Content = selectedQuestion.LabelB;

            // Hack, bắt đầu xử lí sự kiện timer ngay lập tức
            timer_Tick(null, null);

            // Bắt đầu đếm ngược thời gian
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
        }
        

        void timer_Tick(object sender, object e)
        {
            if (current == -1)
            {
                timer.Stop();

                // TODO: Hiển thị kết quả và chuyển qua câu hỏi kế


            }
            else
            {
                lblTimer.Text = current.ToString();
                current--;
            }
        }

        private void btnTop_Click(object sender, RoutedEventArgs e)
        {
            // Ẩn nút còn lại
            btnBottom.Visibility = Visibility.Collapsed;

            responseCorrect()

            if (selectedQuestion.Answer == selectedQuestion.LabelA)
                responseCorrect();
            else
                responseWrong();
        }

        private void btnBottom_Click(object sender, RoutedEventArgs e)
        {
            // Ẩn nút còn lại
            btnTop.Visibility = Visibility.Collapsed;

            if (selectedQuestion.Answer == selectedQuestion.LabelB)
                responseCorrect();
            else
                responseWrong();
        }

        void response(Button b, bool correct)
        {
            b.Background = correct ? Color.FromArgb() : Color.FromArgb();
        }

        // Hồi đáp nếu trả lời đúng
        void responseCorrect()
        {
            
        }

        // Hồi đáp nếu trả lời sai
        void responseWrong()
        {

        }

    }
}
