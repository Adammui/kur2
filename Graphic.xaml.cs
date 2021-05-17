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
        public user logged;
        public Graphic(user a)
        {
            InitializeComponent();
            ic.MouseUp += Strokes_StrokesChanged;
            Saves.Add(ic.Strokes.Clone());
            logged = a;
            try
            {
                var fs = new FileStream(tempstrokes,
               FileMode.Open, FileAccess.Read);
                StrokeCollection strokes = new StrokeCollection(fs);
                ic.Strokes = strokes;
            }
            catch { }
            Saves.Add(ic.Strokes.Clone());
            Sliderink.Visibility = Visibility.Visible;
            ic.EditingMode = InkCanvasEditingMode.Ink;
        }
        
        public Graphic(user a,picture p)
        {
            InitializeComponent();
            ic.MouseUp += Strokes_StrokesChanged;
            Saves.Add(ic.Strokes.Clone());
            logged = a;
            try
            {
                var fs = new FileStream(p.path,
               FileMode.Open, FileAccess.Read);
                StrokeCollection strokes = new StrokeCollection(fs);
                ic.Strokes = strokes;
            }
            catch { }
            Saves.Add(ic.Strokes.Clone());
            Sliderink.Visibility = Visibility.Visible;
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
        }
        private void Перо_Click(object sender, RoutedEventArgs e)
        {
            refreshbuttons();
            Sliderpencil.Visibility = Visibility.Visible;
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
            ic.EditingMode = InkCanvasEditingMode.None;
            ic.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, (byte)pickcolor.SelectedColor.R, (byte)pickcolor.SelectedColor.G, (byte)pickcolor.SelectedColor.B));
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
        public void Save(string adr)
        {
            try 
            { 
                RenderTargetBitmap rtb = new RenderTargetBitmap((int)ic.Width, (int)ic.Height, 96d, 96d, PixelFormats.Default);
                rtb.Render(ic);
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(rtb));
            
                FileStream fs = File.Open(adr, FileMode.Create);
                encoder.Save(fs);
                fs.Close();
            }
            catch (Exception ex) { Message exept = new Message(ex.Message); exept.ShowDialog(); }
        }
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
            Save(temppic);
            string str="null";
            Infopicker inf = new Infopicker("Название или описание вашего эскиза:");
            if (inf.ShowDialog() == true)
            {
                try 
                { 
                    str = inf.write.Text;
                    string path = Environment.CurrentDirectory.ToString()+ $@"\Resources\Data\stroke_copies\{str}.bmp";
                    byte[] buffer = File.ReadAllBytes(temppic);
                    string base64String = Convert.ToBase64String(buffer, 0, buffer.Length); 

                    byte[] bufferdata = Convert.FromBase64String(base64String);

                    using (painDB_Entities db = new painDB_Entities())
                    {
                        picture p1 = new picture {painting_id=1, username=logged.username, painting=bufferdata, date_created=DateTime.Now, descript=str, path=path };

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

        private void Clear(object sender, RoutedEventArgs e)
        {
            try
            {
                ic.Strokes.Clear();
            } catch (Exception ex) { Message a = new Message($"Произошла ошибка: {ex.Message}"); a.Show(); }
        }

        /*private void hidepanel(object sender, RoutedEventArgs e)
        {
            if (instruments.Visibility == Visibility.Visible)
            { instruments.Visibility = Visibility.Collapsed; ic.Width = 1903; }
            else if (instruments.Visibility == Visibility.Collapsed)
            { instruments.Visibility = Visibility.Visible; ic.Width = 1519;}
        }
        */






        //maybe ill use it later
        private ImageCodecInfo getEncoderInfo(string mimeType)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            for (int i = 0; i < codecs.Length; i++)
                if (codecs[i].MimeType == mimeType)
                    return codecs[i];
            return null;
        }
        
        void Saveinfile()
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

        
    }
}
