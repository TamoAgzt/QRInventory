using AForge.Video;
using AForge.Video.DirectShow;
using System;
using QRInventory.Classes;
using SQLite;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
using System.Drawing.Imaging;
using ZXing;
using ZXing.Common;

namespace QRInventory.windows
{
    /// <summary>
    /// Interaction logic for scan.xaml
    /// </summary>
    public partial class scan : Window
    {
        private FilterInfoCollection CaptureDevice;
        private VideoCaptureDevice FinalFrame;

        public scan()
        {
            InitializeComponent();

            CaptureDevice = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo Device in CaptureDevice)
                CbCameras.Items.Add(Device.Name);
            CbCameras.SelectedIndex = 0;
            FinalFrame = new VideoCaptureDevice();
            
        }

        private void HOME_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void SCAN_Click(object sender, RoutedEventArgs e)
        {
            FinalFrame = new VideoCaptureDevice(CaptureDevice[CbCameras.SelectedIndex].MonikerString);
            FinalFrame.NewFrame += new NewFrameEventHandler(FinalFrame_NewFrame);
            FinalFrame.Start();
        }

        private void FinalFrame_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap bmp = (Bitmap)eventArgs.Frame.Clone();
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    string test = FindQrCodeInImage(bmp);
                    if (test != null) 
                        QRCodeCheck(test);
                    imgQR.Source = ImageSource(bmp);

                });
            }
            catch (Exception)
            {
                Environment.Exit(0);
            }
        }

        private void QRCodeCheck(string qrCodeImage)
        {
            if (FinalFrame != null)
            {
                FinalFrame.SignalToStop();
                FinalFrame.NewFrame -= new NewFrameEventHandler(FinalFrame_NewFrame);
                FinalFrame = null;
            }

            // Use the Inventory class method to get the entry based on QRCodeImage
            Inventory inventoryItem = Inventory.GetByQRCodeImage(qrCodeImage);

            if (inventoryItem != null)
            {
                // You have found the item in the database based on QRCodeImage
                // You can now use the 'inventoryItem' object to access the properties
                // For example, to get the Name_Full, Amount, and QRCodeImage:
                string itemName = inventoryItem.Name_Full;
                string itemAmount = inventoryItem.Amount;
                byte[] itemQRCodeImage = inventoryItem.QRCodeImage;

                // Do whatever you need with the retrieved data
                // For example, display it in your UI or perform further actions
                tbItemName.Text = itemName;
                tbAmount.Text = itemAmount;
            }
            else
            {
                // Item not found in the database
                MessageBox.Show("Item not found.");
            }
        }

        private string FindQrCodeInImage(Bitmap bmp)
        {
            var source = new BitmapLuminanceSource(bmp);
            var bitmap = new BinaryBitmap(new HybridBinarizer(source));
            var result = new MultiFormatReader().decode(bitmap);

            if (result == null)
            {
                return null;
            }

            //show the found qr code in the app
            return result.Text;
        }

        private ImageSource ImageSource(Bitmap bmp)
        {
            {
                using (MemoryStream memory = new MemoryStream())
                {
                    bmp.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                    memory.Position = 0;
                    BitmapImage bitmapimage = new BitmapImage();
                    bitmapimage.BeginInit();
                    bitmapimage.StreamSource = memory;
                    bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapimage.EndInit();
                    return bitmapimage;
                }
            }
        }

    }
}

