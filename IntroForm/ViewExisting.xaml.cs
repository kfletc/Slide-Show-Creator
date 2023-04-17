using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace IntroForm
{
    /// <summary>
    /// Interaction logic for ViewExisting.xaml
    /// </summary>
    public partial class ViewExisting : Window
    {
        private String? selectedSlideShow;

        public ViewExisting()
        {

            InitializeComponent();
            String slideShowPath = @"C:\ProgramData\SlideShowCreator\SlideShows";
            string[] jsonFiles = Directory.GetFiles(slideShowPath, "*.json");
            foreach (string jsonFile in jsonFiles)
            {
                Border jsonBorder = new Border();

                jsonBorder.Width = 110;
                jsonBorder.Height = 110;
                jsonBorder.BorderBrush = Brushes.Transparent;
                jsonBorder.BorderThickness = new Thickness(1);
                jsonBorder.Margin = new Thickness(10, 10, 10, 10);
                jsonBorder.PreviewMouseUp += selectJsonFile;
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(@"C:\ProgramData\SlideShowCreator\Resources\Photo_slideshow.png");
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                Image image = new Image();
                image.Source = bitmapImage;
                image.VerticalAlignment = VerticalAlignment.Top;
                jsonBorder.Child = image;
                Grid grid = new Grid();
                grid.Children.Add(jsonBorder);
                Label jsonLabel = new Label();
                jsonBorder.DataContext = jsonLabel;
                jsonLabel.Foreground = Brushes.White;
                jsonLabel.HorizontalAlignment = HorizontalAlignment.Center;
                jsonLabel.VerticalAlignment = VerticalAlignment.Bottom;
                jsonLabel.Margin = new Thickness(0, 0, 0, 8);
                jsonLabel.DataContext = jsonBorder;
                String jsonFileName = System.IO.Path.GetFileName(jsonFile);
                jsonLabel.Content = jsonFileName.Substring(0, jsonFileName.Length - 5);
                grid.Children.Add(jsonLabel);
                JsonDisplayPanel.Children.Add(grid);
            }
        }

        private void selectJsonFile(object sender , RoutedEventArgs e)
        {
            Border? border = sender as Border;
            if (border != null)
            {
                foreach (Grid grid in JsonDisplayPanel.Children)
                {
                    Border? otherBorder = grid.Children[0] as Border;
                    if (otherBorder != null)
                    {
                        otherBorder.BorderBrush = Brushes.Transparent;
                    }
                }
                BrushConverter converter = new BrushConverter();
                border.BorderBrush = (Brush?)converter.ConvertFrom("#DA34AE");
                border.BorderThickness = new Thickness(2);
                Label? label = border.DataContext as Label;
                String? jsonName;
                if (label != null)
                {
                    jsonName = (String)label.Content;
                    String slideShowPath = @"C:\ProgramData\SlideShowCreator\SlideShows";
                    slideShowPath = System.IO.Path.Combine(slideShowPath, jsonName + ".json");
                    this.selectedSlideShow = slideShowPath;
                }

            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
            }
            else
            {
                WindowState = WindowState.Maximized;
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnShow_Click(object sender, RoutedEventArgs e)
        {
            if(selectedSlideShow!= null)
            {
                String jsonString = File.ReadAllText(this.selectedSlideShow);
                SlideShow? slideShow = JsonSerializer.Deserialize<SlideShow>(jsonString)!;
                if (slideShow != null)
                {
                    foreach(Slide slide in slideShow.Slides)
                    {
                        slide.Image.loadBitmap();
                    }

                    Viewer viewer = new Viewer(slideShow);
                    this.Close();
                    viewer.Show();
                }
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            this.Close();
            mainWindow.Show();
        }
    }
}
