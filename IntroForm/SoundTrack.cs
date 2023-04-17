using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace IntroForm
{
    public class SoundTrack
    {
        private String? name;

        public String? Name
        {
            get { return name; }
            set { name = value; }
        }

        private String? folderPath;

        public String? FolderPath
        {
            get { return folderPath; }
            set { folderPath = value; }
        }

        private TimeSpan audioDuration;

        public TimeSpan AudioDuration
        {
            get { return audioDuration; }
            set { audioDuration = value; }
        } 


        public SoundTrack(String name, String folderPath, TimeSpan duration)
        {
            Name = name;
            FolderPath = folderPath;
            AudioDuration = duration;
        }

        [JsonConstructor]
        public SoundTrack() { }
    }
}
