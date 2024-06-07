using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VKRTalalaev.ClassFolder;
using VKRTalalaev.DataFolder;
using VKRTalalaev.WindowFolder;

namespace VKRTalalaev.PageFolder
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string hashedPassword = ComputeSha256Hash(PasswordTb.Password);
                DBEntities.ResetContext();
                var user = DBEntities.GetContext().User
                    .FirstOrDefault(u => u.Login == LoginTB.Text && u.PasswordHash == hashedPassword && !u.IsBanned);

                if (user != null)
                {
                    MBClass.ShowMesagePopup("Успешно", Application.Current.MainWindow);
                    new MainWindow().Show();
                    VariableClass.CurentUser = user.IdUser;
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window is Window && window.Title == "Autorisation")
                        {
                            window.Close();
                        }
                    }
                }
                else
                {
                    MBClass.ShowErrorPopup("Неверный логин или пароль", Application.Current.MainWindow);
                }
            }
            catch (Exception ex)
            {
                MBClass.ShowErrorPopup(ex.Message, Application.Current.MainWindow);
            }
        }

        private static string ComputeSha256Hash(string rawData)
        {
            // Создаем SHA256
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Вычисляем хэш - возвращаем массив байтов
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Преобразуем байты в строку
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new RegPage());
        }

        private void TextBlock_MouseEnter(object sender, MouseEventArgs e)
        {
            var textBlock = sender as TextBlock;
            textBlock.Foreground = new SolidColorBrush(Colors.LightGray); 
        }

        private void TextBlock_MouseLeave(object sender, MouseEventArgs e)
        {
            var textBlock = sender as TextBlock;
            textBlock.Foreground = new SolidColorBrush(Colors.White); 
        }
    }
}
