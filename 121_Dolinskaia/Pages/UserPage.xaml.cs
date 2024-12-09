using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace _121_Dolinskaia.Pages
{
    /// <summary>
    /// Логика взаимодействия для UserPage.xaml
    /// </summary>
    public partial class UserPage : Page
    {
        public UserPage()
        {
            InitializeComponent();
            UpdateUsers();

            var currentUsers = Entities.GetContext().User.ToList();
            ListUser.ItemsSource = currentUsers;

            sortFilter.SelectedIndex = 0;
            roleFilter.IsChecked = false;

        }

        private void UpdateUsers()
        {
            //загружаем всех пользователей в список
            var currentUsers = Entities.GetContext().User.ToList();

            //осуществляем поиск по Ф.И.О. без учета регистра букв
            currentUsers = currentUsers.Where(x => x.FIO.ToLower().Contains(FIOfilter.Text.ToLower())).ToList();

            //обрабатываем фильтр по одной роли пользователей
            if (roleFilter.IsChecked.Value)
                currentUsers = currentUsers.Where(x => x.Role.Contains("Пользователь")).ToList();

            //осуществляем сортировку в зависимости от выбора пользователя
            if (sortFilter.SelectedIndex == 0)
                ListUser.ItemsSource = currentUsers.OrderBy(x => x.FIO).ToList();
            else ListUser.ItemsSource = currentUsers.OrderByDescending(x => x.FIO).ToList();
        }

        private void FIOfilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateUsers();
        }

        private void sortFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateUsers();
        }

        private void roleFilter_Checked(object sender, RoutedEventArgs e)
        {
            UpdateUsers();
        }

        private void roleFilter_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateUsers();
        }

        private void clearFilter_Click(object sender, RoutedEventArgs e)
        {
            FIOfilter.Text = "";
            sortFilter.SelectedIndex = 0;
            roleFilter.IsChecked = false;
        }
    }
    }
