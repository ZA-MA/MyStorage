using MyStorage.view.login;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyStorage.view.main.pages
{
    /// <summary>
    /// Логика взаимодействия для Page1.xaml
    /// </summary>
    public partial class CabinetPage : UserControl
    {
        MainWindow _owner;
        public CabinetPage(MainWindow owner)
        {
            _owner = owner;
            InitializeComponent();
            UpdateData();
        }
        
        
        

        public void UpdateData()
        {
            string connectionString = "Server=.\\SQLEXPRESS;Database=Storage;Trusted_Connection=True;";
            string queryString = $"SELECT Workers.*, Post.post_name FROM Workers INNER JOIN Post ON Workers.post_id = Post.id WHERE Workers.id = '{MainWindow.id_user}' ";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(queryString, connection);
                SqlDataReader reader = command.ExecuteReader();
                //command.Parameters.AddWithValue("@id", 1);
                
                while (reader.Read()) // построчно считываем данные
                {
                    L_name.Content = reader["name"];
                    L_post.Content = reader["post_name"];
                    L_birthday.Content = reader["birthdate"].ToString().Split(' ')[0];
                    L_telephone.Content = reader["telephone"];
                }
                reader.Close();
                
            }

        }

        private void Btn_Exit_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            MainWindow.id_user = 0;
            MainWindow.user_access = 0;
            _owner.Close();
            
        }
    }
}
