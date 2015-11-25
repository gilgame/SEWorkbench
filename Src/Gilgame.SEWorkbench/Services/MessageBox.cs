using System;

namespace Gilgame.SEWorkbench.Services
{
    public static class MessageBox
    {

        public static System.Windows.MessageBoxResult ShowMessage(string message)
        {
            return System.Windows.MessageBox.Show(
                message,
                "Space Engineers Workbench",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Information
            );
        }

        public static System.Windows.MessageBoxResult ShowError(string error)
        {
            return System.Windows.MessageBox.Show(
                error,
                "Space Engineers Workbench",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Error
            );
        }

        public static System.Windows.MessageBoxResult ShowQuestion(string question)
        {
            return System.Windows.MessageBox.Show(
                question,
                "Space Engineers Workbench",
                System.Windows.MessageBoxButton.YesNoCancel,
                System.Windows.MessageBoxImage.Question
            );
        }
    }
}
