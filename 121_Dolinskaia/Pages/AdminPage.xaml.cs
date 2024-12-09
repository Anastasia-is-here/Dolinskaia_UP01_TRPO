using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace _121_Dolinskaia.Pages
{
    /// <summary>
    /// Логика взаимодействия для AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        public AdminPage()
        {
            InitializeComponent();
            DataGridUser.ItemsSource = Entities.GetContext().User.ToList();
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Entities.GetContext().ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
                DataGridUser.ItemsSource = Entities.GetContext().User.ToList();
            }
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new AddUserPage(null));
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new AddUserPage((sender as Button).DataContext as User));
        }

        private void ButtonDel_Click(object sender, RoutedEventArgs e)
        {
            var usersForRemoving = DataGridUser.SelectedItems.Cast<User>().ToList();

            if (MessageBox.Show($"Вы точно хотите удалить записи в количестве {usersForRemoving.Count()} элементов?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    Entities.GetContext().User.RemoveRange(usersForRemoving);
                    Entities.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены.");

                    DataGridUser.ItemsSource = Entities.GetContext().User.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }
    }
}
