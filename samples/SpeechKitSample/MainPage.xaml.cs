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

            DataContext = new MainViewModel();
        }
    }
}