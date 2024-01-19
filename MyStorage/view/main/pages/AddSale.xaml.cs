using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace MyStorage.view.main.pages
{
    /// <summary>
    /// Логика взаимодействия для AddProduct.xaml
    /// </summary>
    public partial class AddSale : Window
    {
        SalesPage _owner;
        private Sales _currentSale = new Sales();
        public AddSale(SalesPage owner)
        {
            InitializeComponent();
            _owner = owner;
            CB_Product.ItemsSource = StorageEntities.GetContext().Products.ToList();
        }
        

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Btn_save_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (CB_Product == null)
                errors.Append("Укажите название\n");
            if (TB_Quantity == null)
                errors.Append("Укажите количество\n");
            if (int.Parse(TB_Quantity.Text) <= 0)
                errors.Append("Неверно указано количество\n");


            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            string connectionString = "Server=.\\SQLEXPRESS;Database=Storage;Trusted_Connection=True;";
            string queryString = $"SELECT Products.id, Products.quantity, Products.price_unit FROM Products WHERE Products.name = '{CB_Product.Text}' ";
            int col = 0;
            int price = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(queryString, connection);
                SqlDataReader reader = command.ExecuteReader();
                
                while (reader.Read()) // построчно считываем данные
                {
                    _currentSale.product_id = Convert.ToInt32(reader["id"]);
                    col = Convert.ToInt32(reader["quantity"]);
                    price = Convert.ToInt32(reader["price_unit"]);
                }
                reader.Close();
                connection.Close();
            }
            _currentSale.date_time = DateTime.Now;
            _currentSale.worker_id = MainWindow.id_user;
            _currentSale.quantity = Convert.ToInt32(TB_Quantity.Text);
            _currentSale.price = Convert.ToInt32(TB_Quantity.Text) * price;
            col -= Convert.ToInt32(TB_Quantity.Text);
            StorageEntities.GetContext().Sales.Add(_currentSale);

            queryString = String.Format($"UPDATE Products SET quantity = {col} WHERE Products.name = '{CB_Product.Text}' ");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(queryString, connection);
                command.CommandText = queryString;
                command.ExecuteNonQuery();
                connection.Close();
            }

            try
            {
                StorageEntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена");
                _owner.UpdateData();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
