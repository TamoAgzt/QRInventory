using QRInventory.Classes;
using SQLite;
using System;
using System.Collections.Generic;
using System.Drawing;
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
using ZXing;
using ZXing.Common;
using ZXing.QrCode.Internal;
using ZXing.Rendering;

namespace QRInventory.windows
{
    /// <summary>
    /// Interaction logic for add.xaml
    /// </summary>
    public partial class add : Window
    {
        // Declare a byte array to store the QR code
        byte[] qrCodeByteArray;
        string StringUUID;

        public add()
        {
            InitializeComponent();

            string[] Units = {"Piece", "Liter", "Kilogram", "Meter"};
            foreach (string Unit in Units)
                tbUnit.Items.Add(Unit);
            tbUnit.SelectedIndex = 0;

            string[] Valutas = { "US$", "CA$", "€", "Ft" };
            foreach (string Valuta in Valutas)
                tbValuta.Items.Add(Valuta);
            tbValuta.SelectedIndex = 0;

            // Initialize event handler for ComboBox selection changed
            tbUnit.SelectionChanged += TbUnit_SelectionChanged;

            // Set default value for tbUnitReadOnly
            tbUnitReadOnly.Text = tbUnit.SelectedItem?.ToString();
        }

        private void TbUnit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Update tbUnitReadOnly when ComboBox selection changes
            tbUnitReadOnly.Text = tbUnit.SelectedItem?.ToString();
        }

        private void HOME_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }


        public void GenerateCode(out byte[] qrCodeByteArray)
        {
            BarcodeWriter barcodeWriter = new BarcodeWriter();
            EncodingOptions encodingOptions = new EncodingOptions() { Width = 300, Height = 300, Margin = 0, PureBarcode = false };
            encodingOptions.Hints.Add(EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.H);
            barcodeWriter.Renderer = new BitmapRenderer();
            barcodeWriter.Options = encodingOptions;
            barcodeWriter.Format = BarcodeFormat.QR_CODE;
            Bitmap bitMap = barcodeWriter.Write(StringUUID);

            // Convert the Bitmap to a byte array
            qrCodeByteArray = ImageToByteArray(bitMap);

            // Display the QR code image in the WPF Image control
            imgQR.Source = ImageSourceFromBitmap(bitMap);
        }

        private ImageSource ImageSourceFromBitmap(Bitmap bitMap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitMap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitMapImage = new BitmapImage();
                bitMapImage.BeginInit();
                bitMapImage.StreamSource = memory;
                bitMapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitMapImage.EndInit();
                return bitMapImage;
            }
        }

        private byte[] ImageToByteArray(Bitmap bitMap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitMap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }

        private void CHECK_Click(object sender, RoutedEventArgs e)
        {
            Guid UUID = Guid.NewGuid();
            StringUUID = UUID.ToString();
            tbUUID.Text = StringUUID;

            // Call the GenerateCode method with the out parameter
            GenerateCode(out qrCodeByteArray);

            // Now you can use the qrCodeByteArray as needed
        }

        private void SAVE_Click(object sender, RoutedEventArgs e)
        {

            Inventory inv = new Inventory()
            {
                Name_Full = tbName.Text,
                Manufacturer = tbManufacturer.Text,
                Item_Type = tbType.Text,
                Description = tbDescription.Text,
                ItemUUID = StringUUID,
                Amount = tbAmount.Text,
                MinAmount = tbMinAmount.Text,
                MaxAmount = tbMaxAmount.Text,
                Unit = tbUnit.Text,
                CostPerUnit = tbCost.Text,
                QRCodeImage = qrCodeByteArray,
                Valuta = tbValuta.Text,
                Store = tbStore.Text
            };

            using (SQLiteConnection connection = new SQLiteConnection(App.dbPath))
            {
                connection.CreateTable<Inventory>();
                connection.Insert(inv);
            }

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
