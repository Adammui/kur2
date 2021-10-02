using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GraphicTool
{
    /// <summary>
    /// Логика взаимодействия для Gallery.xaml
    /// </summary>
    public partial class Gallery : Page
    {
        public user logged;
        public int counter;
        public Gallery(user ab)
        {
            InitializeComponent();
            logged = ab;
            counter = 3;

            try { 
                using (painDB_Entities db = new painDB_Entities())
                {
                    var pics = db.pictures;
                    foreach (picture p in pics)
                    {
                        if (p.username == logged.username)
                        {
                            show(p,true);
                        }
                    }
                    foreach (picture p in pics)
                    {
                        if (p.username != logged.username)
                        {
                            show(p, false);
                        }
                    }
                }
            }
            catch (Exception ex) { Message exept = new Message(ex.Message); exept.ShowDialog(); }
        }
        public Gallery(user ab, bool a)
        {
            InitializeComponent();
            logged = ab;
            counter = 3;
            b1.Visibility = Visibility.Hidden;
            b2.Visibility = Visibility.Hidden;

            try
            {
                using (painDB_Entities db = new painDB_Entities())
                {
                    var users = db.users;
                    foreach (user u in users)
                    {
                        UserControl1 b = new UserControl1(u);
                        if (counter % 3 == 0)
                            this.stack_to_add.Children.Add(b);
                        else if (counter % 2 == 0)
                            this.stack_to_add1.Children.Add(b);
                        else if (counter % 1 == 0)
                            this.stack_to_add2.Children.Add(b);
                        counter -= 1;
                        if (counter == 0)
                            counter = 3;
                    }
                }
            }
            catch (Exception ex) { Message exept = new Message(ex.Message); exept.ShowDialog(); }
        }
        void show(picture p, bool color)
        {
            gallerytemplate a = new gallerytemplate(p, logged);
            if (color == true) a.usersitem.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#992A1C64"));
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = new MemoryStream(p.painting);
            bi.EndInit();
            a.img.Source = bi;
            if (counter % 3 == 0)
                this.stack_to_add.Children.Add(a);
            else if (counter % 2 == 0)
                this.stack_to_add1.Children.Add(a);
            else if (counter % 1 == 0)
                this.stack_to_add2.Children.Add(a);
            counter -= 1;
            if (counter == 0)
                counter = 3;
        }

        private void sortName(object sender, RoutedEventArgs e)
        {
            this.stack_to_add2.Children.Clear();
            this.stack_to_add1.Children.Clear();
            this.stack_to_add.Children.Clear();
            counter = 3;
            using (painDB_Entities db = new painDB_Entities())
            {
                var pics = db.pictures.OrderBy(s => s.descript);
                foreach (picture p in pics)
                {
                    if (p.username == logged.username)
                    {
                        show(p, true);
                    }
                }
                foreach (picture p in pics)
                {
                    if (p.username != logged.username)
                    {
                        show(p, false);
                    }
                }
            }
        }
        private void sortDate(object sender, RoutedEventArgs e)
        {
            this.stack_to_add2.Children.Clear();
            this.stack_to_add1.Children.Clear();
            this.stack_to_add.Children.Clear();
            counter = 3;
            using (painDB_Entities db = new painDB_Entities())
            {
                var pics = db.pictures.OrderByDescending(s => s.date_created);
                foreach (picture p in pics)
                {
                    if (p.username == logged.username)
                    {
                        show(p, true);
                    }
                }
                foreach (picture p in pics)
                {
                    if (p.username != logged.username)
                    {
                        show(p, false);
                    }
                }
            }
        }
    }
}
