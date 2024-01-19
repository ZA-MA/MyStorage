using MyStorage.view.main;
using System;
using System.Data.SqlClient;
using System.Windows;

namespace MyStorage.view.login
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Btn_entry_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Server=.\\SQLEXPRESS;Database=Storage;Trusted_Connection=True;";
            string queryString = $"SELECT Workers.id, Workers.login, Workers.pass, Workers.name, Post.access FROM Workers INNER JOIN Post ON Workers.post_id = Post.id WHERE Workers.login = '{Input_login.Text}' ";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(queryString, connection);
                SqlDataReader reader = command.ExecuteReader();
                //command.Parameters.AddWithValue("@id", 1);
                int id = 0;
                object login = null;
                object pass = null;
                string name = "";
                int access = 1;
                while (reader.Read()) // построчно считываем данные
                {
                    id = Convert.ToInt32(reader["id"]);
                    login = reader["login"];
                    pass = reader["pass"];
                    name = reader["name"].ToString();
                    access = Convert.ToInt32(reader["access"]);
                }
                reader.Close();
                if(Input_login.Text != string.Empty && Input_pass.Password != string.Empty) 
                { 
                
                    if (login != null)
                    {

                        if (Input_pass.Password.ToString() == pass.ToString())
                        {
                            MainWindow win2 = new MainWindow(access, name, id);
                            win2.Show();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Неверный пароль");
                        }
                        // использовать значение ячейки
                    }
                    else
                    {
                        MessageBox.Show("Такого пользователя с таким логином не существует");
                    }
                }
                else { MessageBox.Show("Вы не ввели логин или пароль"); }
            }
        }
    }
}
