using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace IntroForm
{
    public class SoundTrack
    {
        private String? folderPath;

        public String? FolderPath
        {
            get { return folderPath; }
            set { folderPath = value; }
        }

        private String? name;

        public String? Name
        {
            get { return name; }
            set { name = value; }
        }

        private Duration audioDuration;

        public Duration AudioDuration
        {
            get { return audioDuration; }
            set { audioDuration = value; }
        }


        public SoundTrack(String folderPath, String name, Duration audioDuration)
        {
            FolderPath = folderPath;
            Name = name;
            AudioDuration = audioDuration;
        }
    }
}
