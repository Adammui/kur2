using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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
    /// Логика взаимодействия для Graphic.xaml
    /// </summary>
    public partial class Graphic : Page
    {
        public Graphic()
        {
            InitializeComponent();
            ic.MouseUp += Strokes_StrokesChanged;
            Saves.Add(ic.Strokes.Clone());
        }
        StrokeCollection _added;
        StrokeCollection _removed;
        public List<StrokeCollection> Saves = new List<StrokeCollection> { }; //<
        public List<StrokeCollection> Redos = new List<StrokeCollection> { };     //>


        // Add an InkCanvas to the window, and allow the user to 
        // switch between using a green pen and a purple highlighter 
        // on the InkCanvas.
        private void WindowLoaded(object sender, EventArgs e)
        {
            
            /*inkCanvas1.Background = Brushes.DarkSlateBlue;
            inkCanvas1.DefaultDrawingAttributes.Color = Colors.SpringGreen;

            root.Children.Add(inkCanvas1);
            
            

            // Set up the DrawingAttributes for the pen.
            inkDA = new DrawingAttributes();
            inkDA.Color = Colors.SpringGreen;
            inkDA.Height = 5;
            inkDA.Width = 5;
            inkDA.FitToCurve = false;

            inkCanvas1.DefaultDrawingAttributes = inkDA;
            */

        }

        private void Strokes_StrokesChanged(object sender, MouseButtonEventArgs e)
        {
            if (delet == true)
            {   
                Redos.Clear();
                redo.IsEnabled = false;
                delet = false;
            }
            Saves.Add(ic.Strokes.Clone());

            if(Saves.Count() == 7) 
            {
                Saves.RemoveAt(0);
            }
            
            undo.IsEnabled = true;
            /*
            _added = e.Added;
            _removed = e.Removed;

            */
        }
        bool delet=false;
        private void Undo(object sender, RoutedEventArgs e)
        {

            /*handle = false;
             _removed = ic.Strokes.Clone();
             ic.Strokes.Clear();
             _added.RemoveAt(_added.Count-1);

            ic.Strokes.Add(_added);
            handle = true;
            */
            /* ic.Strokes.Remove(_added); //и он сразу записался в ремувед
             redo.IsEnabled = true;
             undo.IsEnabled = false;*/
            if (Saves.Count() > 1)
            {
                ic.Strokes.Clear();
                ic.Strokes.Add( Saves[Saves.Count() - 2]); //вставляю предпоследний элемент сэйва
                Redos.Add(Saves[Saves.Count() - 1]); //добавляю посл в сэйв справа
                Saves.RemoveAt(Saves.Count() - 1); //удаляю посл из сэйва слева\
                redo.IsEnabled = true;

                delet = true;
                if (Saves.Count() > 1)
                    undo.IsEnabled = true;
                else undo.IsEnabled = false;
            }
            
        }

        private void Redo(object sender, RoutedEventArgs e)
        {

            /* handle = false;
             ic.Strokes.Clear();
             ic.Strokes.Add(_removed);
             //ic.Strokes.Remove(_removed);
             handle = true;
            */
            /* ic.Strokes.Add(_removed);
             undo.IsEnabled = true;
             redo.IsEnabled = false;*/
            if (Redos.Count() > 0)
            {
                ic.Strokes.Clear();
                ic.Strokes.Add(Redos[Redos.Count()-1]);
                //Saves.Clear();
                Saves.Add(Redos[Redos.Count() - 1]);
                undo.IsEnabled = true;
                Redos.RemoveAt(Redos.Count()-1);

                delet = false;
                if (Redos.Count() > 0) 
                    redo.IsEnabled = true;
                else redo.IsEnabled = false;
            }
        }

        private void ic_StrokeCollected(object sender, InkCanvasStrokeCollectedEventArgs e)
        {
          
           
        }

        private void Page_KeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.Key == Key.Z)
            {
                Undo(sender, e);
            }
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.Key == Key.Y)
            {
                Redo(sender, e);
            }
        }

        private void SaveintoFile(object sender, RoutedEventArgs e)
        {
            

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                double width = ic.ActualWidth;
                double height = ic.ActualHeight;
                RenderTargetBitmap bmpCopied = new RenderTargetBitmap((int)Math.Round(width), (int)Math.Round(height), 96, 96, PixelFormats.Default);
                DrawingVisual dv = new DrawingVisual();
                using (DrawingContext dc = dv.RenderOpen())
                {
                    VisualBrush vb = new VisualBrush(ic);
                    dc.DrawRectangle(vb, null, new Rect(new System.Windows.Point(), new System.Windows.Size(width, height)));
                }
                bmpCopied.Render(dv);
                System.Drawing.Bitmap bitmap;
                using (MemoryStream outStream = new MemoryStream())
                {
                    BitmapEncoder enc = new BmpBitmapEncoder();
                    enc.Frames.Add(BitmapFrame.Create(bmpCopied));
                    enc.Save(outStream);
                    bitmap = new System.Drawing.Bitmap(outStream);
                }

                EncoderParameter qualityParam =
                    new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 85L);

                ImageCodecInfo jpegCodec = getEncoderInfo("image/jpeg");

                if (jpegCodec == null)
                    return;

                EncoderParameters encoderParams = new EncoderParameters(1);
                encoderParams.Param[0] = qualityParam;
                Bitmap btm = new Bitmap(bitmap);
                bitmap.Dispose();
                saveFileDialog.DefaultExt = "jpeg";
                saveFileDialog.Filter = "Bitmap Image (.bmp)|*.bmp|JPEG Image (.jpeg)|*.jpeg |Png Image (.png)|*.png ";

                btm.Save(saveFileDialog.FileName, jpegCodec, encoderParams);
                btm.Dispose();
            }
        }
        private ImageCodecInfo getEncoderInfo(string mimeType)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            for (int i = 0; i < codecs.Length; i++)
                if (codecs[i].MimeType == mimeType)
                    return codecs[i];
            return null;
        }
        private void Clear(object sender, RoutedEventArgs e)
        {
            try
            {
                ic.Strokes.Clear();
            }
            catch(Exception ex) { Message a = new Message($"Произошла ошибка: {ex.Message}"); a.Show(); }
        }
    }
}
