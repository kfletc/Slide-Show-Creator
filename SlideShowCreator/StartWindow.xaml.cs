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

namespace SlideShowCreator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        public StartWindow()
        {
            InitializeComponent();
        }

        private void CreateNewButtonClick(object sender, RoutedEventArgs e)
        {
            this.Hide();
            var nameWindow = new SlideShowNameWindow();
            nameWindow.Show();
        }

        private void ViewExistingButtonClick(object sender, RoutedEventArgs e)
        {
            this.Hide();
            var viewWindow = new ViewerWindow();
            viewWindow.Show();
        }
    }
}
