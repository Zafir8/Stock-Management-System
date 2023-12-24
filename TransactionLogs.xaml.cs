using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace SDAM2_Stock_Management_App
{
    /// <summary>
    /// Interaction logic for TransactionLogs.xaml
    /// </summary>
    public partial class TransactionLogs : Window
    {
        public TransactionLogs()
        {
            InitializeComponent();
            SqlConnection sqlCon = new SqlConnection(@"Data Source=(local)\SQLEXPRESS; Initial Catalog=StockManagementDB; Integrated Security=True");

            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                {
                    sqlCon.Open();
                }

                SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT TransactionTime, Action, StockCode, ItemName, QuantityChanged, NewQuantity FROM TransactionLog", sqlCon);
                DataTable dt = new DataTable();
                sqlDa.Fill(dt);

                stockDataGrid.ItemsSource = dt.DefaultView;

                sqlCon.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }



        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Menu menu = new Menu();
            menu.Show();
            this.Close();
        }
    }
}
