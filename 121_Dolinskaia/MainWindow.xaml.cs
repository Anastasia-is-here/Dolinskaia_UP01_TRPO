using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace _121_Dolinskaia
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.IsEnabled = true;
            timer.Tick += (o, t) => { DateNow.Content = DateTime.Now.ToString(); };
            timer.Start();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var res = MessageBox.Show("Вы уверены, что хотите закрыть программу?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res == MessageBoxResult.Yes)
            {
                e.Cancel = false;
                return;
            }
            else
            {
                e.Cancel = true;
                return;
            }
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (!(e.Content is Page page)) return;
            this.Title = $"ProjectByDolinskaya - {page.Title}";

            if (page is Pages.AuthPage)
                Back.Visibility = Visibility.Hidden;
            else
                Back.Visibility = Visibility.Visible;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.CanGoBack) MainFrame.GoBack();
        }
    }

}
