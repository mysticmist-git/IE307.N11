﻿using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DoAn_IE307_N11.Views
{
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
        }

        private void ImgNotification_Tapped(object sender, EventArgs e)
        {

        }

        private void BtnAdd_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddTransactionPage());
        }
    }
}