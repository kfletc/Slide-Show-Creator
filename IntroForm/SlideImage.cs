using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Forms.Design;
using System.Windows.Media.Imaging;

namespace IntroForm
{
    public class SlideImage
    {
        private String? fileName;

        public String? FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        private String? folderPath;

        public String? FolderPath
        {
            get { return folderPath; }
            set { folderPath = value; }
        }

        private BitmapImage? bitmapImage;

        [JsonIgnore]
        public BitmapImage? BitmapImage
        {
            get { return bitmapImage; }
        }

        public SlideImage(String fileName, String folderPath)
        {
            this.FileName = fileName;
            this.FolderPath = folderPath;
        }

        [JsonConstructor]
        public SlideImage() { }

        public void loadBitmapSize(int size, bool byWidth)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(folderPath + fileName);
            if (byWidth)
            {
                bitmapImage.DecodePixelWidth = size;
            }
            else bitmapImage.DecodePixelHeight = size;
            bitmapImage.EndInit();
        }

        public void loadBitmap()
        {
            if(folderPath != null && fileName != null) 
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(System.IO.Path.Combine(folderPath, fileName));
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                this.bitmapImage = bitmapImage;
            }
        }

        public void updateFolder(String folder)
        {
            this.FolderPath = folder;
            loadBitmap();
        }

    }
}
