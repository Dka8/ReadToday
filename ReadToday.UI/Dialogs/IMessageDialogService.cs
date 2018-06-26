
using System;

namespace ReadToday.UI.Dialogs
{
    public interface IMessageDialogService
    {
        MessageDialogResult ShowYesNoDialog(String title, String message);
    }

    public enum MessageDialogResult
    {
        Yes,
        No
    }
}
