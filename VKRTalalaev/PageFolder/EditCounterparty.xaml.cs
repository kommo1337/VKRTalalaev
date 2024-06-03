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
    /// Логика взаимодействия для EditCounterparty.xaml
    /// </summary>
    public partial class EditCounterparty : Page
    {
        private byte[] FotoImageData;
        Counterparty user;

        public EditCounterparty(Counterparty user)
        {
            InitializeComponent();
            DataContext = user;
            this.user = user;
            DBEntities.ResetContext();
            LoadCounterpartyPhoto(user.IdCounterparty);
            LoadComboBoxData();
        }

        private void LoadComboBoxData()
        {
            DBEntities.ResetContext();
            AdresCb.ItemsSource = DBEntities.GetContext().Adress.ToList();
        }


        private void LoadCounterpartyPhoto(int counterpartyId)
        {
            using (var context = DBEntities.GetContext())
            {
                try
                {
                    var counterparty = context.Counterparty
                                               .FirstOrDefault(c => c.IdCounterparty == counterpartyId);
                    if (counterparty != null && counterparty.PasportPhoto != null)
                    {
                        BitmapImage image = new BitmapImage();
                        using (MemoryStream stream = new MemoryStream(counterparty.PasportPhoto))
                        {
                            image.BeginInit();
                            image.CacheOption = BitmapCacheOption.OnLoad;
                            image.StreamSource = stream;
                            image.EndInit();
                        }
                        PhotoIMG.Source = image;
                        FotoImageData = counterparty.PasportPhoto;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
                DBEntities.ResetContext(); // Сброс контекста перед созданием нового
                using (var context = DBEntities.GetContext())
                {
                    int index6 = AdresCb.SelectedIndex + 1;
                    var userId = user.IdCounterparty;

                    var userFromDb = context.Counterparty
                                             .FirstOrDefault(u => u.IdCounterparty == userId);

                    if (userFromDb != null)
                    {
                        userFromDb.IdAdress = index6;
                        userFromDb.INN = int.Parse(NameOperationTb.Text);
                        userFromDb.KPP = int.Parse(KPPTb.Text);
                        userFromDb.OGRN = int.Parse(OgrnipTb.Text);
                        userFromDb.FinancialAccaunt = int.Parse(FinAccTb.Text);
                        userFromDb.PasportPhoto = FotoImageData;

                        context.SaveChanges();
                        MBClass.ShowMesagePopup("Успешно", Application.Current.MainWindow);
                    }
                    else
                    {
                        MBClass.ShowMesagePopup("Пользователь не найден", Application.Current.MainWindow);
                    }
                }
            //}
            //catch (Exception ex)
            //{
            //    MBClass.ShowErrorPopup(ex.Message, Application.Current.MainWindow);
            //}

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
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
            catch (Exception ex)
            {
                ClassFolder.MBClass.ErrorMB("Ошибка при загрузке изображения: " + ex.Message);
            }
        }
    }



}
