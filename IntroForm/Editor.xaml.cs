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
using System.Text.RegularExpressions;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using System.Numerics;
using System.Windows.Forms.Design;

namespace IntroForm
{
    /// <summary>
    /// Interaction logic for Editor.xaml
    /// </summary>
    public partial class Editor : Window
    {
        private bool isAiff = false;

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
                            ContextMenu deleteMenu = new ContextMenu();
                            MenuItem deleteItem = new MenuItem();
                            deleteItem.Header = "Delete";
                            deleteItem.Click += deleteSlide;
                            deleteMenu.Items.Add(deleteItem);
                            slideDisplay.ImageBorder.ContextMenu = deleteMenu;

                            panel.Children.Add(slideDisplay);

                            this.sshow.resetSlides();
                            foreach(SlideDisplay slide in panel.Children)
                            {
                                if (slide.CurrentSlide != null)
                                {
                                    this.sshow.addSlide(slide.CurrentSlide);
                                }
                            }
                            update_info();

                            // set the value to return to the DoDragDrop call
                            e.Effects = System.Windows.DragDropEffects.None;
                        }
                    }
                }
            }
        }

        private void deleteSlide(object sender, RoutedEventArgs e)
        {
            foreach(SlideDisplay slideDisplay in SlidePanel.Children)
            {
                if(slideDisplay.CurrentSlide == this.sshow.SelectedSlide)
                {
                    SlidePanel.Children.Remove(slideDisplay);
                    this.sshow.Slides.Remove(this.sshow.SelectedSlide);
                    update_info();
                    break;
                }
            }
        }

        private void update_info()
        {
            double total_time = 0.0;
            foreach(Slide slide in this.sshow.Slides)
            {
                total_time += slide.SlideDuration;
                total_time += slide.TransitionDuration;
            }
            total_time = total_time / 1000;
            TimeSpan t = TimeSpan.FromSeconds(total_time);
            string answer = string.Format("{0:D2}m:{1:D2}s:{2:D3}ms",
            t.Minutes,
            t.Seconds,
            t.Milliseconds);
            if (cbManualSlideShow.IsChecked == true)
            {
                ShowTime.Text = "Slideshow Duration: N/A";
            }
            else
            {
                ShowTime.Text = "Slideshow Duration: " + answer;
            }
            SlideTotal.Text = "Total Number of Slides: " + this.sshow.Slides.Count.ToString();
            AudioTotal.Text = "Total Soundtracks: " + this.sshow.SoundTracks.Count.ToString();
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
            Viewer newViewer = new Viewer(this.sshow);
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
                    System.IO.File.Copy(file, dest, true);
                    SlideImage image = new SlideImage(System.IO.Path.GetFileName(file), tempDir);
                    image.loadBitmap();
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

        private void playerAudioOpened(object? sender, EventArgs e)
        {
            MediaPlayer? player = sender as MediaPlayer;
            if (player != null)
            {
                TimeSpan duration = TimeSpan.FromSeconds(0);
                if (player.NaturalDuration.HasTimeSpan)
                {
                    duration = player.NaturalDuration.TimeSpan;
                }

                String tempDir = @"C:\ProgramData\SlideShowCreator\.temp\";
                tempDir = System.IO.Path.Combine(tempDir, this.sshow.Name);
                tempDir = System.IO.Path.Combine(tempDir, "audio");
                String fileName = System.IO.Path.GetFileName(player.Source.ToString());
                player.Close();

                if (this.isAiff)
                {
                    String fullPath = System.IO.Path.Combine(tempDir, fileName);
                    System.IO.File.Delete(fullPath);
                    fileName = fileName.Substring(0, fileName.Length - 4) + ".aiff";
                    this.isAiff = false;
                }

                SoundTrack audioTrack = new SoundTrack(fileName, tempDir, duration);
                this.sshow.SoundTracks.Add(audioTrack);
                updateAudioTracks();
                update_info();
            }
        }

        private void btnImportAudio_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "Audio files (*.wav;*.aiff;*.mp3)|*.wav;*.aiff;*.mp3";
            if (dialog.ShowDialog() == true)
            {
                String tempDir = @"C:\ProgramData\SlideShowCreator\.temp\";
                tempDir = System.IO.Path.Combine(tempDir, this.sshow.Name);
                tempDir = System.IO.Path.Combine(tempDir, "audio");
                String fileName = System.IO.Path.GetFileName(dialog.FileName);
                String dest = System.IO.Path.Combine(tempDir, fileName);
                System.IO.File.Copy(dialog.FileName, dest, true);

                if(fileName.EndsWith(".aiff"))
                {
                    this.isAiff = true;
                    String wavFile = dialog.FileName.Substring(0, dialog.FileName.Length - 5) + ".wav";
                    try
                    {
                        SlideShowUtilities.ConvertAiffToWav(dialog.FileName, wavFile);
                    }
                    catch(FormatException exception)
                    {
                        System.Windows.MessageBox.Show(exception.Message + "\nMake sure aiff file chosen isn't compressed.", "Format Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    dest = wavFile;
                }
                
                MediaPlayer player = new MediaPlayer();
                player.Open(new Uri(dest));
                player.MediaOpened += playerAudioOpened;
            }
        }

        private void updateAudioTracks()
        {
            AudioPanel.Children.Clear();
            foreach(SoundTrack soundTrack in this.sshow.SoundTracks) 
            {
                AudioDisplay audioDisplay = new AudioDisplay(soundTrack);
                audioDisplay.PreviewMouseUp += audio_MouseUp;
                ContextMenu deleteMenu = new ContextMenu();
                MenuItem deleteItem = new MenuItem();
                deleteItem.Header = "Delete";
                deleteItem.Click += deleteAudio;
                deleteMenu.Items.Add(deleteItem);
                audioDisplay.ContextMenu = deleteMenu;
                AudioPanel.Children.Add(audioDisplay);
            }

        }

        private void audio_MouseUp(object sender, RoutedEventArgs e)
        {
            AudioDisplay? audio = sender as AudioDisplay;
            if(audio != null)
            {
                this.sshow.SelectedSoundTrack = audio.AudioTrack;
            }
        }

        private void deleteAudio(object sender, RoutedEventArgs e) 
        {
            foreach(AudioDisplay audioDisplay in AudioPanel.Children) 
            {
                if(this.sshow.SelectedSoundTrack == audioDisplay.AudioTrack)
                {
                    AudioPanel.Children.Remove(audioDisplay);
                    this.sshow.SoundTracks.Remove(audioDisplay.AudioTrack);
                    update_info();
                    break;
                }
            }
        }

        private void cbHandleDefaultTransition(object sender, RoutedEventArgs e)
        {
            foreach(Slide slide in this.sshow.Slides)
            {
                slide.Transition = Slide.TransitionType.None;
            }
            WipeRight.IsChecked = false;
            WipeLeft.IsChecked = false;
            WipeUp.IsChecked = false;
            WipeDown.IsChecked = false;
            CrossFade.IsChecked = false;
            NoTransition.IsChecked = true;
            WipeRight.IsEnabled = false;
            WipeLeft.IsEnabled = false;
            WipeUp.IsEnabled = false;
            WipeDown.IsEnabled = false;
            CrossFade.IsEnabled = false;
            NoTransition.IsEnabled = false;

        }

        private void cbManualDefaultTransition(object sender, RoutedEventArgs e)
        {
            WipeRight.IsEnabled = true;
            WipeLeft.IsEnabled = true;
            WipeUp.IsEnabled = true;
            WipeDown.IsEnabled = true;
            CrossFade.IsEnabled = true;
            NoTransition.IsEnabled = true;
        }

        private void cbHandleDefaultTransitionLength(object sender, RoutedEventArgs e)
        {
            foreach (Slide slide in this.sshow.Slides)
            {
                slide.TransitionDuration = 1000;
            }
            TransitionLengthTextBox.Text = "1000";
            TransitionLengthTextBox.IsEnabled = false;
        }

        private void cbManualDefaultTransitionLength(object sender, RoutedEventArgs e)
        {
            TransitionLengthTextBox.IsEnabled = true;
        }

        private void cbHandleManualSlideShow(object sender, RoutedEventArgs e)
        {
            this.sshow.IsAutomatic = false;
            ImageDurationTextBox.IsEnabled = false;
            update_info();
        }

        private void cbManualManualSlideShow(object sender, RoutedEventArgs e)
        {
            this.sshow.IsAutomatic = true;
            ImageDurationTextBox.IsEnabled = true;
            update_info();
        }

        private void cbHandleDefaultImageDuration(object sender, RoutedEventArgs e)
        {
            foreach (Slide slide in this.sshow.Slides)
            {
                slide.SlideDuration = 3000;
            }
            ImageDurationTextBox.Text = "3000";
            ImageDurationTextBox.IsEnabled = false;
        }

        private void cbManualDefaultImageDuration(object sender, RoutedEventArgs e)
        {
            ImageDurationTextBox.IsEnabled = true;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void rbChooseWipeUp(object sender, RoutedEventArgs e)
        {
            if(this.sshow.SelectedSlide != null)
                this.sshow.SelectedSlide.Transition = Slide.TransitionType.WipeUp;
        }

        private void rbChooseWipeDown(object sender, RoutedEventArgs e)
        {
            if (this.sshow.SelectedSlide != null)
                this.sshow.SelectedSlide.Transition = Slide.TransitionType.WipeDown;
        }

        private void rbChooseWipeLeft(object sender, RoutedEventArgs e)
        {
            if (this.sshow.SelectedSlide != null)
                this.sshow.SelectedSlide.Transition = Slide.TransitionType.WipeLeft;
        }

        private void rbChooseWipeRight(object sender, RoutedEventArgs e)
        {
            if (this.sshow.SelectedSlide != null)
                this.sshow.SelectedSlide.Transition = Slide.TransitionType.WipeRight;
        }

        private void rbChooseCrossFade(object sender, RoutedEventArgs e)
        {
            if (this.sshow.SelectedSlide != null)
                this.sshow.SelectedSlide.Transition = Slide.TransitionType.CrossFade;
        }
        
        private void rbChooseNone(object sender, RoutedEventArgs e)
        {
            if (this.sshow.SelectedSlide != null)
                this.sshow.SelectedSlide.Transition = Slide.TransitionType.None;
        }

        private void tbSetTransitionLength(object sender, RoutedEventArgs e)
        {
            if (this.sshow.SelectedSlide != null)
            {
                this.sshow.SelectedSlide.TransitionDuration = Int32.Parse(TransitionLengthTextBox.Text);
                update_info();
            }
        }

        private void tbSetImageLength(object sender, RoutedEventArgs e)
        {
            if (this.sshow.SelectedSlide != null)
            {
                this.sshow.SelectedSlide.SlideDuration = Int32.Parse(ImageDurationTextBox.Text);
                update_info();
            }
        }
    }
}
