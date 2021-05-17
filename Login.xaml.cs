using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace GraphicTool
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        // string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
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
            //реализация того же, но на ado.net
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
                 if (listofsers.Count() == 1)
                 {
                     foreach (user u in listofusers)
                     {
                         if (u.Password == password)
                         {

                             MainWindow a = new MainWindow();
                             a.Show();
                             this.Close();
                         }   else { Message a = new Message("Неверный пароль"); a.ShowDialog(); }
                     }
                 }
                 else
                 { Message a = new Message("Такого пользователя нет. Зарегистрируйтесь"); a.ShowDialog(); }
             }
             else { Message a = new Message("Логин и пароль должны быть заполнены!"); a.ShowDialog(); }
            */
            if (usernamecontainer.Text != String.Empty && passwdcontainer.Password != String.Empty)
            {
                try
                {
                    username = usernamecontainer.Text.ToString();
                    password = passwdcontainer.Password.ToString();
                    using (painDB_Entities db = new painDB_Entities())
                    {

                        var us = db.users;
                        foreach (user u in us)
                        {
                            if (u.username == username)
                            {
                                listofusers.Add(u);
                                if (u.passw == password)
                                {
                                    new MainWindow(u).Show();
                                    this.Close();

                                }
                                else { Message a = new Message("Неверный пароль"); a.ShowDialog(); }

                            }
                        }
                        if (listofusers.Count == 0) { Message a = new Message("Такого пользователя нет. Зарегистрируйтесь"); a.ShowDialog(); }
                    }
                } catch (Exception ex) { Message exept = new Message(ex.Message); exept.ShowDialog(); }
            }
            else { Message a = new Message("Логин и пароль должны быть заполнены!"); a.ShowDialog(); }
        }

        private void register_Click(object sender, RoutedEventArgs e)
        {
            if (usernamecontainer.Text != String.Empty && passwdcontainer.Password != String.Empty)
            {
                try
                {
                    username = usernamecontainer.Text.ToString();
                    password = passwdcontainer.Password.ToString();
                    using (painDB_Entities db = new painDB_Entities())
                    {
                        user u1 = new user { username = username, passw = password, role = "users" };
                        // добавление
                        var us = db.users;
                        if (us.Find(u1.username) == null)
                        {
                            db.users.Add(u1);
                            db.SaveChanges();// сохранение изменений
                            Message exept = new Message("Вы зарегистрированы. Попробуйте войти"); exept.ShowDialog();
                        }
                        else { Message exept = new Message("Такой пользователь уже есть"); exept.ShowDialog(); }

                    }
                }
                catch (Exception ex) { Message exept = new Message(ex.Message); exept.ShowDialog(); }
            }

            
        }
    }
}
