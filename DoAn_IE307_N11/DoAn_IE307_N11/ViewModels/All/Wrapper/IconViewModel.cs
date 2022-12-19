using DoAn_IE307_N11.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoAn_IE307_N11.ViewModels.Transaction.Wrapper
{
    public class IconViewModel
    {
        public string Image { get; set; }
        public Icon Info { get; set; }

        public IconViewModel(Icon icon)
        {
            this.Info = icon;
        }
    }
}
