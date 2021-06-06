using I2CWpfApp.AppWnd;
using System.Windows;

namespace I2CWpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnRegisterValueEdit_Click(object sender, RoutedEventArgs e)
        {
            WndRegisterValue wndRegValue = new WndRegisterValue();
            wndRegValue.ShowDialog();
        }

        private void BtnI2CExample_Click(object sender, RoutedEventArgs e)
        {
            WndI2CExample wndI2CExample = new WndI2CExample();
            wndI2CExample.ShowDialog();
        }
    }
}
