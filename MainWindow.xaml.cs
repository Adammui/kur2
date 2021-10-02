using System;
using System.Windows;
namespace GraphicTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       user logged ;
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Content = new Gallery(logged);
            if (logged.role == "admin") manage.Visibility = Visibility.Visible;
        }

        public MainWindow(user a)
        {
            try
            {
                InitializeComponent();
                logged = a;
                user.Text = logged.username;
                MainFrame.Content = new Gallery(logged);
                if (logged.role == "admin") manage.Visibility = Visibility.Visible;
            }
            catch (Exception ex) { Message exept = new Message(ex.Message); exept.ShowDialog(); }
        }

        private void holst_Click(object sender, RoutedEventArgs e)
        {
             MainFrame.Content = new Graphic(logged); 
        }

        private void gallery_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new Gallery(logged);
        }


        private void userchange_Click(object sender, RoutedEventArgs e)
        {
            Message ms = new Message("Вы уверены, что хотите выйти из учетной записи?");

            if (ms.ShowDialog() == true)
            { 
                Login a = new Login();
                a.Show();
                this.Close();
                
            }
        }

        private void manage_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new Gallery(logged, true);
        }
    }

}
