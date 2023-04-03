using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace IntroForm
{
    public class SlideImage
    {
        private String fileName;

        public String FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        private String folderPath;

        public String FolderPath
        {
            get { return folderPath; }
            set { folderPath = value; }
        }

        private BitmapImage bitmapImage;

        public BitmapImage BitmapImage
        {
            get { return bitmapImage; }
        }

        public SlideImage(String fileName, String folderPath)
        {
            this.FileName = fileName;
            this.FolderPath = folderPath;
            this.bitmapImage = null;
        }

        public void loadBitmap(int size, bool byWidth)
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

    }
}
