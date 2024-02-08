using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace QRInventory.Classes
{
    class Inventory
    {
        [PrimaryKey, AutoIncrement]
        public int Item_ID { get; set; }
        public string Name_Full { get; set; }
        public string Manufacturer { get; set; }
        public string Item_Type { get; set; }
        public string Description { get; set; }
        public string ItemUUID { get; set; }
        public string Amount { get; set; }
        public string MinAmount { get; set; }
        public string MaxAmount { get; set; }
        public string Unit{ get; set; }
        public string CostPerUnit { get; set; }
        public string Valuta { get; set; }
        public string Store { get; set; }

        public byte[] QRCodeImage { get; set; }
        //CreatedAt
        //UpdatedAt

        public override string ToString()
        {
            return $"{ItemUUID} - {Name_Full} - {Manufacturer} - {Item_Type} - {Description}- {Amount} {Unit} - {MinAmount} {Unit} - {MaxAmount} {Unit} - {Valuta} {CostPerUnit} - {Store}";
        }

        public static Inventory GetByQRCodeImage(string qrCodeText)
        {
            using (SQLiteConnection connection = new SQLiteConnection(App.dbPath))
            {
                // Convert the QR code string to a byte array
                byte[] qrCodeBytes = Encoding.UTF8.GetBytes(qrCodeText);

                // Retrieve all records from the table
                var allItems = connection.Table<Inventory>().ToList();

                // Filter the records in-memory using LINQ
                var inventoryItem = allItems.FirstOrDefault(item => item.QRCodeImage.SequenceEqual(qrCodeBytes));

                return inventoryItem;
            }
        }
    }
}
