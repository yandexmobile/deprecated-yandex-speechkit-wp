using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace Yandex.SpeechKit.Demo
{
    /// <summary>
    /// View-model for the main application page.
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _status;
        private double _power;

        private Recognizer _recognizer;

        static MainViewModel()
        {
            SpeechKitInitializer.Configure("7385ba8e-b595-4411-9404-093bff3e042c");
            SpeechKitInitializer.SetParameter("soundformat", "speex");
        }

        public MainViewModel()
        {
            Results = new ObservableCollection<string>();

            StartRecognitionCommand = new RelayCommand(StartRecognition);
            FinishRecordingCommand = new RelayCommand(FinishRecording);
            CancelRecognitionCommand = new RelayCommand(CancelRecognition);

            UpdateStatus("Ready");
        }

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region public properties

        public ICommand StartRecognitionCommand { get; private set; }
        
        public ICommand FinishRecordingCommand { get; private set; }
        
        public ICommand CancelRecognitionCommand { get; private set; }

        public ObservableCollection<string> Results { get; private set; }

        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged();
            }
        }

        public double Power
        {
            get { return _power; }
            set
            {
                _power = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region command handlers

        private void StartRecognition()
        {
            Results.Clear();
            UpdateStatus("Initializing...");

            if (_recognizer != null)
            {
                _recognizer.RecognitionDone -= RecognizerRecognitionDone;
                _recognizer.RecognitionError -= RecognizerRecognitionError;
                _recognizer.RecordingStarted -= RecognizerRecordingStarted;
                _recognizer.RecordingFinished -= RecognizerRecordingFinished;
                _recognizer.PowerUpdated -= RecognizerPowerUpdated;
                _recognizer.Cancel();
            }

            _recognizer = Recognizer.Create("ru-RU", LanguageModel.General);
            _recognizer.RecognitionDone += RecognizerRecognitionDone;
            _recognizer.RecognitionError += RecognizerRecognitionError;
            _recognizer.RecordingStarted += RecognizerRecordingStarted;
            _recognizer.RecordingFinished += RecognizerRecordingFinished;
            _recognizer.PowerUpdated += RecognizerPowerUpdated;
            _recognizer.Start();
        }

        private void FinishRecording()
        {
            if (_recognizer != null)
            {
                _recognizer.FinishRecording();
            }
        }

        private void CancelRecognition()
        {
            if (_recognizer != null)
            {
                _recognizer.Cancel();
            }
        }

        #endregion

        #region Recognizer events handlers

        private void RecognizerRecognitionDone(Recognizer sender, Recognition recognition)
        {
            RecognitionHypothesis[] results = recognition.Results ?? new RecognitionHypothesis[0];

            BeginInvoke(() =>
            {
                UpdateStatus(string.Format("{0} result(s)", results.Length));
                foreach (RecognitionHypothesis hypothesis in results)
                {
                    Results.Add(string.Format("{0:0.00}: {1}", hypothesis.Confidence, hypothesis.Text));
                }
            });
        }

        private void RecognizerRecognitionError(Recognizer sender, Error error)
        {
            BeginInvoke(() => UpdateStatus("Error: " + error.Text));
        }

        private void RecognizerRecordingStarted(Recognizer sender)
        {
            BeginInvoke(() => UpdateStatus("Recording started"));
        }

        private void RecognizerRecordingFinished(Recognizer sender)
        {
            BeginInvoke(() => UpdateStatus("Recording finished"));
        }

        private void RecognizerPowerUpdated(Recognizer sender, float value)
        {
            BeginInvoke(() => { Power = value; });
        }

        #endregion

        #region private methods

        private void UpdateStatus(string text)
        {
            Status = text;
            Power = 0;
        }

        /// <summary>
        /// Executes the specified action asynchronously on the UI thread.
        /// </summary>
        private static void BeginInvoke(Action action)
        {
            Deployment.Current.Dispatcher.BeginInvoke(action);
        }

        #endregion
    }
}
