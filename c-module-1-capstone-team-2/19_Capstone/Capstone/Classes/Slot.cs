using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Capstone.Classes;


namespace Capstone.Classes
{
    public class Slot
    {
        public string IdNumber { get; set; }
        public string ItemName { get; set; }
        public decimal Price { get; set; }
        public string ItemCategory { get; set; }
        public int Quantity { get; set; }

        public Slot(string idNumber, string itemName, decimal price, string itemCategory, int quantity) 
        {
            IdNumber = idNumber;
            ItemName = itemName;
            Price = price;
            ItemCategory = itemCategory;
            Quantity = quantity;
        }
    }
}
