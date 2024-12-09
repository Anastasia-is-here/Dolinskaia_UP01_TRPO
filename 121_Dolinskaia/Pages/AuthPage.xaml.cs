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
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();
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

        private void txtBxLogin_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TextBoxLogin.Text.Length == 0)
            {
                txtHintLogin.Visibility = Visibility.Visible;
            }

        }

        private void PasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (PasswordBox.Password.Length == 0)
            {
                txtHintPassword.Visibility = Visibility.Visible;
            }
        }

        private void txtHintPassword_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            txtHintPassword.Visibility = Visibility.Hidden;
            PasswordBox.Focus();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxLogin.Text) || string.IsNullOrEmpty(PasswordBox.Password))
            {
                MessageBox.Show("Введите логин и пароль.");
                return;
            }

            using (var db = new Entities())
            {
                String password = GetHash(PasswordBox.Password);


                var user = db.User
                    .AsNoTracking()
                    .FirstOrDefault(u => u.Login == TextBoxLogin.Text && u.Password == password);

                if (user == null)
                {
                    MessageBox.Show("Пользователь с такими данными не найден.");
                    return;
                }
                else
                {
                    MessageBox.Show("Пользователь успешно найден.");
                    TextBoxLogin.Clear();

                    switch (user.Role)
                    {
                        case "Администратор":
                            NavigationService?.Navigate(new AdminPage());
                            break;
                        case "Пользователь":
                            NavigationService?.Navigate(new UserPage());
                            break;
                    }
                }


            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new RegPage());
        }
    }
}
