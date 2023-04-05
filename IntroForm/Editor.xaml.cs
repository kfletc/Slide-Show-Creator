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
using System.Windows.Shapes;
using System.Text.Json;
using System.IO;

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
            Application.Current.Shutdown();
        }

        private void btnShow_Click(object sender, RoutedEventArgs e)
        {
            SlideImage testImage = new SlideImage("test/path", "testname");
            Slide testSlide = new Slide(testImage);
            this.sshow.Test = testSlide;

            this.sshow.copyTempShowOut();
            this.sshow.saveSlideShow();
            
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
