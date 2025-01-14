﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace hackaton
{
    /// <summary>
    /// Interaction logic for Win.xaml
    /// </summary>

    public partial class Win : Window
    {
        ImageBrush winwin = new ImageBrush();
        public Win()
        {
            InitializeComponent();
            winwin.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + @"\img\" + "Win.png"));
            win.Fill = winwin;
        }

        private void restart_Click(object sender, RoutedEventArgs e)
        {
            IControlable.Hit = 0;
            MainWindow restart = new MainWindow();
            restart.Show();
            this.Close();
        }
    }
}