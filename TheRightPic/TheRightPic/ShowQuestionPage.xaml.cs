﻿using System;
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
        DisplayImage img;
        bool answered = false;

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
            // Hiển thị điểm hiện tại
            lblScore.Text = String.Format("Score: {0}/{1}", Global.CurrentPoint, Global.AnsweredQuestionsCount);

            // Lựa chọn ngẫu nhiên câu hỏi từ hình
            img = e.Parameter as DisplayImage;
            Random randomizer = new Random();

            selectedQuestion = img.Questions[randomizer.Next(img.Questions.Count)];

            // Hiển thị câu hỏi và các lựa chọn
            lblQuestion.Text = selectedQuestion.Content;
            btnTop.Content = selectedQuestion.LabelA;
            btnBottom.Content = selectedQuestion.LabelB;

            // Hack, bắt đầu xử lí sự kiện timer ngay lập tức
            timer_Tick(null, null);

            // Bắt đầu đếm ngược thời gian
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();

            // Một vài setup khác
            btnTop.Tag = leftResponse;
            btnBottom.Tag = rightResponse;
        }        

        void timer_Tick(object sender, object e)
        {
            if (current == -1)
            {
                timer.Stop();
                response(null, false);
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
                if (btnTop.Content == selectedQuestion.Answer)
                    btnTop.Background = new SolidColorBrush(RED);
                else
                    btnBottom.Background = new SolidColorBrush(RED);                             
            }
            
            imgAnswer.Source = new BitmapImage(new Uri(this.BaseUri, img.BasePath + img.FileName));
            logo.Visibility = Visibility.Visible;
            btnNext.Visibility = Visibility.Visible;

            // Cập nhật điểm
            if (correct)
            {
                Global.CurrentPoint++;
            }

            Global.AnsweredQuestionsCount++;

            lblScore.Text = String.Format("Score: {0}/{1}", Global.CurrentPoint, Global.AnsweredQuestionsCount);

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