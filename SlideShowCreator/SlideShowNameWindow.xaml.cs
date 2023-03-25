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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class SlideShowNameWindow : Window
    {
        public SlideShowNameWindow()
        {
            InitializeComponent();
        }

        private void Submit_Button(object sender, RoutedEventArgs e)
        {
            string name = SlideShow_Name.Text;
            SlideShow currentSlideShow = new SlideShow(name);
            EditWindow editWindow = new EditWindow(currentSlideShow);
            this.Hide();
            editWindow.Show();
        }
    }
}
