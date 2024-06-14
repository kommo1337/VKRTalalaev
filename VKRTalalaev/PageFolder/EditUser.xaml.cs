using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
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

namespace VKRTalalaev.PageFolder
{
    /// <summary>
    /// Логика взаимодействия для EditUser.xaml
    /// </summary>
    public partial class EditUser : Page
    {
        User user;
        public EditUser(User user)
        {
            this.user = user;
            DataContext = user;
            
            InitializeComponent();
            if (user.IsBanned==true )
            {
                BanUser.IsChecked = true;
            }
            else
            {
                BanUser.IsChecked = false;
            }
            LoadComboBoxData();
        }

        private void LoadComboBoxData()
        {
            DBEntities.ResetContext();

            GenderCb.ItemsSource = DBEntities.GetContext().Role.ToList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int index = GenderCb.SelectedIndex + 1;
                user = DBEntities.GetContext().User
                    .FirstOrDefault(u => u.IdUser == user.IdUser);

                // Check if ArticulTb is empty
                if (!string.IsNullOrWhiteSpace(ArticulTb.Text))
                {
                    string hashedPassword = ComputeSha256Hash(ArticulTb.Text);
                    user.PasswordHash = hashedPassword;
                }

                user.IdRole = index;
                user.Login = NameTb.Text;

                if (BanUser.IsChecked == true)
                {
                    user.IsBanned = true;
                }
                else
                {
                    user.IsBanned = false;
                }

                DBEntities.GetContext().SaveChanges();
                MBClass.ShowMesagePopup("Успешно", Application.Current.MainWindow);
            }
            catch (Exception ex)
            {
                MBClass.ShowErrorPopup(ex.Message, Application.Current.MainWindow);
            }

        }

        private static string ComputeSha256Hash(string rawData)
        {

            using (SHA256 sha256Hash = SHA256.Create())
            {

                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
