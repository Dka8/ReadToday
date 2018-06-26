using System.Windows;
using Autofac;
using ReadToday.UI.Startup;
using ReadToday.UI.View;

namespace ReadToday.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            CBootStrapper bootStrapper = new CBootStrapper();
            IContainer container = bootStrapper.BootStrup();

            MainWindow mainWindow = container.Resolve<MainWindow>();

            mainWindow.Show();
        }
    }
}
