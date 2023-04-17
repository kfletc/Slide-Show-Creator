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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IntroForm
{
    /// <summary>
    /// Interaction logic for AudioDisplay.xaml
    /// </summary>
    public partial class AudioDisplay : UserControl
    {

        private SoundTrack audioTrack;
        
        public SoundTrack AudioTrack
        {
            get { return audioTrack; }
            set { audioTrack = value; }
        }

        public AudioDisplay(SoundTrack audioTrack)
        {
            InitializeComponent();
            this.audioTrack = audioTrack;
            BitmapImage audioBitmap = new BitmapImage();
            audioBitmap.BeginInit();
            audioBitmap.UriSource = new Uri(@"C:\ProgramData\SlideShowCreator\Resources\audioThumbnail.jpg");
            audioBitmap.CacheOption = BitmapCacheOption.OnLoad;
            audioBitmap.EndInit();
            Image audioImage = new Image();
            audioImage.Source = audioBitmap;
            String contentString = this.audioTrack.Name + "\n";
            TimeSpan length = this.audioTrack.AudioDuration;
            string lengthString = string.Format("{0:D2}m:{1:D2}s:{2:D3}ms",
            length.Minutes,
            length.Seconds,
            length.Milliseconds);
            contentString += lengthString;
            AudioLabel.Foreground = Brushes.White;
            AudioLabel.HorizontalAlignment = HorizontalAlignment.Center;
            AudioLabel.Margin = new Thickness(0, 90, 0, 0);
            AudioLabel.DataContext = audioImage;
            AudioLabel.Content = contentString;
            AudioBorder.Child = audioImage;
            AudioBorder.VerticalAlignment = VerticalAlignment.Top;
            
        }
    }
}
