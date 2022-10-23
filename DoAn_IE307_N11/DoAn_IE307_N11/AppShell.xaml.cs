using DoAn_IE307_N11.ViewModels;
using DoAn_IE307_N11.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace DoAn_IE307_N11
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

    }
}
