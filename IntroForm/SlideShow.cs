using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Security.AccessControl;

namespace IntroForm
{
    public class SlideShow
    {
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private ObservableCollection<Slide> slides;

        public ObservableCollection<Slide> Slides
        {
            get { return slides; }
        }

        private Slide selectedSlide;

        public Slide SelectedSlide
        {
            get { return selectedSlide; }
            set { selectedSlide = value; }
        }

        private ObservableCollection<SlideImage> images;

        public ObservableCollection<SlideImage> Images
        {
            get { return images; }
        }

        private ObservableCollection<SoundTrack> soundTracks;

        public ObservableCollection<SoundTrack> SoundTracks
        {
            get { return soundTracks; } 
        }

        private SoundTrack selectedSoundTrack;

        public SoundTrack SelectedSoundTrack
        {
            get { return selectedSoundTrack; }
            set { selectedSoundTrack = value; }
        }

        private bool isAutomatic;

        public bool IsAutomatic
        {
            get { return isAutomatic; }
            set { isAutomatic = value; }
        }

        public SlideShow(string inName)
        {
            this.name = inName;

            this.slides = new ObservableCollection<Slide>();
            this.images = new ObservableCollection<SlideImage>();
            this.soundTracks = new ObservableCollection<SoundTrack>();

            String baseDir = @"C:\ProgramData\SlideShowCreator\.temp";
            String dir = System.IO.Path.Combine(baseDir, this.name);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            String imageDir = System.IO.Path.Combine(dir, "images");
            if (!Directory.Exists(imageDir))
            {
                Directory.CreateDirectory(imageDir);
            }
            String audioDir = System.IO.Path.Combine(dir, "audio");
            if(!Directory.Exists(audioDir)) 
            {
                Directory.CreateDirectory(audioDir);
            }
        }

        public void resetSlides()
        {
            slides.Clear();
        }

        public void addSlide(Slide slide)
        {
            slides.Add(slide);
        }

        public void addImage(SlideImage image)
        {
            images.Add(image);
        }

        public void play()
        {

        }

        public void deleteTempShow()
        {
            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();
            String dir1 = @"C:\ProgramData\SlideShowCreator\.temp\" + this.name;
            Directory.Delete(dir1, true);
        }

        public void copyTempShowOut()
        {
            String dir1 = @"C:\ProgramData\SlideShowCreator\.temp\" + this.name;
            String dir2 = @"C:\ProgramData\SlideShowCreator\SlideShows\" + this.name;

            SlideShowUtilities.CopyFolder(dir1, dir2);

            foreach (SlideImage image in images)
            {
                image.updateFolder(System.IO.Path.Combine(dir2, "images"));
            }

            foreach (SoundTrack track in SoundTracks)
            {
                track.FolderPath = System.IO.Path.Combine(dir2, "audio");
            }
        }

        public void saveSlideShow()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(this, options);

            string fileName = @"C:\ProgramData\SlideShowCreator\SlideShows\" + this.name + ".json";
            File.WriteAllText(fileName, jsonString);
        }
    }
}
