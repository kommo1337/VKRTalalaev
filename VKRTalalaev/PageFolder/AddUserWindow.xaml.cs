﻿using System;
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
    /// Логика взаимодействия для AddUserWindow.xaml
    /// </summary>
    public partial class AddUserWindow : Page
    {
        public AddUserWindow()
        {
            InitializeComponent();
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

                string hashedPassword = ComputeSha256Hash(ArticulTb.Text);
                DBEntities.ResetContext();
                DBEntities.
                GetContext().User.Add(new User()
                {
                    Login = NameTb.Text,
                    PasswordHash = hashedPassword,
                    IdRole = 1,
                    IsBanned = false
                });
                DBEntities.GetContext().SaveChanges();
                DBEntities.ResetContext();
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
