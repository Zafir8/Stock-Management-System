using System;

namespace SDAM2_Stock_Management_App
{
    public class TransactionLogEntry
    {
        // Properties
        public DateTime DateTime { get; set; }
        public string Action { get; set; }
        public string StockCode { get; set; }
        public string StockItemName { get; set; }
        public int QuantityAddedOrRemoved { get; set; }
        public int NewQuantity { get; set; }

        // Constructor
        public TransactionLogEntry(DateTime dateTime, string action, string stockCode, string stockItemName, int quantityAddedOrRemoved, int newQuantity)
        {
            DateTime = dateTime;
            Action = action;
            StockCode = stockCode;
            StockItemName = stockItemName;
            QuantityAddedOrRemoved = quantityAddedOrRemoved;
            NewQuantity = newQuantity;
        }
    }
}
