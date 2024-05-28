using ControlzEx.Standard;
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
using VKRTalalaev.ClassFolder;
using VKRTalalaev.DataFolder;

namespace VKRTalalaev.PageFolder
{
    /// <summary>
    /// Логика взаимодействия для EditEmployer.xaml
    /// </summary>
    public partial class EditEmployer : Page
    {
        private byte[] FotoImageData;
        private byte[] FotoImageData2;
        Employer Emp;
        public EditEmployer(Employer Emp)
        {
            InitializeComponent();
            DataContext = Emp;
            this.Emp = Emp;
            DBEntities.ResetContext();
            LoadComboBoxData();
            LoadCounterpartyPhoto(Emp.IdEmployer);
        }

        private void LoadComboBoxData()
        {
            DBEntities.ResetContext();
            AdresCb.ItemsSource = DBEntities.GetContext().Adress.ToList();
            GenderCb.ItemsSource = DBEntities.GetContext().Gender.ToList();
            UserCb.ItemsSource = DBEntities.GetContext().User.ToList();
        }

        private void LoadCounterpartyPhoto(int counterpartyId)
        {
            DBEntities.ResetContext();
            using (var context = DBEntities.GetContext())
            {
                try
                {
                    var counterparty = context.Employer
                                               .FirstOrDefault(c => c.IdEmployer == counterpartyId);
                    if (counterparty != null && counterparty.Photo != null)
                    {
                        BitmapImage image = new BitmapImage();
                        using (MemoryStream stream = new MemoryStream(counterparty.Photo))
                        {
                            image.BeginInit();
                            image.CacheOption = BitmapCacheOption.OnLoad;
                            image.StreamSource = stream;
                            image.EndInit();
                        }
                        PhotoIMG.Source = image;
                        FotoImageData = counterparty.Photo;
                    }
                    else
                    {
                        ClassFolder.MBClass.ErrorMB("Фото не найдено или повреждено");
                    }
                }
                catch (Exception ex)
                {
                    ClassFolder.MBClass.ErrorMB("Ошибка при загрузке фото: " + ex.Message);
                }
            }
        }

        private void LoadCounterpartyPhotoPasport(int counterpartyId)
        {
            DBEntities.ResetContext();
            using (var context = DBEntities.GetContext())
            {
                try
                {
                    var counterparty = context.Employer
                                               .FirstOrDefault(c => c.IdEmployer == counterpartyId);
                    if (counterparty != null && counterparty.PhotoPasport != null)
                    {
                        BitmapImage image = new BitmapImage();
                        using (MemoryStream stream = new MemoryStream(counterparty.PhotoPasport))
                        {
                            image.BeginInit();
                            image.CacheOption = BitmapCacheOption.OnLoad;
                            image.StreamSource = stream;
                            image.EndInit();
                        }
                        PhotoIMG.Source = image;
                        FotoImageData2 = counterparty.PhotoPasport;
                    }
                    else
                    {
                        ClassFolder.MBClass.ErrorMB("Фото не найдено или повреждено");
                    }
                }
                catch (Exception ex)
                {
                    ClassFolder.MBClass.ErrorMB("Ошибка при загрузке фото: " + ex.Message);
                }
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                int index = AdresCb.SelectedIndex + 1;
                int index2 = UserCb.SelectedIndex + 1;
                int index3 = GenderCb.SelectedIndex + 1;
                DBEntities.ResetContext();
                Emp = DBEntities.GetContext().Employer
                    .FirstOrDefault(u => u.IdEmployer == Emp.IdEmployer);
                Emp.IdAdress = index;
                Emp.IdUser = index2;
                Emp.IdGender = index3;
                Emp.Name = NameTb.Text;
                Emp.Surname = SurnameTb.Text;
                Emp.Therdname = TherdnameTb.Text;
                Emp.Phone = PhoneTb.Text;
                Emp.Email = EmailTb.Text;
                Emp.PhotoPasport = FotoImageData2;
                Emp.Photo = FotoImageData;
                DBEntities.GetContext().SaveChanges();
                MBClass.ShowMesagePopup("Успешно", Application.Current.MainWindow);

            }
            catch (Exception ex)
            {
                MBClass.ShowErrorPopup(ex.Message, Application.Current.MainWindow);
            }
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
                    
                }
            }
            catch
            {
                ClassFolder.MBClass.ErrorMB("Ошибка при загрузке изображения");
            }
        }
    }
}
