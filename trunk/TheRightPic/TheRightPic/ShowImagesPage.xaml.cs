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

        public ShowImagesPage()
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
            // Display images to raise question
            //imgTop.Source = new BitmapImage(new Uri(this.BaseUri, "puzzles.jpg"));



            timer_Tick(null, null);

            timer.Interval = TimeSpan.FromMilliseconds(40);
            timer.Tick += timer_Tick;
            timer.Start();

        }
        int count = 0;
        bool flag = true;

        void timer_Tick(object sender, object e)
        {
            if (flag)
            {
                imgTop.Width -= 3;
                imgTop.Height -= 3;
            }
            else
            {                
                imgTop.Width += 3;
                imgTop.Height += 3;
            }

            count++;
            if (count == 10)
            {
                count = 0;
                flag = !flag;
            }
           


            if (current == -1000)
            {
                timer.Stop();
                Frame.Navigate(typeof(ShowQuestionPage));
            }
            else
            {
                lblTimer.Text = current.ToString() + "s";
                current--;
            }
        }

        private void GoHome(object sender, RoutedEventArgs e)
        {

        }

    }
}
