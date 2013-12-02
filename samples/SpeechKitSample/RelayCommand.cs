using System;
using System.Windows.Input;

namespace Yandex.SpeechKit.Demo
{
    /// <summary>
    /// Basic ICommand implementation.
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action _handler;

        public RelayCommand(Action handler)
        {
            _handler = handler;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (_handler != null)
            {
                _handler();
            }
        }

        event EventHandler ICommand.CanExecuteChanged
        {
            add { }
            remove { }
        }
    }
}