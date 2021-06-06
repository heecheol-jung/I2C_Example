using Serilog;
using System.Windows;

namespace I2CWpfApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // https://github.com/serilog/serilog/wiki/AppSettings
            Log.Logger = new LoggerConfiguration().ReadFrom.AppSettings().CreateLogger();

            AppUtil.CreateDefaultDb();
        }
    }
}
