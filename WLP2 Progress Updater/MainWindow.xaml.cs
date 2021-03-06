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
using System.IO;

namespace WLP2_Progress_Updater
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string path = @"\\PRODDATA-DC1\Groups\Clients\WLP2\Chris\WLP2Progress.txt";
        //private string path = @"C:\Users\Zaidongy\Desktop\test.txt";
        private string defaultComment = "Enter Comment Here";
        public MainWindow()
        {
            InitializeComponent();
            checkFileExistence();
            dateSelector.SelectedDate = DateTime.Today;
            commentText.Text = defaultComment;
            labeltip.Content = path;
        }

        /// <summary>
        /// Checks if the progress update file exists, if not display an error message
        /// </summary>
        private void checkFileExistence()
        {
            if (!File.Exists(path))
            {
                try
                {
                    File.Create(path);
                }
                catch (System.IO.IOException) 
                { 
                    MessageBox.Show("File not found.\nAre you logged into the server?");
                    this.Close();
                }
            }
        }
        private void commentText_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            textBoxFocused();
        }

        private void commentText_GotMouseCapture(object sender, MouseEventArgs e)
        {
            textBoxFocused();
        }

        private void textBoxFocused()
        {
            if (commentText.Text == defaultComment)
                commentText.SelectAll();
            labeltip.Content = "Enter comment";
        }

        private void MenuItemExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            using(StreamWriter writer = new StreamWriter(path, append: true))
            {
                writer.WriteLine(dateSelector.SelectedDate.Value.ToString("MM/dd/yyyy"));
                writer.WriteLine("----------");//10 to match date format
                writer.WriteLine(commentText.Text);
                writer.WriteLine();
            }
            labeltip.Content = "Submitted!";
            commentText.Text = defaultComment;
        }

        private void MenuItemOpen_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(path);
            labeltip.Content = "File Opened!";
        }

        private void dateSelector_GotFocus(object sender, RoutedEventArgs e)
        {
            labeltip.Content = "Select a Date";
        }

        private void ToolBar_Loaded(object sender, RoutedEventArgs e)
        {
            ToolBar toolBar = sender as ToolBar;
            var overflowGrid = toolBar.Template.FindName("OverflowGrid", toolBar) as FrameworkElement;
            if (overflowGrid != null)
            {
                overflowGrid.Visibility = Visibility.Collapsed;
            }
            var mainPanelBorder = toolBar.Template.FindName("MainPanelBorder", toolBar) as FrameworkElement;
            if (mainPanelBorder != null)
            {
                mainPanelBorder.Margin = new Thickness();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Closing -= Window_Closing;
            e.Cancel = true;
            var anim = new System.Windows.Media.Animation.DoubleAnimation(0, (Duration)TimeSpan.FromSeconds(1));
            anim.Completed += (s, _) => this.Close();
            this.BeginAnimation(UIElement.OpacityProperty, anim);
        }
    }
}
