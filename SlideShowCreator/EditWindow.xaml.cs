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
using System.Windows.Shapes;

namespace SlideShowCreator
{
    /// <summary>
    /// Interaction logic for EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        private SlideShow slide_show;

        public SlideShow slideShow
        {
            get { return slide_show; }
            set { slide_show = value; }
        }

        public EditWindow(SlideShow slideShow)
        {
            this.slideShow = slideShow;
            InitializeComponent();
            SlideShowNameDisplay.Text = "Name: " + slideShow.Name;
        }
    }
}
