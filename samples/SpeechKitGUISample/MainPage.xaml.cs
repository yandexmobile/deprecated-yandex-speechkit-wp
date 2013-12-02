using System;
using System.Windows.Input;

namespace Yandex.SpeechKit.Demo
{
    /// <summary>
    /// Main application page.
    /// </summary>
    public partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void InitializeButtonTap(object sender, GestureEventArgs args)
        {
            MessageTextBlock.Text = "Initializing...";

            try
            {
                await SpeechKitInitializer.InitializeAsync();
            }
            catch (SpeechKitException e)
            {
                MessageTextBlock.Text = e.Message;
                return;
            }

            MessageTextBlock.Text = "Initialized";
        }

        private void RecognizerButtonTap(object sender, GestureEventArgs args)
        {
            NavigationService.Navigate(new Uri(@"/RecognizerPage.xaml", UriKind.Relative));
        }
    }
}