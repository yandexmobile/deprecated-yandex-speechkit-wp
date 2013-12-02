using System.Windows;
using System.Windows.Navigation;
using Yandex.SpeechKit.UI;

namespace Yandex.SpeechKit.Demo
{
    /// <summary>
    /// Page with speech recognizer. Contains standard SpeechKit GUI.
    /// </summary>
    public partial class RecognizerPage
    {
        public RecognizerPage()
        {
            InitializeComponent();
        }

        #region overrides

        protected override void OnNavigatedTo(NavigationEventArgs args)
        {
            RecognizerView.StartRecognition("ru-RU", LanguageModel.General);

            base.OnNavigatedTo(args);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs args)
        {
            RecognizerView.CancelRecognition();

            base.OnNavigatedFrom(args);
        }

        #endregion

        private void RecognizerViewFinished(object sender, RecognitionFinishedEventArgs args)
        {
            string result = args.Result;
            if (!string.IsNullOrEmpty(result))
            {
                MessageBox.Show(result);
            }

            NavigationService.GoBack();
        }
    }
}