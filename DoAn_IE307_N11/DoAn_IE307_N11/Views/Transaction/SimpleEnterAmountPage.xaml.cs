using DoAn_IE307_N11.Enums;
using DoAn_IE307_N11.ViewModels;
using DoAn_IE307_N11.ViewModels.All;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DoAn_IE307_N11.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SimpleEnterAmountPage : ContentPage
    {
        public ForType Type { get; set; }
        public SimpleEnterAmountPage(object parent, ForType type)
        {
            InitializeComponent();

            this.BindingContext = new EnterAmountViewModel(parent, type);
            Type = type;
            switch (Type)
            {
                case ForType.ForCreateWallet:
                    LblResult.Text = (parent as CreateWalletViewModel).WalletBalance.ToString();
                    break;
                case ForType.ForEditWallet:
                    LblResult.Text = (parent as EditWalletViewModel).Wallet.Wallet.Balance.ToString();
                    break;
            }
        }

        private decimal firstNumner;
        private string operatorName;
        private bool isOperatorClicked;

        public decimal Calculate(decimal firstNumber, decimal secondNumber)
        {
            decimal result = 0;
            if (operatorName == "+")
            {
                result = firstNumber + secondNumber;
            }
            else if (operatorName == "-")
            {
                result = firstNumber - secondNumber;
            }
            else if (operatorName == "*")
            {
                result = firstNumber * secondNumber;
            }
            else if (operatorName == "/")
            {
                result = firstNumber / secondNumber;
            }
            return result;
        }

        private void BtnCommon_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (LblResult.Text == "0" || isOperatorClicked)
            {
                isOperatorClicked = false;
                LblResult.Text = button.Text;
            }
            else
            {
                LblResult.Text += button.Text;
            }

        }

        private void BtnCommonOperation_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            isOperatorClicked = true;
            operatorName = button.Text;
            firstNumner = Convert.ToDecimal(LblResult.Text);
        }

        async private void BtnEqual_Clicked(object sender, EventArgs e)
        {
            var viewModel = (this.BindingContext as EnterAmountViewModel);

            try
            {
                //decimal secondNumber = Convert.ToDecimal(LblResult.Text);
                //string finalResult = Calculate(firstNumner, secondNumber).ToString("0.##");
                //LblResult.Text = finalResult;
                switch (Type)
                {
                    case ForType.ForCreateWallet:
                        {
                            var parent = viewModel.ParentViewModel as CreateWalletViewModel;
                            var result = int.Parse(LblResult.Text);
                            parent.WalletBalance = result;
                            await Navigation.PopAsync();
                        }
                        break;
                    case ForType.ForEditWallet:
                        {
                            var parent = viewModel.ParentViewModel as EditWalletViewModel;
                            var result = int.Parse(LblResult.Text);
                            parent.Wallet.Wallet.Balance = result;
                            parent.PublicOnPropertyChanged(nameof(parent.Wallet));
                            await Navigation.PopAsync();
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Ok");
            }
        }

        private void BtnDel_Clicked(object sender, EventArgs e)
        {
            string number = LblResult.Text;
            if (number != "0")
            {
                number = number.Remove(number.Length - 1, 1);
                if (string.IsNullOrEmpty(number))
                {
                    LblResult.Text = "0";
                }
                else
                {
                    LblResult.Text = number;
                }
            }
        }

        private void BtnAC_Clicked(object sender, EventArgs e)
        {
            LblResult.Text = "0";
            isOperatorClicked = false;
            firstNumner = 0;
        }
    }
}