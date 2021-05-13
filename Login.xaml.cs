using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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
using System.Windows.Shapes;

namespace GraphicTool
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public String username;
        public String password;
        List<user> listofusers= new List<user> { };
        public Login()
        {
            InitializeComponent();
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void login_Click(object sender, RoutedEventArgs e)
        {
            /* if (usernamecontainer.Text != String.Empty && passwdcontainer.Password != String.Empty)
             {
                 username = usernamecontainer.Text.ToString();
                 password = passwdcontainer.Password.ToString();
                 string cmdlogin = $"select * from users where username='{username}' ";
                 using (SqlConnection connection = new SqlConnection(connectionString))
                 {
                     listofusers.Clear();
                     SqlCommand command = new SqlCommand(cmdlogin, connection);
                     connection.Open();
                     using (SqlDataReader oReader = command.ExecuteReader())
                     {
                         while (oReader.Read())
                         {
                             listofusers.Add(new user(oReader["username"].ToString(), oReader["passw"].ToString()));
                         }

                     }
                     connection.Close();

                 }
                 if (listofusers.Count() == 1)
                 {
                     foreach (user u in listofusers)
                     {
                         if (u.Password == password)
                         {

                             MainWindow a = new MainWindow();
                             a.Show();
                             this.Close();
                         }
                         else { Message a = new Message("Неверный пароль"); a.ShowDialog(); }
                     }

                 }
                 else
                 {
                     Message a = new Message("Такого пользователя нет. Зарегистрируйтесь"); a.ShowDialog();
                 }
             }
             else { Message a = new Message("Логин и пароль должны быть заполнены!"); a.ShowDialog(); }
            */
            if (usernamecontainer.Text != String.Empty && passwdcontainer.Password != String.Empty)
            {
                username = usernamecontainer.Text.ToString();
                password = passwdcontainer.Password.ToString();
                using (Model1 db = new Model1())
                {
                    var users = db.users;
                    foreach (user u in users)
                    {
                        if (u.username==username)
                        listofusers.Add(u);
                    }
                    if (listofusers.Count() == 1)
                    {
                        if (listofusers[0].passw == password)
                        {
                            MainWindow a = new MainWindow();
                            a.Show();
                            this.Close();
                        }
                            else { Message a = new Message("Неверный пароль"); a.ShowDialog(); }
                    }
                    else
                    {
                        Message a = new Message("Такого пользователя нет. Зарегистрируйтесь"); a.ShowDialog();
                    }
                }
            }
            else { Message a = new Message("Логин и пароль должны быть заполнены!"); a.ShowDialog(); }

        }

        private void register_Click(object sender, RoutedEventArgs e)
        {
            if (usernamecontainer.Text != String.Empty && passwdcontainer.Password != String.Empty)
            {
                username = usernamecontainer.Text.ToString();
                password = passwdcontainer.Password.ToString();
                string cmdregister = $"insert into users(username,passw) values('{username}', '{password}')";
            }
           
        }
    }
}
