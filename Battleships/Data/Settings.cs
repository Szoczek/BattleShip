using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Battleships.Data
{
    class Settings : IDataErrorInfo
    {
        public int Height { get; set; } = 900;
        public int Width { get; set; } = 900;
        public int ShipLimit { get; set; } = 20;
        public string Location { get; set; }
        public string CountryTag { get; set; }

        public string GetLocalization()
        {
            return Location.ToUpper() + "," + CountryTag.ToUpper();
        }

        public string this[string columnName]
        {
            get
            {
                var sb = new StringBuilder();
                if (string.IsNullOrWhiteSpace(columnName) || columnName == nameof(Location))
                {
                    if (string.IsNullOrWhiteSpace(Location))
                    {
                        sb.AppendLine("Localization field cannot be empty!");
                    }
                    if (!Regex.IsMatch(Location ?? " ", @"^[a-zA-Z]+$"))
                    {
                        sb.AppendLine("Localization field should consist only letters!");
                    }
                }
                if (string.IsNullOrWhiteSpace(columnName) || columnName == nameof(CountryTag))
                {
                    if (string.IsNullOrWhiteSpace(CountryTag))
                    {
                        sb.AppendLine("Country code field cannot be empty!");
                    }
                    if (!Regex.IsMatch(CountryTag ?? " ", @"^[a-zA-Z]+$") || (CountryTag.Count() < 2) || (CountryTag.Count() > 3))
                    {
                        sb.AppendLine("Country code field should be consisted of 2-3 letters!");
                    }
                }
                return sb.ToString();
            }
        }

        public string Error
        {
            get { return this[string.Empty]; }
        }
    }
}