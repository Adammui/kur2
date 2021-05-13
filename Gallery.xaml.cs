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
    /// Логика взаимодействия для Gallery.xaml
    /// </summary>
    public partial class Gallery : Page
    {
        public Gallery()
        {
            InitializeComponent();
            for (int i = 0; i < 4; i++)
            {
                gallerytemplate a = new gallerytemplate();
                this.stack_to_add.Children.Add(a);
            }
            for (int j = 0; j < 4; j++)
            {
                gallerytemplate a = new gallerytemplate();
                this.stack_to_add1.Children.Add(a);
            }
        }
    }
}
