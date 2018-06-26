using System;
using System.Windows;

namespace ReadToday.UI.Dialogs
{
    public class CMessageDialogService : IMessageDialogService
    {
        public MessageDialogResult ShowYesNoDialog(String title, String message)
        {
            return new YesNoDialog(title, message)
            {
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = App.Current.MainWindow
            }.ShowDialog().GetValueOrDefault()
                ? MessageDialogResult.Yes
                : MessageDialogResult.No;
        }
    }
}
