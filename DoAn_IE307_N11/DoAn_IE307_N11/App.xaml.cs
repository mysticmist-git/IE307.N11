using DoAn_IE307_N11.Models;
using DoAn_IE307_N11.Services;
using DoAn_IE307_N11.Views;
using DoAn_IE307_N11.Views.All;
using SQLite;
using System;
using System.Reflection;
using System.Transactions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: ExportFont("MaterialIconsRegular.ttf", Alias = "Material")]
namespace DoAn_IE307_N11
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<Interfaces.IMessageService, Services.MessageService>();
            DependencyService.Register<SQLiteDB>();
            DependencyService.Register<SQLiteDBAsync>();
            DependencyService.Register<LocalDataService>();
            DependencyService.Register<ConstantService>();
            DependencyService.Register<ViewService>();
            DependencyService.Register<UtilsService>();

            DependencyService.Get<LocalDataService>().InitDB();

            MainPage = new LoginPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

    }
}
