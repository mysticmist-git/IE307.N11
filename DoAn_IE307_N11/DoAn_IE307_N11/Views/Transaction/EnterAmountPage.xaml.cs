using DoAn_IE307_N11.Enums;
using DoAn_IE307_N11.ViewModels;
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
    public partial class EnterAmountPage : ContentPage
    {
        public EnterAmountPage(object parent, ForType type)
        {
            InitializeComponent();

            this.BindingContext = new EnterAmountViewModel(parent, type);
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

        private void BtnEqual_Clicked(object sender, EventArgs e)
        {
            try
            {
                var finalResult = firstNumner.ToString();

                if (isOperatorClicked)
                {
                    decimal secondNumber = Convert.ToDecimal(LblResult.Text);
                    finalResult = Calculate(firstNumner, secondNumber).ToString("0.##");
                    isOperatorClicked = false;
                }
                
                LblResult.Text = finalResult;
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.Message, "Ok");
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