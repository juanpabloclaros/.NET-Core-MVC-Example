using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(true)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            url.Text = "https://fg.ull.es/";
            Browser.Source = url.Text;
        }

        private void Back_Clicked(object sender, EventArgs e)
        {
            if (Browser.CanGoBack)
                Browser.GoBack();
        }

        private void Forward_Clicked(object sender, EventArgs e)
        {
            if (Browser.CanGoForward)
                Browser.GoForward();
        }

        private void Go_Clicked(object sender, EventArgs e)
        {
            Browser.Source = url.Text;
        }

        private void Browser_Navigating(object sender, WebNavigatingEventArgs e)
        {
            LoadingLabel.IsVisible = true;
        }

        private void Browser_Navigated(object sender, WebNavigatedEventArgs e)
        {
            LoadingLabel.IsVisible = false;
        }
    }
}
