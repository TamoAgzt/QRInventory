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
        public string Brand { get; set; }
        public string Item_Type { get; set; }
        public string Description { get; set; }
        public string Name_Short { get; set; }
        public int Amount { get; set; }

        public override string ToString()
        {
            return $"{Name_Full} - {Brand} - {Item_Type} - {Description} - {Name_Short} - {Amount}";
        }
    }
}
