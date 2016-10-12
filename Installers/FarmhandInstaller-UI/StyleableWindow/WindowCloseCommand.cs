using System;
using System.Windows;
using System.Windows.Input;

namespace FarmhandInstaller.StyleableWindow
{
#pragma warning disable CS0067
    public class WindowCloseCommand :ICommand
    {     

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            var window = parameter as Window;

            if (window != null)
            {
                window.Close();
            }
        }
    }
#pragma warning restore CS0067
}
