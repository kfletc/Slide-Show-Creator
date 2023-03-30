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
        private String file_name;

        public String File_Name
        {
            get { return file_name; }
            set { file_name = value; }
        }

        private String folder_path;

        public String Folder_Path
        {
            get { return folder_path; }
            set { folder_path = value; }
        }

        private BitmapImage bitmap_image;

        public BitmapImage Bitmap_Image
        {
            get { return bitmap_image; }
        }

        public SlideImage(String file_name, String folder_path)
        {
            this.File_Name = file_name;
            this.Folder_Path = folder_path;
            this.bitmap_image = null;
        }

        public void loadBitmap(int size, bool byWidth)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(folder_path + file_name);
            if (byWidth)
            {
                bitmapImage.DecodePixelWidth = size;
            }
            else bitmapImage.DecodePixelHeight = size;
            bitmapImage.EndInit();
        }

    }
}
