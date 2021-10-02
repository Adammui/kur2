using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GraphicTool
{
    /// <summary>
    /// Логика взаимодействия для Graphic.xaml
    /// </summary>
    public partial class Graphic : Page
    {
        public List<StrokeCollection> Saves = new List<StrokeCollection> { }; //<
        public List<StrokeCollection> Redos = new List<StrokeCollection> { };     //>
        public string temppic = Environment.CurrentDirectory.ToString() + @"\Resources\Data\temp\t.jpg";
        public string tempstrokes = Environment.CurrentDirectory.ToString() + @"\Resources\Data\temp\ts.bmp";
        public string tempbackg = Environment.CurrentDirectory.ToString() + @"\Resources\Data\temp\tbac.jpg";
        public user logged;
        public Graphic(user a)
        {
            InitializeComponent();
            ic.MouseLeftButtonUp += Strokes_StrokesChanged;
            Saves.Add(ic.Strokes.Clone());
            logged = a;
            try
            { var fs = new FileStream(tempstrokes,
               FileMode.Open, FileAccess.Read);
               StrokeCollection strokes = new StrokeCollection(fs);
               ic.Strokes = strokes;
               fs.Close();

                byte[] bufferbg = File.ReadAllBytes(tempbackg);
                string base64Stringbg = Convert.ToBase64String(bufferbg, 0, bufferbg.Length);
                byte[] bufferdatabg = Convert.FromBase64String(base64Stringbg);

                BitmapImage bmpbg = new BitmapImage();
               bmpbg.BeginInit();
               bmpbg.StreamSource = new MemoryStream(bufferdatabg);
               bmpbg.EndInit();
               background.Source = bmpbg;
               background.Visibility = Visibility.Visible;
            }
            catch(Exception ex) { Message у = new Message(ex.Message); у.ShowDialog(); }
            Saves.Add(ic.Strokes.Clone());
            Sliderink.Visibility = Visibility.Visible;
            
            ic.DefaultDrawingAttributes.Width = 4;
            ic.DefaultDrawingAttributes.Height = 4;
            ic.EditingMode = InkCanvasEditingMode.None;
            ic.EditingMode = InkCanvasEditingMode.Ink;

        }
        
        public Graphic(user a,picture p)
        {
            InitializeComponent();
            ic.MouseLeftButtonUp += Strokes_StrokesChanged;
            Saves.Add(ic.Strokes.Clone());
            logged = a;
            try
            {
                var fs = new FileStream(p.path,
                FileMode.Open, FileAccess.Read);
                StrokeCollection strokes = new StrokeCollection(fs);
                ic.Strokes = strokes;

                BitmapImage bmpbg = new BitmapImage();
                bmpbg.BeginInit();
                bmpbg.StreamSource  = new MemoryStream(p.bg);
                bmpbg.EndInit();
                background.Source = bmpbg;
                background.Visibility = Visibility.Visible;


            }
            catch { }
            Saves.Add(ic.Strokes.Clone());
            Sliderink.Visibility = Visibility.Visible;

            ic.DefaultDrawingAttributes.Width = 4;
            ic.DefaultDrawingAttributes.Height = 4;
            ic.EditingMode = InkCanvasEditingMode.None;
            ic.EditingMode = InkCanvasEditingMode.Ink;


        }
        private void refreshbuttons()
        {
            Slidereraser.Visibility = Visibility.Collapsed;
            Sliderink.Visibility = Visibility.Collapsed;
            Sliderpencil.Visibility = Visibility.Collapsed;
        }

        private void Кисть_Click(object sender, RoutedEventArgs e)
        {
            refreshbuttons();
            Sliderink.Visibility = Visibility.Visible;
            ic.EditingMode = InkCanvasEditingMode.Ink;
            ic.DefaultDrawingAttributes.Width = Sliderink.Value;
            ic.DefaultDrawingAttributes.Height = Sliderink.Value;
            ic.EditingMode = InkCanvasEditingMode.None;
            ic.EditingMode = InkCanvasEditingMode.Ink;
        }
        private void Перо_Click(object sender, RoutedEventArgs e)
        {
            refreshbuttons();
            Sliderpencil.Visibility = Visibility.Visible;
            ic.EditingMode = InkCanvasEditingMode.Ink;
            ic.DefaultDrawingAttributes.Width = Sliderpencil.Value;
            ic.DefaultDrawingAttributes.Height = Sliderpencil.Value / 4;
            ic.EditingMode = InkCanvasEditingMode.None;
            ic.EditingMode = InkCanvasEditingMode.Ink;
        }
        private void Ластик_Click(object sender, RoutedEventArgs e)
        { 
            refreshbuttons();
            Slidereraser.Visibility = Visibility.Visible;
            ic.EditingMode = InkCanvasEditingMode.EraseByPoint;
        }

        private void Sliderink_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ic.EditingMode = InkCanvasEditingMode.Ink;
            ic.DefaultDrawingAttributes.Width = Sliderink.Value;
            ic.DefaultDrawingAttributes.Height = Sliderink.Value;
            ic.EditingMode = InkCanvasEditingMode.None;
            ic.EditingMode = InkCanvasEditingMode.Ink;
        }
        private void Sliderpencil_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ic.EditingMode = InkCanvasEditingMode.Ink;
            ic.DefaultDrawingAttributes.Width = Sliderpencil.Value;
            ic.DefaultDrawingAttributes.Height = Sliderpencil.Value/4;
            ic.EditingMode = InkCanvasEditingMode.None;
            ic.EditingMode = InkCanvasEditingMode.Ink;
        }
        private void Slidereraser_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ic.EraserShape = new RectangleStylusShape(Slidereraser.Value, Slidereraser.Value);
            ic.EditingMode = InkCanvasEditingMode.None;
            ic.EditingMode = InkCanvasEditingMode.EraseByPoint;
        }
        private void Выделение_Click(object sender, RoutedEventArgs e)
        {
            refreshbuttons();
            ic.EditingMode = InkCanvasEditingMode.Select;
        }
        private void Заливка_Click(object sender, RoutedEventArgs e)
        {
            refreshbuttons();
            background.Visibility = Visibility.Hidden;
            ic.EditingMode = InkCanvasEditingMode.None;
            ic.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255,
            (byte)pickcolor.SelectedColor.R, (byte)pickcolor.SelectedColor.G, (byte)pickcolor.SelectedColor.B));
            //перезагрузить фон.....долго долго не было понятно что не так :(
            ic.UpdateLayout();

            StrokeCollection t = new StrokeCollection(ic.Strokes);
            ic.Strokes.Clear();
            Save(tempbackg);
            ic.Strokes = t;

        }
        private void Картинка_Click(object sender, RoutedEventArgs e)
        {
            refreshbuttons();
            ic.EditingMode = InkCanvasEditingMode.None;
            OpenFileDialog oFileDialog = new OpenFileDialog();
            if (oFileDialog.ShowDialog() == true)
            {
                background.Source = new BitmapImage(new Uri(oFileDialog.FileName));
                background.Visibility = Visibility.Visible;
                ic.UpdateLayout();

                StrokeCollection t = new StrokeCollection(ic.Strokes);
                ic.Strokes.Clear();
                Save(tempbackg);
                ic.Strokes = t;
            }
        }
        private void PortableColorPicker_ColorChanged(object sender, RoutedEventArgs e)
        {
            ic.DefaultDrawingAttributes.Color = pickcolor.SelectedColor;
            ic.EditingMode = InkCanvasEditingMode.Ink;
        }

        private void Strokes_StrokesChanged(object sender, MouseButtonEventArgs e)
        {
            if (delet == true)
            {
                Redos.Clear();
                redo.IsEnabled = false;
                redoz.IsEnabled = false;
                delet = false;
            }
            Saves.Add(ic.Strokes.Clone());

            if (Saves.Count() == 20)
            {
                Saves.RemoveAt(0);
            }

            undo.IsEnabled = true;
            undoz.IsEnabled = true;
            try
            {
                var fs = new FileStream(tempstrokes, FileMode.Create);
                ic.Strokes.Save(fs);
                fs.Close();
            }
            catch { }
        }
        bool delet = false;
        private void Undo(object sender, RoutedEventArgs e)
        {
            try 
            { 
                if (Saves.Count() > 1)
                {
                    ic.Strokes.Clear();
                    ic.Strokes.Add(Saves[Saves.Count() - 2]); //вставляю предпоследний элемент сэйва
                    Redos.Add(Saves[Saves.Count() - 1]); //добавляю посл в сэйв справа
                    Saves.RemoveAt(Saves.Count() - 1); //удаляю посл из сэйва слева\
                    redo.IsEnabled = true;
                    redoz.IsEnabled = true;

                    delet = true;
                    if (Saves.Count() > 1)
                    {
                        undo.IsEnabled = true;
                        undoz.IsEnabled = true;
                    }
                    else 
                    { 
                        undo.IsEnabled = false;
                        undoz.IsEnabled = false;
                    }
                }
            } catch (Exception ex) { Message exept = new Message(ex.Message); exept.ShowDialog(); }
        }

        private void Redo(object sender, RoutedEventArgs e)
        {
            try
            { 
                if (Redos.Count() > 0)
                {
                    ic.Strokes.Clear();
                    ic.Strokes.Add(Redos[Redos.Count() - 1]);
                    //Saves.Clear();
                    Saves.Add(Redos[Redos.Count() - 1]);
                    undo.IsEnabled = true;
                    undoz.IsEnabled = true;
                    Redos.RemoveAt(Redos.Count() - 1);

                    delet = false;
                    if (Redos.Count() > 0)
                    {
                        redo.IsEnabled = true;
                        redoz.IsEnabled = true;
                    }
                    else
                    {
                        redo.IsEnabled = false;
                        redoz.IsEnabled = false;
                    }
                }
            } catch (Exception ex) { Message exept = new Message(ex.Message); exept.ShowDialog(); }
        }

        private void Clear(object sender, RoutedEventArgs e)
        {
            try
            {
                ic.Strokes.Clear();
                background.Source = null;
                background.Visibility = Visibility.Visible;
                ic.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255,
                (byte)255, (byte)255, (byte)255));
                ic.UpdateLayout();

                Save(tempbackg);
                byte[] bufferbg = File.ReadAllBytes(tempbackg);
                string base64Stringbg = Convert.ToBase64String(bufferbg, 0, bufferbg.Length);
                byte[] bufferdatabg = Convert.FromBase64String(base64Stringbg);

                BitmapImage bmpbg = new BitmapImage();
                bmpbg.BeginInit();
                bmpbg.StreamSource = new MemoryStream(bufferdatabg);
                bmpbg.EndInit();
                background.Source = bmpbg;

                ic.UpdateLayout();
                Saves.Add(ic.Strokes.Clone());

                var fs = new FileStream(tempstrokes, FileMode.Create);
                ic.Strokes.Save(fs);
                fs.Close();
            }
            catch (Exception ex) { Message a = new Message($"Произошла ошибка: {ex.Message}"); a.Show(); }
        }

        private void hidepanel(object sender, RoutedEventArgs e)
        {
            if (instruments.Visibility == Visibility.Visible)
            { instruments.Visibility = Visibility.Collapsed; ic_scroll.Width = 1903; ic.Width = 1903; background.Width = 1903; }
            else if (instruments.Visibility == Visibility.Collapsed)
            { instruments.Visibility = Visibility.Visible;  ic_scroll.Width = 1519; ic.Width = 1519; background.Width = 1519; }
        }

        //binds
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
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.Key == Key.S)
            {
                SaveintoGallery(sender, e);
            }
        }
        private void showbinds(object sender, RoutedEventArgs e)
        {
            Message a = new Message("CTRL+Z  - Отмена\n CTRL+Y  - Вернуть\n CTRL+С  - Копировать\n CTRL+V  - Вставить\n CTRL+S  - Сохранить\n");
            a.ShowDialog();
        }
        //saves
       
        private void SaveintoFile(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                Save(saveFileDialog.FileName);
            }
        }
        private void SaveintoGallery(object sender, RoutedEventArgs e)
        {
            Infopicker inf = new Infopicker("Название или описание вашего эскиза:");
            if (inf.ShowDialog() == true)
            {
                try 
                {
                    Save(temppic);
                    StrokeCollection t = new StrokeCollection(ic.Strokes);
                    ic.Strokes.Clear();
                    Save(tempbackg);
                    ic.Strokes = t;
                    
                    var fs1 = new FileStream(tempstrokes, FileMode.Create, FileAccess.ReadWrite);
                    ic.Strokes.Save(fs1);
                    fs1.Close();

                    string str = "null";

                    using (painDB_Entities db = new painDB_Entities())
                    {
                    str = inf.write.Text;
                    string path = Environment.CurrentDirectory.ToString()+ $@"\Resources\Data\stroke_copies\{str}.bmp";

                    byte[] buffer = File.ReadAllBytes(temppic);
                    string base64String = Convert.ToBase64String(buffer, 0, buffer.Length); 
                    byte[] bufferdata = Convert.FromBase64String(base64String);

                    byte[] bufferbg = File.ReadAllBytes(tempbackg);
                    string base64Stringbg = Convert.ToBase64String(bufferbg, 0, bufferbg.Length);
                    byte[] bufferdatabg = Convert.FromBase64String(base64Stringbg);

                        picture p1 = new picture { username=logged.username, painting= bufferdata, date_created=DateTime.Now, 
                            descript=str, path=path, bg=bufferdatabg};

                        db.pictures.Add(p1);
                        db.SaveChanges();
                        //сохраняю штрихи в хранилище
                        var fs = new FileStream(path, FileMode.Create);
                        ic.Strokes.Save(fs);
                    }
                }
                catch (Exception ex) { Message exept = new Message(ex.Message); exept.ShowDialog(); }
            }
        }
        public void Save(string adr)
        {
            try
            {
                RenderTargetBitmap rtb = new RenderTargetBitmap((int)ic.Width, (int)ic.Height, 96d, 96d, PixelFormats.Default);
                rtb.Render(ic);
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(rtb));

                using (FileStream fs = File.Open(adr, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    encoder.Save(fs);
                }
                
            }
            catch (Exception ex) { Message exept = new Message(ex.Message); exept.ShowDialog(); }
        }

        public int figure = 0; //0-ничего 1-круг 2-незакрашенный круг 3-прямоуг 4-незакрашенный прямоуг
        double x1, y1, x2, y2;

        private void fig1_Click(object sender, RoutedEventArgs e)
        {

            cleanfigselection();
            figure = 1;
            elipsecolored.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255,
                    (byte)pickcolor.SelectedColor.R, (byte)pickcolor.SelectedColor.G, (byte)pickcolor.SelectedColor.B));
            ic.EditingMode = InkCanvasEditingMode.None;
        }
        private void fig2_Click(object sender, RoutedEventArgs e)
        {

            cleanfigselection();
            figure = 2;
            elipse.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255,
                   (byte)pickcolor.SelectedColor.R, (byte)pickcolor.SelectedColor.G, (byte)pickcolor.SelectedColor.B));
            ic.EditingMode = InkCanvasEditingMode.None;
        }
        private void fig3_Click(object sender, RoutedEventArgs e)
        {

            cleanfigselection();
            figure = 3;
            rect.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255,
                   (byte)pickcolor.SelectedColor.R, (byte)pickcolor.SelectedColor.G, (byte)pickcolor.SelectedColor.B));
            ic.EditingMode = InkCanvasEditingMode.None;
        }
        private void fig4_Click(object sender, RoutedEventArgs e)
        {

            cleanfigselection();
            figure = 4;
            contrect.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255,
                   (byte)pickcolor.SelectedColor.R, (byte)pickcolor.SelectedColor.G, (byte)pickcolor.SelectedColor.B));
            ic.EditingMode = InkCanvasEditingMode.None;
        }


        private void ic_MouseDown(object sender, MouseButtonEventArgs e)
        {

            x1 = e.GetPosition(background).X;
            y1 = e.GetPosition(background).Y;
        }

        private void ic_MouseUp(object sender, MouseButtonEventArgs e)
        {
            x2 = e.GetPosition(background).X;
            y2 = e.GetPosition(background).Y;
            if (x1 > x2) { double temp = x1; x1 = x2; x2 = temp; }
            if (y1 > y2) { double temp = y1; y1 = y2; y2 = temp; }
            try
            {
                ic.UpdateLayout();
                StrokeCollection t = new StrokeCollection(ic.Strokes);
                if (figure == 1 || figure==2)
                {
                    Ellipse e1 = new Ellipse();
                    e1.Width = Math.Abs(x2 - x1);
                    e1.Height = Math.Abs(y2 - y1);
                    var brush = new SolidColorBrush();
                    brush.Color = System.Windows.Media.Color.FromArgb(255,
                    (byte)pickcolor.SelectedColor.R, (byte)pickcolor.SelectedColor.G, (byte)pickcolor.SelectedColor.B);
                    e1.Stroke = brush;
                    if (figure==1 )e1.Fill = brush;
                    e1.StrokeThickness = 4;
                    Thickness margin = new Thickness();
                    margin.Left = x1; margin.Top = y1;
                    e1.Margin = margin;
                    
                    //e1.RenderedGeometry
                    ic.Children.Add(e1);
                    ic.UpdateLayout();

                    ic.Strokes.Clear();
                    Save(tempbackg);
                    ic.Children.Remove(e1);
                }
                else if (figure ==3 || figure==4)
                {
                    System.Windows.Shapes.Rectangle e1 = new System.Windows.Shapes.Rectangle();
                    e1.Width = Math.Abs(x2 - x1);
                    e1.Height = Math.Abs(y2 - y1);
                    var brush = new SolidColorBrush();
                    brush.Color = System.Windows.Media.Color.FromArgb(255,
                    (byte)pickcolor.SelectedColor.R, (byte)pickcolor.SelectedColor.G, (byte)pickcolor.SelectedColor.B);
                    e1.Stroke = brush;
                    if (figure == 3) e1.Fill = brush;
                    e1.StrokeThickness = 4;
                    Thickness margin = new Thickness();
                    margin.Left = x1; margin.Top = y1;
                    e1.Margin = margin;

                    ic.Children.Add(e1);
                    ic.UpdateLayout();

                    ic.Strokes.Clear();
                    Save(tempbackg);
                    ic.Children.Remove(e1);
                }
                byte[] bufferbg = File.ReadAllBytes(tempbackg);
                string base64Stringbg = Convert.ToBase64String(bufferbg, 0, bufferbg.Length);
                byte[] bufferdatabg = Convert.FromBase64String(base64Stringbg);

                BitmapImage bmpbg = new BitmapImage();
                bmpbg.BeginInit();
                bmpbg.StreamSource = new MemoryStream(bufferdatabg);
                bmpbg.EndInit();
                background.Source = bmpbg;
                background.Visibility = Visibility.Visible;

                ic.Background = null;
                ic.Strokes = t;
                ic.UpdateLayout();
            }
            finally
            {
                figure = 0;
                cleanfigselection();
                ic.EditingMode = InkCanvasEditingMode.None;
            }

        }
        public void cleanfigselection() {
            elipsecolored.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 0, 0));
            elipse.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 0, 0));
            rect.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 0, 0));
            contrect.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 0, 0));
        }
        //maybe ill use it later
        private ImageCodecInfo getEncoderInfo(string mimeType)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            for (int i = 0; i < codecs.Length; i++)
                if (codecs[i].MimeType == mimeType)
                    return codecs[i];
            return null;
        }

        void Saveinfile(string adr)
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

            btm.Save(adr, jpegCodec, encoderParams);
            btm.Dispose();

        }

    }
}
