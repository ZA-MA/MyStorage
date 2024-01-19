using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace MyStorage.view.main.pages
{
    /// <summary>
    /// Логика взаимодействия для Products.xaml
    /// </summary>
    public partial class ProductsPage : UserControl
    {
        public int size_storage = 1000;
        public ProductsPage()
        {
            InitializeComponent();
            UpdateData();
            if (MainWindow.user_access == 1)
            {
                Btn_add.Visibility = Visibility.Collapsed;
                DG_btn.Visibility = Visibility.Collapsed;
                size_storage = 850;
            }
        }

        public void UpdateData()
        {
            
            StorageEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
            DataGrid.ItemsSource = StorageEntities.GetContext().Products.ToList();
        }

        public void resize()
        {

            //DG_btn.MinWidth = 0;
            //DG_btn.MaxWidth = DataGrid.ActualWidth - Col_1.ActualWidth - Col_2.ActualWidth - Col_3.ActualWidth - Col_4.ActualWidth - Col_5.ActualWidth - Col_6.ActualWidth - Col_7.ActualWidth - Col_8.ActualWidth - Col_9.ActualWidth;
            //Col_10.Visibility = Visibility.Hidden;
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {

            double width = DataGrid.ActualWidth - Col_1.ActualWidth - Col_2.ActualWidth - Col_3.ActualWidth - Col_4.ActualWidth - Col_5.ActualWidth - Col_6.ActualWidth - Col_7.ActualWidth - Col_8.ActualWidth - Col_9.ActualWidth - Col_10.ActualWidth;
            if (width > 0) { DG_btn.MinWidth = width; }
            else { DG_btn.MinWidth = 0; }
            if (DataGrid.ActualWidth > size_storage)
            {
                Col_10.Visibility = Visibility.Visible;
            }
            else
            {
                Col_10.Visibility = Visibility.Hidden;

            }
        }

        private void Btn_Edit_Click(object sender, RoutedEventArgs e)
        {
            AddEditProduct editProduct = new AddEditProduct((sender as Button).DataContext as Products, this);
            editProduct.ShowDialog();


        }

        private void Btn_Add_Click(object sender, RoutedEventArgs e)
        {
            AddEditProduct addProduct = new AddEditProduct(null, this);
            addProduct.Show();

        }



        private void Btn_del_Click(object sender, RoutedEventArgs e)
        {
            var prodDel = DataGrid.SelectedItems.Cast<Products>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить этот товар?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    StorageEntities.GetContext().Products.RemoveRange(prodDel);
                    StorageEntities.GetContext().SaveChanges();

                    UpdateData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void Btn_Search_Click(object sender, RoutedEventArgs e)
        {
            if (TB_Search.Text.Trim().Length > 0)
            {
                int number = -1;
                int.TryParse(TB_Search.Text, out number);
                string connectionString = "Server=.\\SQLEXPRESS;Database=Storage;Trusted_Connection=True;";
                string queryString = $"SELECT Products.*, Types.type_name, Genders.gender_name, Sizes.size_name, Storages.storage_name FROM Products INNER JOIN Types ON Products.type_id = Types.id INNER JOIN Genders ON Products.gender_id = Genders.id INNER JOIN Sizes ON Products.size_id = Sizes.id INNER JOIN Storages ON Products.storage_id = Storages.id  WHERE Products.id = '{number}' OR Products.name = '{TB_Search.Text}' OR Types.type_name = '{TB_Search.Text}' OR Genders.gender_name = '{TB_Search.Text}' OR Sizes.size_name = '{TB_Search.Text}' OR Products.color = '{TB_Search.Text}' OR Products.quantity = '{number}' OR Products.price_unit = '{number}'";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    connection.Open();

                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.CommandText = queryString;
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command)) // построчно считываем данные
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        DataGrid.ItemsSource = dt.DefaultView;
                    }
                    Col_3.Binding = new Binding("type_name");
                    Col_4.Binding = new Binding("gender_name");
                    Col_5.Binding = new Binding("size_name");
                    Col_9.Binding = new Binding("storage_name");


                }
            }
            else
            {
                
                Col_3.Binding = new Binding("Types.type_name");
                Col_4.Binding = new Binding("Genders.gender_name");
                Col_5.Binding = new Binding("Sizes.size_name");
                Col_9.Binding = new Binding("Storages.storage_name");
                UpdateData();
            }
        }

        private void TB_Search_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
