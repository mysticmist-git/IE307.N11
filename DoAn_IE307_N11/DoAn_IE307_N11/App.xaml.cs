using DoAn_IE307_N11.Models;
using DoAn_IE307_N11.Services;
using DoAn_IE307_N11.Views;
using SQLite;
using System;
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

            Services.SQLiteDB.Db.CreateTable<TransactionType>();
            Services.SQLiteDB.Db.CreateTable<Wallet>();
            //Services.SQLiteDB.Db.CreateTable<Event>();
            Services.SQLiteDB.Db.CreateTable<Models.Transaction>();
            Services.SQLiteDB.Db.CreateTable<User>();

            GenerateTransactionType();
            //GenerateTransactions();

            MainPage = new AppShell();
        }

        

        private void GenerateTransactionType()
        {
            if (Services.SQLiteDB.Db.Table<TransactionType>().Count() > 0)
                return;
            
            Services.SQLiteDB.Db.InsertAll(new[]
            {
                new TransactionType
                {
                    Name = "Ăn uống",
                    Image = "AnUong.png",
                    IsIncome = false
                },
                new TransactionType
                {
                    Name = "Bảo dưỡng xe",
                    Image = "BaoDuongXe.png",
                    IsIncome = false
                },
                new TransactionType
                {
                    Name = "Bảo hiểm",
                    Image = "BaoHiem.png",
                    IsIncome = false
                },
                new TransactionType
                {
                    Name = "Các chi phí khác",
                    Image = "CacChiPhiKhac.png",
                    IsIncome = false
                },
                new TransactionType
                {
                    Name = "Cho vay",
                    Image = "ChoVay.png",
                    IsIncome = false
                },
                new TransactionType
                {
                    Name = "Đầu tư",
                    Image = "DauTu.png",
                    IsIncome = false
                },
                new TransactionType
                {
                    Name = "Di chuyển",
                    Image = "DiChuyen.png",
                    IsIncome = false
                },
                new TransactionType
                {
                    Name = "Dịch vụ gia đình",
                    Image = "DichVuGiaDinh.png",
                    IsIncome = false
                },
                new TransactionType
                {
                    Name = "Dịch vụ trực tuyến",
                    Image = "DichVuTrucTuyen.png",
                    IsIncome = false
                },
                new TransactionType
                {
                    Name = "Đi vay",
                    Image = "DiVay.png"
                },
                new TransactionType
                {
                    Name = "Đồ dùng gia đình",
                    Image = "DoDungGiaDinh.png",
                    IsIncome = false
                },
                new TransactionType
                {
                    Name = "Đồ gia dụng",
                    Image = "DoGiaDung.png",
                    IsIncome = false
                },
                new TransactionType
                {
                    Name = "Giáo dục",
                    Image = "GiaoDuc.png",
                    IsIncome = false
                },
                new TransactionType
                {
                    Name = "Hóa đơn điện",
                    Image = "HoaDonDien.png",
                    IsIncome = false
                },
                new TransactionType
                {
                    Name = "Hóa đơn điện thoại",
                    Image = "HoaDonDienThoai.png",
                    IsIncome = false
                },
                new TransactionType
                {
                    Name = "Hóa đơn gas",
                    Image = "HoaDonGas.png",
                    IsIncome = false
                },
                new TransactionType
                {
                    Name = "Hóa đơn Internet",
                    Image = "HoaDonInternet.png",
                    IsIncome = false
                },
                new TransactionType
                {
                    Name = "Hóa đơn nước",
                    Image = "HoaDonNuoc.png",
                    IsIncome = false
                },
                new TransactionType
                {
                    Name = "Hóa đơn tiện ích khác",
                    Image = "HoaDonTienIchKhac.png",
                    IsIncome = false
                },
                new TransactionType
                {
                    Name = "Hóa đơn TV",
                    Image = "HoaDonTV.png",
                    IsIncome = false
                },
                new TransactionType
                {
                    Name = "Khám sức khỏe",
                    Image = "KhamSucKhoe.png",
                    IsIncome = false
                },
                new TransactionType
                {
                    Name = "Làm đẹp",
                    Image = "LamDep.png",
                    IsIncome = false
                },
                new TransactionType
                {
                    Name = "Lương",
                    Image = "Luong.png"
                },
                new TransactionType
                {
                    Name = "Quà tặng - Quyên góp",
                    Image = "QuaTang_QuyenGop.png",
                    IsIncome = false
                },
                new TransactionType
                {
                    Name = "Sửa - Trang trí nhà",
                    Image = "Sua_TrangTriNha.png",
                    IsIncome = false
                },
                new TransactionType
                {
                    Name = "Thẻ dục thể thao",
                    Image = "TheDucTheThao.png",
                    IsIncome = false
                },
                new TransactionType
                {
                    Name = "Thuê nhà",
                    Image = "ThueNha.png",
                    IsIncome = false
                },
                new TransactionType
                {
                    Name = "Thu nhập khác",
                    Image = "ThuNhapKhac.png"
                },
                new TransactionType
                {
                    Name = "Thu nợ",
                    Image = "ThuNo.png"
                },
                new TransactionType
                {
                    Name = "Tiền chuyển đến",
                    Image = "TienChuyenDen.png"
                },
                new TransactionType
                {
                    Name = "Tiền chuyển đi",
                    Image = "TienChuyenDi.png",
                    IsIncome = false
                },
                new TransactionType
                {
                    Name = "Trả lãi",
                    Image = "TraLai.png",
                    IsIncome = false
                },
                new TransactionType
                {
                    Name = "Trả nợ",
                    Image = "TraNo.png",
                    IsIncome = false
                },
                new TransactionType
                {
                    Name = "Vật nuôi",
                    Image = "VatNuoi.png",
                    IsIncome = false
                },
                new TransactionType
                {
                    Name = "Vui chơi",
                    Image = "VuiChoi.png",
                    IsIncome = false
                }
            });
        }

        private void GenerateTransactions()
        {
            Services.SQLiteDB.Db.InsertAll(new[]
            {
                new Models.Transaction
                {
                    DateTime = new DateTime(2022, 11, 14),
                    Description = "Hello",
                    Amount = 25000,
                    TransactionTypeId = 1,
                    WalletId = 1
                },
                new Models.Transaction
                {
                    DateTime = new DateTime(2022, 11, 14),
                    Description = "Bye",
                    Amount = 35000,
                    TransactionTypeId = 2,
                    WalletId = 1
                }
            });
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
