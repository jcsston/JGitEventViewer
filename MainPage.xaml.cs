﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace JGitEventViewer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Refresh()
        {
            this.EventFetchProgressCircle.IsActive = true;

            GitHubEvents result = await ((App)App.Current).Git.RefreshAsync();
            this.GitEvents.ItemsSource = result;

            this.EventFetchProgressCircle.IsActive = false;
        }

        private void RefreshEvents_Click(object sender, RoutedEventArgs e)
        {
            this.Refresh();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Trigger inital refresh
            this.Refresh();
        }
    }
}
