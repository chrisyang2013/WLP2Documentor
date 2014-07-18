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
        private string defaultComment = "Enter Comment Here";
        public MainWindow()
        {
            InitializeComponent();
            checkFileExistence();
            dateSelector.SelectedDate = DateTime.Today;
            commentText.Text = defaultComment;
            labeltip.Content = path;
        }

        private void checkFileExistence()
        {
            if (!File.Exists(path))
                File.Create(path);
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

        public static bool IsApplictionInstalled(string p_name)
        {
            string displayName;
            Microsoft.Win32.RegistryKey key;

            // search in: CurrentUser
            key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall");
            foreach (String keyName in key.GetSubKeyNames())
            {
                Microsoft.Win32.RegistryKey subkey = key.OpenSubKey(keyName);
                displayName = subkey.GetValue("DisplayName") as string;
                if (p_name.Equals(displayName, StringComparison.OrdinalIgnoreCase) == true)
                {
                    return true;
                }
            }

            // search in: LocalMachine_32
            key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall");
            foreach (String keyName in key.GetSubKeyNames())
            {
                Microsoft.Win32.RegistryKey subkey = key.OpenSubKey(keyName);
                displayName = subkey.GetValue("DisplayName") as string;
                if (p_name.Equals(displayName, StringComparison.OrdinalIgnoreCase) == true)
                {
                    return true;
                }
            }

            // search in: LocalMachine_64
            key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall");
            foreach (String keyName in key.GetSubKeyNames())
            {
                Microsoft.Win32.RegistryKey subkey = key.OpenSubKey(keyName);
                displayName = subkey.GetValue("DisplayName") as string;
                if (p_name.Equals(displayName, StringComparison.OrdinalIgnoreCase) == true)
                {
                    return true;
                }
            }

            // NOT FOUND
            return false;
        }
        
    }
}