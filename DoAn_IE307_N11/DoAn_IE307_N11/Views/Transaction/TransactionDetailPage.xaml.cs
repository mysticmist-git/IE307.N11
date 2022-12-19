using DoAn_IE307_N11.Models;
using DoAn_IE307_N11.ViewModels;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DoAn_IE307_N11.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty(nameof(TransactionId), "transactionId")]
    public partial class TransactionDetailPage : ContentPage
    {
        public string TransactionId { get; set; }

        public TransactionDetailPage()
        {
            InitializeComponent();
        }
    }
}