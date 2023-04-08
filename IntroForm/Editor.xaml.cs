using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Text.Json;
using System.IO;
using Microsoft.Win32;

namespace IntroForm
{
    /// <summary>
    /// Interaction logic for Editor.xaml
    /// </summary>
    public partial class Editor : Window
    {
        private SlideShow sshow;

        public SlideShow SShow
        {
            get { return sshow; }
            set { sshow = value; }
        }

        public Editor(SlideShow input)
        {
            this.sshow = input;
            InitializeComponent();
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
            this.sshow.deleteTempShow();
            System.Windows.Application.Current.Shutdown();
        }

        private void btnShow_Click(object sender, RoutedEventArgs e)
        {
            ImageDisplayPanel.Children.Clear();

            this.sshow.copyTempShowOut();
            this.sshow.deleteTempShow();
            this.sshow.saveSlideShow();

            this.Hide();
            Viewer newViewer = new Viewer();
            newViewer.Show();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.sshow.deleteTempShow();
            MainWindow mainWindow= new MainWindow();
            this.Close();
            mainWindow.Show();
        }

        private void btnImportImages_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog diag = new FolderBrowserDialog();
            if (diag.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string folder = diag.SelectedPath;  //selected folder path
                string[] pngFiles = Directory.GetFiles(folder, "*.png");
                string[] jpegFiles = Directory.GetFiles(folder, "*.jpeg");
                string[] jpgFiles = Directory.GetFiles(folder, "*.jpg");
                string[] imageFiles = pngFiles.Concat(jpegFiles).Concat(jpgFiles).ToArray();
                String tempDir = @"C:\ProgramData\SlideShowCreator\.temp\";
                tempDir = System.IO.Path.Combine(tempDir, this.sshow.Name);
                tempDir = System.IO.Path.Combine(tempDir, "images");
                foreach (string file in imageFiles)
                {
                    String dest = System.IO.Path.Combine(tempDir, System.IO.Path.GetFileName(file));
                    File.Copy(file, dest, true);
                    SlideImage image = new SlideImage(System.IO.Path.GetFileName(file), tempDir);
                    this.sshow.addImage(image);
                }

                ImageDisplayPanel.Children.Clear();
                foreach (SlideImage image in this.sshow.Images)
                {
                    Image displayImage = new Image();
                    displayImage.Source = image.BitmapImage;
                    Border border = new Border();
                    border.Width = 75;
                    border.Height = 75;
                    border.BorderBrush = Brushes.LightGray;
                    border.BorderThickness = new Thickness(1);
                    border.Margin = new Thickness(5, 5, 5, 5);
                    border.Child = displayImage;
                    ImageDisplayPanel.Children.Add(border);
                }

            }
        }

        private void btnImportAudio_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cbHandleDefaultTransition(object sender, RoutedEventArgs e)
        {

        }

        private void cbManualDefaultTransition(object sender, RoutedEventArgs e)
        {

        }

        private void cbHandleDefaultTransitionLength(object sender, RoutedEventArgs e)
        {

        }

        private void cbManualDefaultTransitionLength(object sender, RoutedEventArgs e)
        {

        }

        private void cbHandleNoTransitionAnimations(object sender, RoutedEventArgs e)
        {

        }

        private void cbManualNoTransitionAnimations(object sender, RoutedEventArgs e)
        {

        }

        private void cbHandleDefaultImageDuration(object sender, RoutedEventArgs e)
        {

        }

        private void cbManualDefaultImageDuration(object sender, RoutedEventArgs e)
        {

        }
    }
}
