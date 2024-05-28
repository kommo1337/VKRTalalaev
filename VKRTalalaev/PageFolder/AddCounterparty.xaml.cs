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
using VKRTalalaev.DataFolder;

namespace VKRTalalaev.PageFolder
{
    /// <summary>
    /// Логика взаимодействия для AddCounterparty.xaml
    /// </summary>
    public partial class AddCounterparty : Page
    {
        private bool IsFotoLoaded;
        private byte[] FotoImageData;

        public AddCounterparty()
        {
            InitializeComponent();
            LoadComboBoxData();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
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

        private void LoadComboBoxData()
        {

            AdresCb.ItemsSource = DBEntities.GetContext().Adress.ToList();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                DBEntities.GetContext().Counterparty.Add(new Counterparty()
                {
                    CounterpartyName = CounterPartyNameTb.Text,
                    INN = int.Parse(NameOperationTb.Text),
                    KPP = int.Parse(KPPTb.Text),
                    OGRN = int.Parse(OgrnipTb.Text),
                    IdAdress = Int32.Parse(AdresCb.SelectedValue.ToString()),
                    FinancialAccaunt = int.Parse(FinAccTb.Text),
                    PasportPhoto= FotoImageData
            });
                DBEntities.GetContext().SaveChanges();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при добавлении операции: {ex.Message}");
            }
        }
    }
}
