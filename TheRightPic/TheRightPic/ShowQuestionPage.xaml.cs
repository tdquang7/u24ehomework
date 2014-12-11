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
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace NowUSeeIt
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
        DisplayImage image;
        bool answered = false;
        Random randomizer = new Random();

        public ShowQuestionPage()
        {
            this.InitializeComponent();

            // Đặt ở đây để tiện khi thiết kế
            logo.Visibility = Visibility.Collapsed;
            btnNext.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Reset trang về trạng thái ban đầu, trống trơn
            leftResponse.Visibility = Visibility.Collapsed;
            rightResponse.Visibility = Visibility.Collapsed;
            imgAnswer.Visibility = Visibility.Collapsed;

            // Hiển thị điểm hiện tại
            lblScore.Text = String.Format("Điểm: {0}/{1}", Global.CurrentPoint, Global.AnsweredQuestionsCount);

            // Lựa chọn ngẫu nhiên câu hỏi từ hình
            image = e.Parameter as DisplayImage;
            image.TrackCount++;

            selectedQuestion = GetNextQuestion(image.Questions);

            // Hiển thị câu hỏi và các lựa chọn
            lblQuestion.Text = selectedQuestion.Content;
            btnTop.Content = selectedQuestion.LabelA;
            btnBottom.Content = selectedQuestion.LabelB;

            // Hack: bắt đầu xử lí sự kiện timer ngay lập tức
            timer_Tick(null, null);

            // Bắt đầu đếm ngược thời gian
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();

            // Một vài setup khác
            btnTop.Tag = leftResponse;
            btnBottom.Tag = rightResponse;
        }        

        Question GetNextQuestion(List<Question> list)
        {
            // Lựa ngẫu nhiên một câu hỏi
            int position = randomizer.Next(list.Count);
            int anchor = position; // Điểm neo vị trí ban đầu

            Question nextQuestion = list[position];

            // Xác định số lần được chọn lớn nhất là bao nhiêu
            int maxCount = list.Max(q => q.TrackCount);

            do
            {
                if (nextQuestion.TrackCount == maxCount)
                {
                    // Đi tới câu hỏi kế
                    position = (position + 1) % list.Count; // % để quay lại đầu danh sách
                    nextQuestion = list[position];
                }
                else
                    break;
            }
            while (position != anchor);

            return nextQuestion;
        }


        void timer_Tick(object sender, object e)
        {
            if (current == -1)
            {
                timer.Stop();
                response(null, false); // Hiển thị đáp án đúng
            }
            else
            {
                // Hiển thị thời gian còn lại
                lblTimer.Text = current.ToString() + "s";
                current--;
            }
        }

        private void btnTop_Click(object sender, RoutedEventArgs e)
        {
            leftResponse.Visibility = Visibility.Visible;

            if (!answered) // Ngăn cản việc bấm trả lời lại ảnh hưởng đến score
            {
                timer.Stop();
                lblTimer.Visibility = Visibility.Collapsed;

                btnBottom.IsEnabled = false;
                response(sender as Button, selectedQuestion.Answer == selectedQuestion.LabelA);

                answered = true;
            }            
        }

        private void btnBottom_Click(object sender, RoutedEventArgs e)
        {
            rightResponse.Visibility = Visibility.Visible;
            
            if (!answered) // Ngăn cản việc bấm trả lời lại ảnh hưởng đến score
            {
                timer.Stop();
                lblTimer.Visibility = Visibility.Collapsed;

                btnTop.IsEnabled = false;
                response(sender as Button, selectedQuestion.Answer == selectedQuestion.LabelB);

                answered = true;
            }
        }

        void response(Button b, bool correct)
        {
            Color RED = Color.FromArgb(255, 255, 0, 0);
            Color GREEN = Color.FromArgb(255, 0, 255, 0);

            if (b != null)
            {
                Image tag = b.Tag as Image;
                b.Background = new SolidColorBrush(correct ? GREEN : RED);                
                tag.Source = new BitmapImage(new Uri(this.BaseUri, "img/" + (correct ? "OK.png" : "Problem.png")));
            }
            else // Hết giờ
            {
                // Làm nổi bật nút đáp án
                string top = btnTop.Content.ToString() ;
                string answer = selectedQuestion.Answer;
                if (top == answer)
                    btnTop.Background = new SolidColorBrush(RED);
                else
                    btnBottom.Background = new SolidColorBrush(RED);                             
            }
            
            // Hiển thị hình ảnh của câu hỏi để người chơi kiểm tra lại
            imgAnswer.Source = new BitmapImage(new Uri(this.BaseUri, image.BasePath + image.FileName));
            imgAnswer.Visibility = Visibility.Visible;
            logo.Visibility = Visibility.Visible;
            btnNext.Visibility = Visibility.Visible;

            // Cập nhật điểm
            if (correct)
            {
                Global.CurrentPoint++;
            }

            Global.AnsweredQuestionsCount++;

            lblScore.Text = String.Format("Điểm: {0}/{1}", Global.CurrentPoint, Global.AnsweredQuestionsCount);

            // Lưu lại 
            ApplicationData.Current.LocalSettings.Values["Points"] = Global.CurrentPoint;
            ApplicationData.Current.LocalSettings.Values["AnsweredQuestionsCount"] = Global.AnsweredQuestionsCount;
        }

        private void GoHome(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            Frame.Navigate(typeof(MainPage));
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ShowImagesPage));
        }
    }
}
