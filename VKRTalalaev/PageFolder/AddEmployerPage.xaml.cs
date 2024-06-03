using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using VKRTalalaev.DataFolder;

namespace VKRTalalaev.PageFolder
{
    /// <summary>
    /// Логика взаимодействия для AddEmployerPage.xaml
    /// </summary>
    public partial class AddEmployerPage : Page
    {
        private byte[] FotoImageData;
        private byte[] FotoImageData2;
        private bool IsFotoLoaded;
        public AddEmployerPage()
        {
            InitializeComponent();
            LoadComboBoxData();
        }

        private void LoadComboBoxData()
        {
            DBEntities.ResetContext();
            AdresCb.ItemsSource = DBEntities.GetContext().Adress.ToList();
            GenderCb.ItemsSource = DBEntities.GetContext().Gender.ToList();
            UserCb.ItemsSource = DBEntities.GetContext().User.ToList();
        }

        private void PhotoBTN_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                    BitmapImage imageSource = new BitmapImage();
                    imageSource.BeginInit();
                    imageSource.CacheOption = BitmapCacheOption.OnLoad;
                    imageSource.UriSource = new Uri(openFileDialog.FileName);
                    imageSource.EndInit();
                    JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(imageSource));
                    using (MemoryStream stream = new MemoryStream())
                    {
                        encoder.Save(stream);
                        FotoImageData = stream.ToArray();
                    }
                    PhotoIMG.Source = imageSource;
                    IsFotoLoaded = true;
                }
            }
            catch
            {
                ClassFolder.MBClass.ErrorMB("Ошибка при загрузке изображения");
            }
        }

        private void PasportBTN_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                    BitmapImage imageSource = new BitmapImage();
                    imageSource.BeginInit();
                    imageSource.CacheOption = BitmapCacheOption.OnLoad;
                    imageSource.UriSource = new Uri(openFileDialog.FileName);
                    imageSource.EndInit();
                    JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(imageSource));
                    using (MemoryStream stream = new MemoryStream())
                    {
                        encoder.Save(stream);
                        FotoImageData2 = stream.ToArray();
                    }
                    PhotoIMG.Source = imageSource;
                    IsFotoLoaded = true;
                }
            }
            catch
            {
                ClassFolder.MBClass.ErrorMB("Ошибка при загрузке изображения");
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                DBEntities.GetContext().Employer.Add(new Employer()
                {
                    Name = NameTb.Text,
                    Surname = SurnameTb.Text,
                    Therdname = TherdnameTb.Text,
                    Phone = PhoneTb.Text,
                    IdAdress = Int32.Parse(AdresCb.SelectedValue.ToString()),
                    IdGender = Int32.Parse(GenderCb.SelectedValue.ToString()),
                    IdUser = Int32.Parse(UserCb.SelectedValue.ToString()),
                    Email = EmailTb.Text,
                    PhotoPasport = FotoImageData2,
                    Photo = FotoImageData
                });
                DBEntities.GetContext().SaveChanges();
                ClassFolder.MBClass.ShowMesagePopup("Успешно", Application.Current.MainWindow);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при добавлении операции: {ex.Message}");
            }
        }
    }
}
