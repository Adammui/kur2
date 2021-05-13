using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GraphicTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void holst_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new Graphic();
        }

        private void gallery_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new Gallery();
        }

        private void lang_Click(object sender, RoutedEventArgs e)
        {

        }

        private void userchange_Click(object sender, RoutedEventArgs e)
        {
            Message ms = new Message();

            if (ms.ShowDialog() == true)
            { 
                Login a = new Login();
                a.Show();
                this.Close();
                
            }
        }
    }

}
