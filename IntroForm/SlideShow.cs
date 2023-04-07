using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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

        public SlideShow(string inName)
        {
            this.name = inName;

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

        public void deleteTempShow()
        {
            String dir1 = @"C:\ProgramData\SlideShowCreator\.temp\" + this.name;
            Directory.Delete(dir1, true);
        }

        public void copyTempShowOut()
        {
            String dir1 = @"C:\ProgramData\SlideShowCreator\.temp\" + this.name;
            String dir2 = @"C:\ProgramData\SlideShowCreator\SlideShows\" + this.name;

            SlideShowUtilities.CopyFolder(dir1, dir2);
            this.deleteTempShow();
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
