using DoAn_IE307_N11.Models;
using DoAn_IE307_N11.Services;
using DoAn_IE307_N11.ViewModels;
using DoAn_IE307_N11.ViewModels.All;
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
            Routing.RegisterRoute(nameof(NewTransactionPage), typeof(NewTransactionPage));
            Routing.RegisterRoute(nameof(TransactionDetailPage), typeof(TransactionDetailPage));
        }

        async protected override void OnAppearing()
        {
            var viewModel = (this.BindingContext as AppViewModel);

            // This will get wallet info
            await viewModel.GETData();
        }
    }
}
