using System;

namespace Gilgame.SEWorkbench.Services
{
    public static class MessageBox
    {

        public static System.Windows.MessageBoxResult ShowMessage(string message)
        {
            return System.Windows.MessageBox.Show(
                message,
                "SE Workbench",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Information
            );
        }

        public static System.Windows.MessageBoxResult ShowError(string error, Exception ex)
        {
            string message = String.Format(
                "{0} ({1}){2}{2}{3}",
                error,
                ex.Message,
                Environment.NewLine,
                ex.StackTrace
            );
            
            return System.Windows.MessageBox.Show(
                message,
                "SE Workbench",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Error
            );
        }

        public static void ShowError(object ex)
        {
            Exception exception = (Exception)ex;

            Views.ExceptionView view = new Views.ExceptionView();

            ViewModels.ExceptionViewModel context = (ViewModels.ExceptionViewModel)view.DataContext;

            string message = String.Format(
                "{0}{1}{1}{2}",
                exception.Message,
                Environment.NewLine,
                exception.StackTrace
            );

            context.Details = message;

            view.ShowDialog();
        }

        public static System.Windows.MessageBoxResult ShowQuestion(string question)
        {
            return System.Windows.MessageBox.Show(
                question,
                "SE Workbench",
                System.Windows.MessageBoxButton.YesNoCancel,
                System.Windows.MessageBoxImage.Question
            );
        }
    }
}
