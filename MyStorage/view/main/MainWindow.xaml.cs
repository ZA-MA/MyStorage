using System;
using System.Collections.Generic;
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


namespace MyStorage.view.main
{

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static int user_access;
        public static int num_page = 1;
        public string user_name;
        public static int id_user;
        public MainWindow(int access_user, string user_name, int user_id)
        {
            InitializeComponent();
            Switcher(1);
            user_access = access_user;
            id_user = user_id;
            User_name.Text = user_name.ToString();
            /*if(user_access == 1) { Tab_item_1.Visibility = Visibility.Collapsed; }*/
            if(user_access == 2) { Tab_item_2.Visibility = Visibility.Collapsed; }
        }


        public void Switcher(int page)
        {
            switch (page)
            {
                case 1:
                    Content.Children.Clear();
                    Content.Children.Add(new pages.ProductsPage());
                    break;
                case 2:
                    Content.Children.Clear();
                    Content.Children.Add(new pages.SalesPage());
                    break;
                case 3:
                    Content.Children.Clear();
                    Content.Children.Add(new pages.CabinetPage(this));
                    break;
            }
           
        }

        private void Tab_item_1_Selected(object sender, RoutedEventArgs e)
        {
            Switcher(1);
        }

        private void Tab_item_2_Selected(object sender, RoutedEventArgs e)
        {
            Switcher(2);
        }

        private void Tab_item_3_Selected(object sender, RoutedEventArgs e)
        {
            Switcher(3);
        }
    }
}
