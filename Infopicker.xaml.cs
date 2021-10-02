using System;
using System.Windows;
using System.Windows.Input;
namespace GraphicTool
{
    /// <summary>
    /// Логика взаимодействия для Infopicker.xaml
    /// </summary>
    public partial class Infopicker : Window
    {
        public Infopicker(string message)
        {
            InitializeComponent();
            wintext.Text = message;
        }

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();

        }

        private void no_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }
    }
}
