using System;
using System.Windows;

namespace ReadToday.UI.Dialogs
{
  public partial class YesNoDialog : Window
  {
    public YesNoDialog(String title, String message )
    {
      InitializeComponent();
      Title = title;
      textBlock.Text = message;
    }

    private void ButtonYes_Click(Object sender, RoutedEventArgs e)
    {
      DialogResult = true;
    }

    private void ButtonNo_Click(Object sender, RoutedEventArgs e)
    {
      DialogResult = false;
    }

  }
}
