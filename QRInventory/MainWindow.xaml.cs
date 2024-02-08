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
        List<Inventory> items;

        public MainWindow()
        {
            InitializeComponent();
            items = new List<Inventory>();
            showitems();
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

        public void showitems()
        {
            using (SQLiteConnection connection = new SQLiteConnection(App.dbPath))
            {
                connection.CreateTable<Inventory>();
                items = (connection.Table<Inventory>().ToList()).OrderBy(i => i.Item_ID).ToList();
            }

            if(items != null)
            {
                itemList.ItemsSource = items;
            }
        }
    }
}
