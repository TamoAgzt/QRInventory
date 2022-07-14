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

namespace QRInventory
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ADD_Click(object sender, RoutedEventArgs e)
        {
            windows.add add = new windows.add();
            add.Show();
            this.Close();
        }

        private void SCAN_Click(object sender, RoutedEventArgs e)
        {
            windows.scan scan = new windows.scan();
            scan.Show();
            this.Close();
        }

        public void shorten()
        {
            string a1 = tbName.Text.Substring(0, 2).ToUpper();
            string a2 = tbName.Text.Substring(tbName.Text.Length - 1).ToUpper();
            string b1 = tbBrand.Text.Substring(0, 3).ToUpper();
            string b2 = tbBrand.Text.Substring(tbBrand.Text.Length - 1).ToUpper();
            tbShort.Text = a1 + a2 + " " + b1 + b2;
        }

        private void SAVE_Click(object sender, RoutedEventArgs e)
        {
            shorten();

            Inventory inv = new Inventory()
            {
                Name_Full = tbName.Text,
                Brand = tbBrand.Text,
                Item_Type = tbType.Text,
                Description = tbDescription.Text,
                Name_Short = tbShort.Text
            };

            using (SQLiteConnection connection = new SQLiteConnection(App.dbPath)){
                connection.CreateTable<Inventory>();
                connection.Insert(inv);
            }

            Close();
        }
    }
}
