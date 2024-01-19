using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace MyStorage.view.main.pages
{
    /// <summary>
    /// Логика взаимодействия для AddProduct.xaml
    /// </summary>
    public partial class AddEditProduct : Window
    {
        ProductsPage _owner;
        private Products _currentProduct = new Products();
        public AddEditProduct(Products selectedProduct, ProductsPage owner)
        {
            InitializeComponent();

            _owner = owner;
            

            

            if (selectedProduct != null)
            {
                LabelPage.Content = $"Редактирование товара № {selectedProduct.id}";
                _currentProduct = selectedProduct;
            }
            else
                LabelPage.Content = "Добавление нового товара";

            DataContext = _currentProduct;
            CB_Type.ItemsSource = StorageEntities.GetContext().Types.ToList();
            CB_Gender.ItemsSource = StorageEntities.GetContext().Genders.ToList();
            CB_Size.ItemsSource = StorageEntities.GetContext().Sizes.ToList();
            CB_Storage.ItemsSource = StorageEntities.GetContext().Storages.ToList();



        }
        

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Btn_save_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(_currentProduct.name))
                errors.Append("Укажите название\n");
            if (_currentProduct.Types == null)
                errors.Append("Выберите тип\n");
            if (_currentProduct.Genders == null)
                errors.Append("Выберите М/Ж\n");
            if (_currentProduct.Sizes == null)
                errors.Append("Выберите размер\n");
            if (string.IsNullOrWhiteSpace(_currentProduct.color))
                errors.Append("Укажите цвет\n");
            if (_currentProduct.quantity == null)
                errors.Append("Укажите количество\n");
            if (_currentProduct.quantity < 0)
                errors.Append("Неверно указано количество\n");
            if (_currentProduct.price_unit == null)
                errors.Append("Укажите цену за ед.\n");
            if (_currentProduct.price_unit <=0)
                errors.Append("Неверно указана цена за ед.\n");
            if (_currentProduct.Storages == null)
                errors.Append("Выберите склад\n");

            if(errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_currentProduct.id == 0)
                StorageEntities.GetContext().Products.Add(_currentProduct);

            try
            {
                StorageEntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена");
                _owner.UpdateData();
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
