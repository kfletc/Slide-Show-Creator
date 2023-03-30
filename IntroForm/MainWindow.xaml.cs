﻿using System;
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

namespace IntroForm
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton==MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState= WindowState.Minimized;
        }

        private void btnMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
            }
            else
            {
                WindowState = WindowState.Maximized;
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnCreateSlideShow_Click(object sender, RoutedEventArgs e)
        {
            WelcomePanel.Visibility = Visibility.Hidden;
            FileNamePanel.Visibility = Visibility.Visible;
            btnBack.Visibility = Visibility.Visible;
        }

        private void btnViewExisting_Click(object sender, RoutedEventArgs e)
        {
            ViewExisting viewExisting = new ViewExisting();
            this.Close();
            viewExisting.Show();
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            SlideShow newSlideshow = new SlideShow(FileNameTextBox.Text);
            Editor newWin = new Editor(newSlideshow);
            this.Close();
            newWin.Show();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            FileNamePanel.Visibility = Visibility.Hidden;
            WelcomePanel.Visibility = Visibility.Visible;
            btnBack.Visibility = Visibility.Hidden;
            FileNameTextBox.Clear();
        }

    }
}