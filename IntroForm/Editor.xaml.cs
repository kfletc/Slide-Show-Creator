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
using System.Threading;
using Microsoft.Win32;
using System.Xml.Linq;

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

        private void show_panel_DragOver(object sender, System.Windows.DragEventArgs e)
        {
            if (e.Data.GetDataPresent("Object"))
            {
                // These Effects values are used in the drag source's
                // GiveFeedback event handler to determine which cursor to display.
                e.Effects = System.Windows.DragDropEffects.Copy;
            }
        }

        private void show_panel_Drop(object sender, System.Windows.DragEventArgs e)
        {
            if (e.Handled == false)
            {
                System.Windows.Controls.Panel panel = (System.Windows.Controls.Panel)sender;
                UIElement element = (UIElement)e.Data.GetData("Object");

                if (panel != null && element != null)
                {
                    System.Windows.Controls.Panel parent = (System.Windows.Controls.Panel)VisualTreeHelper.GetParent(element);
                    if (parent.Name != panel.Name)
                    {
                        if (e.AllowedEffects.HasFlag(System.Windows.DragDropEffects.Copy))
                        {
                            SlideDisplay slideDisplay = new SlideDisplay((SlideDisplay)element);
                            slideDisplay.PreviewMouseUp += slide_MouseUp;
                            slideDisplay.CurrentSlide = new Slide(slideDisplay.CurrentImage);
                            panel.Children.Add(slideDisplay);
                            this.sshow.resetSlides();
                            foreach(SlideDisplay slide in panel.Children)
                            {
                                if (slide.CurrentSlide != null)
                                {
                                    this.sshow.addSlide(slide.CurrentSlide);
                                }
                            }
                            // set the value to return to the DoDragDrop call
                            e.Effects = System.Windows.DragDropEffects.None;
                        }
                    }
                }
            }
        }

        private void slide_MouseUp(object sender, MouseButtonEventArgs e)
        {
            foreach(SlideDisplay slide in SlidePanel.Children)
            {
                slide.unselect();
            }
            SlideDisplay sender2 = (SlideDisplay)sender;
            sender2.select();
            if(sender2.CurrentSlide != null)
            {
                this.sshow.SelectedSlide = sender2.CurrentSlide;

                // set image preview
                Image prevImage = new Image();
                prevImage.Source = this.sshow.SelectedSlide.Image.BitmapImage;
                prevImage.Width = 200;
                prevImage.Height = 175;
                ImagePreview.Child = prevImage;

                // set transition
                NoTransition.IsChecked = false;
                WipeRight.IsChecked = false;
                WipeLeft.IsChecked = false;
                WipeUp.IsChecked = false;
                WipeDown.IsChecked = false;
                CrossFade.IsChecked = false;
                switch (this.sshow.SelectedSlide.Transition)
                {
                    case Slide.TransitionType.None:
                        NoTransition.IsChecked = true;
                        break;
                    case Slide.TransitionType.WipeRight:
                        WipeRight.IsChecked = true;
                        break;
                    case Slide.TransitionType.WipeLeft:
                        WipeLeft.IsChecked = true;
                        break;
                    case Slide.TransitionType.WipeUp:
                        WipeUp.IsChecked = true;
                        break;
                    case Slide.TransitionType.WipeDown:
                        WipeDown.IsChecked = true;
                        break;
                    case Slide.TransitionType.CrossFade:
                        CrossFade.IsChecked = true;
                        break;
                }

                // set transition length
                TransitionLengthTextBox.Text = this.sshow.SelectedSlide.TransitionDuration.ToString();

                // set slide length
                ImageDurationTextBox.Text = this.sshow.SelectedSlide.SlideDuration.ToString();

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
                    /*Image displayImage = new Image();
                    displayImage.Source = image.BitmapImage;
                    Border border = new Border();
                    border.Width = 75;
                    border.Height = 75;
                    border.BorderBrush = Brushes.LightGray;
                    border.BorderThickness = new Thickness(1);
                    border.Margin = new Thickness(5, 5, 5, 5);
                    border.Child = displayImage;
                    ImageDisplayPanel.Children.Add(border);*/

                    SlideDisplay displayImage = new SlideDisplay(image);
                    ImageDisplayPanel.Children.Add(displayImage);
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
