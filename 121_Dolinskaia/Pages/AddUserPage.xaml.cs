using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace _121_Dolinskaia.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddUserPage.xaml
    /// </summary>
    public partial class AddUserPage : Page
    {
        private User _currentUser = new User();
        bool flagEdit = false;
        public AddUserPage(User selectedUser)
        {
            InitializeComponent();

            if (selectedUser != null)
            {
                _currentUser = selectedUser;
                flagEdit = true;
            }
            DataContext = _currentUser;

            if (!string.IsNullOrWhiteSpace(_currentUser.Login))
                txtHintLogin.Visibility = Visibility.Hidden;
            if (!string.IsNullOrWhiteSpace(_currentUser.Password))
                txtHintPassword.Visibility = Visibility.Hidden;
            if (!(ComboBoxRole.Text == ""))
                ComboBoxRole.SelectedIndex = 1;

            if (!string.IsNullOrWhiteSpace(_currentUser.FIO))
                txtHintFIO.Visibility = Visibility.Hidden;
            if (!string.IsNullOrWhiteSpace(_currentUser.Photo))
                txtHintPhoto.Visibility = Visibility.Hidden;
        }
        private void clearAll()
        {
            TextBoxLogin.Clear();
            PasswordBox.Clear();
            ComboBoxRole.SelectedIndex = 1;
            TextBoxFIO.Clear();
            TextBoxPhoto.Clear();
        }
        public static string GetHash(string password)
        {
            using (var hash = SHA1.Create())
            {
                return string.Concat(hash.ComputeHash(Encoding.UTF8.GetBytes(password)).Select(x => x.ToString("X2")));
            }
        }

        private void txtHintLogin_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            txtHintLogin.Visibility = Visibility.Hidden;
            TextBoxLogin.Focus();
        }

        private void txtHintPassword_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            txtHintPassword.Visibility = Visibility.Hidden;
            PasswordBox.Focus();
        }

        private void txtHintFIO_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            txtHintFIO.Visibility = Visibility.Hidden;
            TextBoxFIO.Focus();
        }

        private void TextBoxLogin_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TextBoxLogin.Text.Length == 0)
            {
                txtHintLogin.Visibility = Visibility.Visible;
            }
        }

        private void PasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (PasswordBox.Text.Length == 0)
            {
                txtHintPassword.Visibility = Visibility.Visible;
            }
        }

        private void TextBoxFIO_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TextBoxFIO.Text.Length == 0)
            {
                txtHintFIO.Visibility = Visibility.Visible;
            }
        }

        private void TextBoxPhoto_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TextBoxPhoto.Text.Length == 0)
            {
                txtHintPhoto.Visibility = Visibility.Visible;
            }
        }

        private void txtHintPhoto_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            txtHintPhoto.Visibility = Visibility.Hidden;
            TextBoxPhoto.Focus();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            clearAll();
            NavigationService?.Navigate(new AdminPage());
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_currentUser.Login))
                errors.AppendLine("Укажите логин!");
            if (string.IsNullOrWhiteSpace(_currentUser.Password))
                errors.AppendLine("Укажите пароль!");
            if ((_currentUser.Role == null) || (ComboBoxRole.Text == ""))
                errors.AppendLine("Выберите роль!");
            else
                _currentUser.Role = ComboBoxRole.Text;
            if (string.IsNullOrWhiteSpace(_currentUser.FIO))
                errors.AppendLine("Укажите Ф.И.О.!");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_currentUser.ID == 0)
            {
                _currentUser.Password = GetHash(_currentUser.Password); 
                Entities.GetContext().User.Add(_currentUser);
                try
                {
                    Entities.GetContext().SaveChanges();
                    MessageBox.Show("Данные успешно сохранены.");
                    clearAll();


                    _currentUser = new User();
                    DataContext = _currentUser;

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
            else
            {
                _currentUser.Password = GetHash(_currentUser.Password);
                try
                {
                    Entities.GetContext().SaveChanges();
                    MessageBox.Show("Данные успешно сохранены.");
                    clearAll();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }
    }
}
