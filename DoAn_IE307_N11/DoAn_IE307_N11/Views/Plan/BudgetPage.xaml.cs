using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DoAn_IE307_N11.Models;
using Xamarin.Essentials;

namespace DoAn_IE307_N11.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BudgetPage : TabbedPage
    {
        ObservableCollection<TransactionType> Types { get; set; }
        public BudgetPage()
        {
            InitializeComponent();
            //Types = new ObservableCollection<TransactionType>(GenerateTransactionTypes());
            //LstTransaction.SetBinding(ListView.ItemsSourceProperty, new Binding
            //{
            //    Source = Types,
            //    Path = "."
            //});
        }

        //private List<TransactionType> GenerateTransactionTypes()
        //{
        //    return new List<TransactionType>
        //    {
        //        new TransactionType
        //        {
        //            Name = "Ăn uống",
        //            Image = "AnUong.png"
        //        },
        //        new TransactionType
        //        {
        //            Name = "Di chuyển",
        //            Image = "DiChuyen.png"
        //        },
        //        new TransactionType
        //        {
        //            Name = "Thuê nhà",
        //            Image = "ThueNha.png"
        //        }
        //    };
        //}
    }
}