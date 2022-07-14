using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
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
        public add()
        {
            InitializeComponent();
        }

        private void HOME_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }

        private void GENERATE_Click(object sender, RoutedEventArgs e)
        {
            BarcodeWriter bw = new BarcodeWriter();
            EncodingOptions eo = new EncodingOptions(){Width = 360, Height = 360, Margin = 0, PureBarcode = false};
            eo.Hints.Add(EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.H);
            bw.Renderer = new BitmapRenderer();
            bw.Options = eo;
            bw.Format = BarcodeFormat.QR_CODE;
            Bitmap bm = bw.Write(input.Text);
            Bitmap logo = new Bitmap(@"C:\Users\tamog\source\repos\QRInventory\QRInventory\TamoHelmet2BW320.png");
            Graphics gs = Graphics.FromImage(bm);
            gs.DrawImage(logo, new System.Drawing.Point(145, 145));
            imgQR.Source = imageSource(bm);


        }

        private ImageSource imageSource(Bitmap bm)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bm.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
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
