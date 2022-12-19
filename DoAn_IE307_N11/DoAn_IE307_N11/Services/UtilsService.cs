using System;
using System.Collections.Generic;
using System.Text;

namespace DoAn_IE307_N11.Services
{
    public class UtilsService
    {
        public string GetFlagFromCode(string code)
        {
            switch (code)
            {
                case "usa_dollar":
                    return "usa_flag.png";
                    
                case "vnd":
                    return "vn_flag.png";
                    
                case "pound":
                    return "uk_flag.png";
                    
                case "euro":
                    return "eu_flag.png";

                default:
                    return "";
            }
        }

        internal string GetCurrencySymbolFromCode(string code)
        {
            switch (code)
            {
                case "usa_dollar":
                    return "$";

                case "vnd":
                    return "VNĐ";

                case "pound":
                    return "£";

                case "euro":
                    return "€";

                default:
                    return "ERROR";
            }
        }
    }
}
