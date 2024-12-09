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
    /// Логика взаимодействия для RegPage.xaml
    /// </summary>
    public partial class RegPage : Page
    {
        private void clearAll()
        {
            TextBoxLogin.Clear();
            PasswordBox.Clear();
            PasswordBox_Confirm.Clear();
            ComboBoxRole.SelectedIndex = 1;
            TextBoxFIO.Clear();
        }
        public RegPage()
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

        private void txtHintPassword_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            txtHintPassword.Visibility = Visibility.Hidden;
            PasswordBox.Focus();
        }

        private void txtHintPassword_confirm_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            txtHintPassword_confirm.Visibility = Visibility.Hidden;
            PasswordBox_Confirm.Focus();
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
            if (PasswordBox.Password.Length == 0)
            {
                txtHintPassword.Visibility = Visibility.Visible;
            }
        }

        private void PasswordBox_Confirm_LostFocus(object sender, RoutedEventArgs e)
        {
            if (PasswordBox_Confirm.Password.Length == 0)
            {
                txtHintPassword_confirm.Visibility = Visibility.Visible;
            }
        }

        private void TextBoxFIO_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TextBoxFIO.Text.Length == 0)
            {
                txtHintFIO.Visibility = Visibility.Visible;
            }
        }

        private void RegistrationButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxLogin.Text) || string.IsNullOrEmpty(PasswordBox.Password) ||
                string.IsNullOrEmpty(PasswordBox_Confirm.Password) || string.IsNullOrEmpty(TextBoxFIO.Text))
            {
                MessageBox.Show("Не все поля заполнены.");
                return;
            }

            using (var db = new Entities())
            {
                var login = db.User
                    .AsNoTracking()
                    .FirstOrDefault(u => u.Login == TextBoxLogin.Text);

                if (login != null)
                {
                    MessageBox.Show("Пользователь с такими логином уже существует.");
                    return;
                }
            }

            if (!(PasswordBox.Password.Length < 6))
            {
                bool en = true; 
                bool number = false;

                for (int i = 0; i < PasswordBox.Password.Length; i++) 
                {
                    if (PasswordBox.Password[i] >= 'А' && PasswordBox.Password[i] <= 'Я' || PasswordBox.Password[i] >= 'а' && PasswordBox.Password[i] <= 'я') en = false;
                    if (PasswordBox.Password[i] >= '0' && PasswordBox.Password[i] <= '9') number = true; 
                }

                if (!en)
                {
                    MessageBox.Show("Допускается только английская раскладка.");
                    return;
                }
                else if (!number)
                {
                    MessageBox.Show("Пароль должен содержать хотя бы одну цифру.");
                    return;
                }

                if (PasswordBox.Password == PasswordBox_Confirm.Password)
                {
                    Entities db = new Entities();
                    User userObject = new User
                    {
                        FIO = TextBoxFIO.Text,
                        Login = TextBoxLogin.Text,
                        Password = GetHash(PasswordBox.Password),
                        Role = ComboBoxRole.Text,
                        Photo = String.IsNullOrEmpty(TextBoxPhoto.Text) ? null : TextBoxPhoto.Text

                    };
                    db.User.Add(userObject);
                    db.SaveChanges();

                    MessageBox.Show("Пользователь зарегистрирован.");
                    clearAll();
                    return;
                }
                else
                {
                    MessageBox.Show("Пароли не совпадают.");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Пароль должен содержать 6 или более символов");
                return;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            clearAll();
            NavigationService?.Navigate(new AuthPage());
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
    }
}
