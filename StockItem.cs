using System;

namespace SDAM2_Stock_Management_App
{
    public class StockItem
    {
        // Properties
        public string StockCode { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }

        // Constructor
        public StockItem(string stockCode, string name, int quantity)
        {
            StockCode = stockCode;
            Name = name;
            Quantity = quantity;
        }

        // Add quantity to the stock item
        public void AddQuantity(int amount)
        {
            Quantity += amount;
        }

        // Remove quantity from the stock item
        public void RemoveQuantity(int amount)
        {
            if (Quantity >= amount)
            {
                Quantity -= amount;
            }
        }
    }
}
