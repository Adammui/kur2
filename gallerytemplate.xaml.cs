using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace GraphicTool
{
    /// <summary>
    /// Логика взаимодействия для gallerytemplate.xaml
    /// </summary>
    public partial class gallerytemplate : UserControl
    {
        user logged;
        public gallerytemplate(picture p, user u)
        {
            InitializeComponent();
            
             iddd.Text = p.painting_id.ToString();
             owner.Text = p.username;
             description.Text = p.descript;
             descrip.Text = p.descript;
             datet.Text = p.date_created.ToString();
             logged = u;
             if (logged.role == "admin")
                 owner.IsEnabled = true;
             else owner.IsEnabled = false;
            
        }

        private void del_Click(object sender, RoutedEventArgs e)
        {
            try{
                using (painDB_Entities db = new painDB_Entities())
                {
                    var pics = db.pictures;
                    foreach (picture p in pics)
                    { 
                        if(Convert.ToInt32(iddd.Text)==p.painting_id)
                            db.pictures.Remove(p);
                        string file = Environment.CurrentDirectory.ToString() + $"\\Resources\\Data\\stroke_copies\\{p.descript}.bmp";
                        File.Delete(file);
                    }
                      db.SaveChanges();
                    
                    del.IsEnabled = false;
                    del.Content = "✓";
                    continue_.IsEnabled = false;
                    edit.IsEnabled = false;
                   
                }
            } catch (Exception ex) { Message a = new Message(ex.Message); a.ShowDialog(); }
        }

        private void edit_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                using (painDB_Entities db = new painDB_Entities())
                {
                
                    var pics = db.pictures;
                    foreach (picture p in pics)
                    {
                        if (Convert.ToInt32(iddd.Text) == p.painting_id)
                        {
                            p.descript = description.Text;
                            p.username = owner.Text;
                        }
                    }
                    db.SaveChanges();

                    del.IsEnabled = false;
                    del.Content = "✓";
                    continue_.IsEnabled = false;
                    edit.IsEnabled = false;

               
                }
            } catch (Exception ex) { Message a = new Message(ex.Message); a.ShowDialog(); }
        }

        private void continue_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow main = new MainWindow(logged);
                main.MainFrame.Content = new Graphic(logged); ///вставить рисунок туда 
                main.Show();
                foreach (Window window in Application.Current.Windows)
                {
                    if (window != main)
                    {
                        window.Close();
                    }
                }
            } catch (Exception ex) { Message exept = new Message(ex.Message); exept.ShowDialog(); }
        }

        private void show_Click(object sender, RoutedEventArgs e)
        {
            if (pattern.Visibility == Visibility.Hidden)
                pattern.Visibility = Visibility.Visible;
            else if (pattern.Visibility == Visibility.Visible)
                pattern.Visibility = Visibility.Hidden;

        }
    }
}



/*      public picture() { }
        public picture( string username , byte[] painting, DateTime date_created ,string descript, string path) 
        {
            this.username= username;
            this.painting= painting; 
            this.date_created= date_created;
            this.descript=descript;
            this.path=path;
        }



        public user()
        {
            this.pictures = new HashSet<picture>();
        }
        public user(string u,string p, string role)
        {
            username = u;
            passw = p;
            this.role = role;
            this.pictures = new HashSet<picture>();
        }
*/