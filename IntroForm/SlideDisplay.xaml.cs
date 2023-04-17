using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

namespace IntroForm
{
    /// <summary>
    /// Interaction logic for SlideDisplay.xaml
    /// </summary>
    public partial class SlideDisplay : UserControl
    {
        private SlideImage currentImage;

        public SlideImage CurrentImage
        {
            get { return currentImage; }
            set { currentImage = value; }
        }

        public SlideDisplay(SlideImage image)
        {
            InitializeComponent();
            this.currentImage = image;
            this.currentSlide = null;
            Image displayImage = new Image();
            displayImage.Source = currentImage.BitmapImage;
            ImageBorder.Child = displayImage;
        }

        private Slide? currentSlide;

        public Slide? CurrentSlide
        {
            get { return currentSlide; }
            set { currentSlide = value; }
        }

        public SlideDisplay(SlideDisplay sDisplay)
        {
            InitializeComponent();
            this.currentImage = sDisplay.CurrentImage;
            this.currentSlide = sDisplay.currentSlide;
            Image displayImage = new Image();
            displayImage.Source = currentImage.BitmapImage;
            ImageBorder.Width = 75;
            ImageBorder.Height = 75;
            ImageBorder.BorderBrush = Brushes.LightGray;
            ImageBorder.BorderThickness = new Thickness(1);
            ImageBorder.Margin = new Thickness(5, 5, 5, 5);
            ImageBorder.Child = displayImage;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                // Package the data.
                DataObject data = new DataObject();
                data.SetData(ImageBorder.Width);
                data.SetData(ImageBorder.Height);
                data.SetData(ImageBorder.BorderBrush);
                data.SetData(ImageBorder.BorderThickness); 
                data.SetData(ImageBorder.Margin);

                data.SetData("Object", this);

                // Initiate the drag-and-drop operation.
                DragDrop.DoDragDrop(this, data, DragDropEffects.Copy);
            }
        }

        protected override void OnGiveFeedback(GiveFeedbackEventArgs e)
        {
            base.OnGiveFeedback(e);
            // These Effects values are set in the drop target's
            // DragOver event handler.
            if (e.Effects.HasFlag(DragDropEffects.Copy))
            {
                Mouse.SetCursor(Cursors.Cross);
            }
            else if (e.Effects.HasFlag(DragDropEffects.Move))
            {
                Mouse.SetCursor(Cursors.Pen);
            }
            else
            {
                Mouse.SetCursor(Cursors.No);
            }
            e.Handled = true;
        }

        public void select()
        {
            ImageBorder.BorderThickness = new Thickness(2);
            BrushConverter converter = new BrushConverter();
            ImageBorder.BorderBrush = (Brush?)converter.ConvertFrom("#DA34AE");
        }

        public void unselect()
        {
            ImageBorder.BorderBrush = Brushes.LightGray;
            ImageBorder.BorderThickness = new Thickness(1);
        }

    }
}
