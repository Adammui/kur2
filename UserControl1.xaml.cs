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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GraphicTool
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        user logged;
        public UserControl1( user u)
        {
            InitializeComponent();
            logged = u;
            username.Text = u.username;
            passw.Text = u.passw;
        }

        private void Button_Click(object sender, RoutedEventArgs e) //edit
        {
            try
            {
                using (painDB_Entities db = new painDB_Entities())
                {
                    var pics = db.users;
                    foreach (user p in pics)
                    {
                        if (logged.username == p.username)
                            p.passw = passw.Text;
                    }
                    db.SaveChanges();

                } 
                edit.IsEnabled = false;
                    edit.Content = "✓";
            }
            catch (Exception ex) { Message a = new Message(ex.Message); a.ShowDialog(); }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e) //del
        {
            try { 
                using (painDB_Entities db = new painDB_Entities())
                {
                    var a = db.pictures;
                    foreach (picture pic in a)
                    {
                        if (logged.username == pic.username)
                            db.pictures.Remove(pic);
                    }
                db.SaveChanges();
                }
                using (painDB_Entities db = new painDB_Entities())
                {
                    var pics = db.users;
                    foreach (user p in pics)
                    {
                        if (logged.username== p.username)
                            db.users.Remove(p);
                    }
                    db.SaveChanges();

                }


                delet.IsEnabled = false;
                delet.Content = "✓";
                edit.IsEnabled = false;
            }
            catch (Exception ex) { Message a = new Message(ex.Message); a.ShowDialog(); }

        }
    }
}
