using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace IntroForm
{
    /// <summary>
    /// Interaction logic for Viewer.xaml
    /// </summary>
    public partial class Viewer : Window
    {
        private SlideShow sshow;

        public SlideShow SShow
        {
            get { return sshow; }
            set { sshow = value; }
        }
        // instance variables
        private int audioNum = 0;
        private int slideNum = 0;
        private Border border1;
        private Border border2;
        private Image image1;
        private Image image2;
        private LinearGradientBrush lgbrush;
        private DoubleAnimation animation1;
        private DoubleAnimation animation2;
        private DoubleAnimation animation3;
        private DoubleAnimation animation4;
        private AnimationClock? clock1;
        private AnimationClock? clock2;
        private AnimationClock? clock3;
        private AnimationClock? clock4;
        private DispatcherTimer? timer;
        private DispatcherTimer? transitionTimer;
        private MediaElement? audioPlayer;
        private int currentMs = 0;
        private int currentTMs = 0;
        private Slide currentSlide;

        public Viewer(SlideShow sshow)
        {
            InitializeComponent();
            this.sshow = sshow;
            if(this.sshow.SoundTracks.Count > 0)
            {
                audioPlayer = new MediaElement();
                String audioPath = System.IO.Path.Combine(this.sshow.SoundTracks[audioNum].FolderPath, this.sshow.SoundTracks[audioNum].Name);
                audioPlayer.Source = new Uri(audioPath);
                audioPlayer.MediaEnded += trackOver;
                AudioGrid.Children.Add(audioPlayer);
                audioPlayer.LoadedBehavior = MediaState.Manual;
                audioPlayer.UnloadedBehavior = MediaState.Manual;
                audioPlayer.Play();
            }
            if(this.sshow.IsAutomatic == false)
            {
                btnPlay.Visibility = Visibility.Hidden;
                btnStop.Visibility = Visibility.Hidden;
                if (this.sshow.Slides.Count > 0 )
                {

                    image1 = new Image();
                    image1.Source = this.sshow.Slides[0].Image.BitmapImage;
                    border1 = new Border();
                    border1.Child = image1;
                    ImgGrid.Children.Add(border1);
                }
            }
            else
            {
                btnNext.Visibility = Visibility.Hidden;
                btnPrevious.Visibility = Visibility.Hidden;
                if (this.sshow.Slides.Count > 1)
                {
                    doStoryBoard(this.sshow.Slides[0], this.sshow.Slides[1]);
                }
            }
        }

        private void doStoryBoard(Slide slide1, Slide slide2)
        {
            image1 = new Image();
            if (slide1.Image != null)
            {
                image1.Source = slide1.Image.BitmapImage;
            }
            image2 = new Image();
            if (slide2.Image != null)
            {
                image2.Source = slide2.Image.BitmapImage;
            }
            this.currentSlide = slide1;
            border1 = new Border();
            border2 = new Border();
            BrushConverter converter = new BrushConverter();
            border1.Background = (Brush?)converter.ConvertFrom("#060531");
            border2.Background = (Brush?)converter.ConvertFrom("#060531");
            border1.Child = image1;
            border2.Child = image2;

            if (slide1.Transition == Slide.TransitionType.None)
            {
                border2.Opacity = 0;
                image2.Opacity = 0;
                timer = new DispatcherTimer(DispatcherPriority.Render);
                timer.Interval = TimeSpan.FromMilliseconds(10);
                timer.Tick += timerTickNone;
                timer.Start();
            }
            else if(slide1.Transition == Slide.TransitionType.CrossFade)
            {
                border2.Opacity = 0;
                image2.Opacity = 0;
                animation1 = new DoubleAnimation();
                animation2 = new DoubleAnimation();
                animation3 = new DoubleAnimation();
                animation4 = new DoubleAnimation();
                animation1.BeginTime = new TimeSpan(0);
                animation2.BeginTime = new TimeSpan(0);
                animation3.BeginTime = new TimeSpan(0);
                animation4.BeginTime = new TimeSpan(0);
                animation1.Duration = TimeSpan.FromMilliseconds(currentSlide.TransitionDuration);
                animation2.Duration = TimeSpan.FromMilliseconds(currentSlide.TransitionDuration);
                animation3.Duration = TimeSpan.FromMilliseconds(currentSlide.TransitionDuration);
                animation4.Duration = TimeSpan.FromMilliseconds(currentSlide.TransitionDuration);
                animation1.From = 1;
                animation2.From = 0;
                animation3.From = 1;
                animation4.From = 0;
                animation1.To = 0;
                animation2.To = 1;
                animation3.To = 0;
                animation4.To = 1;
                timer = new DispatcherTimer(DispatcherPriority.Render);
                timer.Interval = TimeSpan.FromMilliseconds(10);
                timer.Tick += timerTickCrossfade;
                timer.Start();
            }
            else if (slide1.Transition == Slide.TransitionType.WipeDown)
            {
                setUpWipe(0, 0, 0, 1);
            }
            else if(slide1.Transition == Slide.TransitionType.WipeUp)
            {
                setUpWipe(0, 1, 0, 0);
            }
            else if(slide1.Transition == Slide.TransitionType.WipeRight)
            {
                setUpWipe(0, 0, 1, 0);
            }
            else if(slide1.Transition == Slide.TransitionType.WipeLeft)
            {
                setUpWipe(1, 0, 0, 0);
            }

            ImgGrid.Children.Clear();
            ImgGrid.Children.Add(border1);
            ImgGrid.Children.Add(border2);

        }

        private void setUpWipe(int startPoint1, int startPoint2, int endPoint1, int endPoint2)
        {
            animation1 = new DoubleAnimation();
            animation2 = new DoubleAnimation();
            animation1.BeginTime = new TimeSpan(0);
            animation2.BeginTime = new TimeSpan(0);;
            animation1.Duration = TimeSpan.FromMilliseconds(currentSlide.TransitionDuration);
            animation2.Duration = TimeSpan.FromMilliseconds(currentSlide.TransitionDuration);
            animation1.From = 0;
            animation2.From = 0;
            animation1.To = 1;
            animation2.To = 1;
            System.Windows.Point startPoint = new System.Windows.Point(startPoint1, startPoint2);
            System.Windows.Point endPoint = new System.Windows.Point(endPoint1, endPoint2);
            lgbrush = new LinearGradientBrush();
            lgbrush.StartPoint =  startPoint;
            lgbrush.EndPoint = endPoint;
            GradientStop black = new GradientStop();
            GradientStop transparent = new GradientStop();
            black.Offset = 0;
            black.Color = Colors.Black;
            transparent.Offset = 0;
            transparent.Color = Colors.Transparent;
            lgbrush.GradientStops.Add(black);
            lgbrush.GradientStops.Add(transparent);
            image2.OpacityMask = lgbrush;
            border2.OpacityMask = lgbrush;
            timer = new DispatcherTimer(DispatcherPriority.Render);
            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Tick += timerTickWipe;
            timer.Start();
        }

        private void timerTickNone(object? sender, EventArgs e)
        {
            currentMs += 10;
            if (timer != null)
            {
                if(currentMs >= currentSlide.SlideDuration || this.sshow.IsAutomatic == false)
                {
                    timer.Stop();
                    currentMs = 0;
                    timer = null;
                    image1.Opacity = 0;
                    border1.Opacity = 0;
                    image2.Opacity = 1;
                    border2.Opacity = 1;
                    if (this.sshow.Slides.Count > slideNum + 2 && this.sshow.IsAutomatic == true)
                    {
                        slideNum += 1;
                        doStoryBoard(this.sshow.Slides[slideNum], this.sshow.Slides[slideNum + 1]);
                    }
                    else if (this.sshow.IsAutomatic == true)
                    {
                        timer = new DispatcherTimer(DispatcherPriority.Render);
                        timer.Interval = TimeSpan.FromMilliseconds(10);
                        timer.Tick += timerTickEnd;
                        timer.Start();
                    }
                }
            }
        }

        private void timerTickCrossfade(object? sender, EventArgs e)
        {
            currentMs += 10;

            if (timer != null)
            {
                if (currentMs >= currentSlide.SlideDuration || this.sshow.IsAutomatic == false)
                {
                    timer.Stop();
                    timer = null;
                    currentMs = 0;
                    clock1 = animation1.CreateClock();
                    clock2 = animation2.CreateClock();
                    clock3 = animation3.CreateClock();
                    clock4 = animation4.CreateClock();
                    image1.ApplyAnimationClock(Image.OpacityProperty, clock1);
                    image2.ApplyAnimationClock(Image.OpacityProperty, clock2);
                    border1.ApplyAnimationClock(Border.OpacityProperty, clock3);
                    border2.ApplyAnimationClock(Border.OpacityProperty, clock4);
                    transitionTimer = new DispatcherTimer(DispatcherPriority.Render);
                    transitionTimer.Interval = TimeSpan.FromMilliseconds(10);
                    transitionTimer.Tick += timerTickTransition;
                    transitionTimer.Start();
                }
            }
        }
        private void timerTickWipe(object? sender, EventArgs e)
        {
            currentMs += 10;
            if(timer != null)
            {
                if (currentMs >= currentSlide.SlideDuration || this.sshow.IsAutomatic == false)
                {
                    timer.Stop();
                    timer = null;
                    currentMs = 0;
                    clock1 = animation1.CreateClock();
                    clock2 = animation2.CreateClock();
                    lgbrush.GradientStops[0].ApplyAnimationClock(GradientStop.OffsetProperty, clock1);
                    lgbrush.GradientStops[1].ApplyAnimationClock(GradientStop.OffsetProperty, clock2);
                    image2.OpacityMask = lgbrush;
                    border2.OpacityMask = lgbrush;
                    //BlackImage.ApplyAnimationClock(GradientStop.OffsetProperty, clock3);
                    //TransparentImage.ApplyAnimationClock(GradientStop.OffsetProperty, clock4);
                    transitionTimer = new DispatcherTimer(DispatcherPriority.Render);
                    transitionTimer.Interval = TimeSpan.FromMilliseconds(10);
                    transitionTimer.Tick += timerTickTransition;
                    transitionTimer.Start();
                }
            }
        }

        private void timerTickTransition(object? sender, EventArgs e)
        {
            currentTMs += 10;

            if(transitionTimer != null && currentTMs >= currentSlide.TransitionDuration)
            {
                currentTMs = 0;
                transitionTimer.Stop();
                transitionTimer = null;
                clock1 = null;
                clock2 = null;
                if(this.sshow.Slides.Count > slideNum + 2 && this.sshow.IsAutomatic == true)
                {
                    slideNum += 1;
                    doStoryBoard(this.sshow.Slides[slideNum], this.sshow.Slides[slideNum + 1]);
                }
                else if (this.sshow.IsAutomatic == true)
                {
                    timer = new DispatcherTimer(DispatcherPriority.Render);
                    timer.Interval = TimeSpan.FromMilliseconds(10);
                    timer.Tick += timerTickEnd;
                    timer.Start();
                }
            }
        }

        private void timerTickEnd(object? sender, EventArgs e)
        {
            currentMs += 10;
            if(timer != null)
            {
                if (currentMs >= (currentSlide.SlideDuration + currentSlide.TransitionDuration))
                {
                    timer.Stop();
                    timer = null;
                    if(audioPlayer!= null)
                    {
                        audioPlayer.Stop();
                        audioPlayer = null;
                    }
                }
            }
        }

        private void trackOver(object? sender, EventArgs e)
        {
            audioNum += 1;
            if(this.sshow.SoundTracks.Count > audioNum) 
            {
                audioPlayer = new MediaElement();
                String audioPath = System.IO.Path.Combine(this.sshow.SoundTracks[audioNum].FolderPath, this.sshow.SoundTracks[audioNum].Name);
                audioPlayer.Source = new Uri(audioPath);
                AudioGrid.Children.Add(audioPlayer);
                audioPlayer.MediaEnded += trackOver;
                audioPlayer.LoadedBehavior = MediaState.Manual;
                audioPlayer.UnloadedBehavior = MediaState.Manual;
                audioPlayer.Play();
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

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            if(clock1 != null)
            {
                clock1.Controller.Pause();
            }
            if(clock2 != null)
            {
                clock2.Controller.Pause();
            }
            if(clock3 != null)
            {
                clock3.Controller.Pause();
            }
            if(clock4 != null)
            { 
                clock4.Controller.Pause();
            }
            if(timer != null)
            {
                timer.Stop();
            }
            if(transitionTimer != null)
            {
                transitionTimer.Stop();
            }
            if(audioPlayer != null)
            {
                audioPlayer.Pause();
            }
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            if (clock1 != null)
            {
                clock1.Controller.Resume();
            }
            if (clock2 != null)
            {
                clock2.Controller.Resume();
            }
            if (clock3 != null)
            {
                clock3.Controller.Resume();
            }
            if (clock4 != null)
            {
                clock4.Controller.Resume();
            }
            if (timer != null)
            {
                timer.Start();
            }
            if(transitionTimer != null)
            {
                transitionTimer.Start();
            }
            if(audioPlayer != null)
            {
                audioPlayer.Play();
            }
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            if(slideNum > 0)
            {
                doStoryBoard(this.sshow.Slides[slideNum], this.sshow.Slides[slideNum - 1]);
                slideNum -= 1;
                if (slideNum == 0)
                {
                    btnPrevious.IsEnabled = false;
                }
            }

        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if(this.sshow.Slides.Count > slideNum + 1)
            {
                slideNum += 1;
                doStoryBoard(this.sshow.Slides[slideNum-1], this.sshow.Slides[slideNum]);
                btnPrevious.IsEnabled = true;
            }
        }
    }
}
