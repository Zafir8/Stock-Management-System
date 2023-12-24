using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDAM2_Stock_Management_App
{
    public class Warehouse
    {
        private string ConnectionString = @"Data Source=(local)\SQLEXPRESS; Initial Catalog=StockManagementDB; Integrated Security=True";

        public string[] AddNewItem(StockItem item)
        {
            string[] result = new string[2];  // Initialize an array to hold the result message and transaction details

            using (SqlConnection sqlCon = new SqlConnection(ConnectionString))
            {
                try
                {
                    if (sqlCon.State == ConnectionState.Closed)
                    {
                        sqlCon.Open();
                    }

                    string query = "SELECT COUNT(*), Name FROM StockItems WHERE StockCode = @StockCode GROUP BY Name";
                    SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@StockCode", item.StockCode);
                    SqlDataReader reader = sqlCmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        int count = reader.GetInt32(0);
                        string existingName = reader.GetString(1);

                        reader.Close();

                        if (existingName != item.Name)
                        {
                            result[0] = "Error: The entered stock name does not match the existing stock name.";
                            return result;
                        }
                        else
                        {
                            query = "UPDATE StockItems SET Quantity = Quantity + @Quantity WHERE StockCode = @StockCode";
                            sqlCmd = new SqlCommand(query, sqlCon);
                            sqlCmd.Parameters.AddWithValue("@StockCode", item.StockCode);
                            sqlCmd.Parameters.AddWithValue("@Quantity", item.Quantity);
                            sqlCmd.ExecuteNonQuery();
                            AddTransactionLog(sqlCon, "Item already exists, stock updated", item);
                            result[0] = "Item already exists, quantity updated successfully.";
                            result[1] = $"Transaction Time: {DateTime.Now.ToString("dd/MM/yyyy HH:mm")} \nAction: Item already exists, stock updated. \nStockCode: {item.StockCode} \nQuantity Added: {item.Quantity}";
                        }
                    }
                    else
                    {
                        reader.Close();

                        query = "INSERT INTO StockItems (StockCode, Name, Quantity) VALUES (@StockCode, @Name, @Quantity)";
                        sqlCmd = new SqlCommand(query, sqlCon);
                        sqlCmd.Parameters.AddWithValue("@StockCode", item.StockCode);
                        sqlCmd.Parameters.AddWithValue("@Name", item.Name);
                        sqlCmd.Parameters.AddWithValue("@Quantity", item.Quantity);
                        sqlCmd.ExecuteNonQuery();
                        AddTransactionLog(sqlCon, "Item added", item);
                        result[0] = "Item added successfully.";
                        result[1] = $"Transaction Time: {DateTime.Now.ToString("dd/MM/yyyy HH:mm")} \nAction: Item added \nStockCode: {item.StockCode} \nQuantity Added: {item.Quantity}";
                    }

                    return result;
                }
                catch (Exception ex)
                {
                    result[0] = $"An error occurred: {ex.Message}";
                    return result;
                }
            }
        }


        private void AddTransactionLog(SqlConnection sqlCon, string action, StockItem item)
        {
            string query = "INSERT INTO TransactionLog (TransactionTime, Action, StockCode, ItemName, QuantityChanged, NewQuantity) VALUES (@TransactionTime, @Action, @StockCode, @ItemName, @QuantityChanged, @NewQuantity)";
            SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
            sqlCmd.Parameters.AddWithValue("@TransactionTime", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            sqlCmd.Parameters.AddWithValue("@Action", action);
            sqlCmd.Parameters.AddWithValue("@StockCode", item.StockCode);
            sqlCmd.Parameters.AddWithValue("@ItemName", item.Name);
            sqlCmd.Parameters.AddWithValue("@QuantityChanged", item.Quantity);

            // Fetch the new quantity after update or insert
            // This could also be done in the AddNewItem method, depending on your choice of flow.
            string fetchNewQuantityQuery = "SELECT Quantity FROM StockItems WHERE StockCode = @StockCode";
            SqlCommand fetchCmd = new SqlCommand(fetchNewQuantityQuery, sqlCon);
            fetchCmd.Parameters.AddWithValue("@StockCode", item.StockCode);
            int newQuantity = Convert.ToInt32(fetchCmd.ExecuteScalar());

            sqlCmd.Parameters.AddWithValue("@NewQuantity", newQuantity);
            sqlCmd.ExecuteNonQuery();
        }

    }
}
