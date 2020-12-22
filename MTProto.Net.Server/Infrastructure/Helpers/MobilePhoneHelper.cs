using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MTProto.NET.Server.Infrastructure.Helpers
{
    public class MobilePhoneHelper 
    {
        private string _phone;
        private static bool IsDigitOnly(string numberString)
        {
            return Regex.IsMatch(numberString, @"^[0-9]*$");
        }
        private static bool isValidLocalPhone(string phone)
        {
            return !string.IsNullOrWhiteSpace(phone) && phone.StartsWith("0") && phone.Length > 10 && IsDigitOnly(phone);
        }
        private static bool IsValidOperatorCode(string code)
        {
            return !string.IsNullOrWhiteSpace(code) && code.Length > 3 && code.StartsWith("0") &&
                code[1] != '0';
        }
        private static bool IsValidCountryCode(string code)
        {
            return !string.IsNullOrWhiteSpace(code) && code.Length == 2;
        }
        
        
        public MobilePhoneHelper Parse(string phone)
        {
            var country_code = "";
            _phone = phone ?? "";
            if (!string.IsNullOrWhiteSpace(phone))
            {
                phone = phone.Trim();
                if (!phone.StartsWith("+") && !phone.StartsWith("00") && !phone.StartsWith("0") && phone.Length > 10)
                {
                    phone = "+" + phone;
                }
                if (phone.StartsWith("+"))
                {
                    phone = phone.Substring(1, phone.Length - 1);
                    country_code = phone.Length > 2 ? phone.Substring(0,2) : "INVALID";
                }
                if (phone.StartsWith("00"))
                {
                    phone = phone.Substring(2, phone.Length - 2);
                    country_code = phone.Length > 2 ? phone.Substring(0,2) : "INVALID";
                }
                if (phone.StartsWith("98"))
                {
                    phone = phone.Substring(2, phone.Length - 2);
                    country_code = "98";
                }
                if (phone.StartsWith("0"))
                    phone = phone.Substring(1, phone.Length - 1);
                country_code = string.IsNullOrWhiteSpace(country_code) ? "98" : country_code;
                _phone = string.Format("+{0}{1}", country_code, phone);
            }


            return this;
        }

        public bool Equals(string other)
        {
            return Equals(new MobilePhoneHelper().Parse(other));
        }
        public bool Equals(MobilePhoneHelper other)
        {
            return other != null && other.IsValid && other.AsIntlPhone == this.AsIntlPhone;
        }
        public string Local
        {
            get
            {
                var result = _phone.Length < 3 ? "" : "0" + _phone.Substring(3, _phone.Length - 3);
                return isValidLocalPhone(result) ? result : null;
            }
        }
        public string OperatorCode
        {
            get
            {
                var local = Local;
                var result = local.Length > 4 ? local.Substring(0, 4) : "";
                return IsValidOperatorCode(result) ? result : null;
            }
        }
        public bool IsValid
        {
            get
            {
                return isValidLocalPhone(Local) && IsValidOperatorCode(OperatorCode) && IsValidCountryCode(CountryCode);
            }
        }
        public string AsLocalPhone
        {
            get
            {
                return Local;
            }
        }
        public string AsIntlPhone
        {
            get { return _phone; }
        }
        public string AsTelegramFriendly
        {
            get
            {
                return !string.IsNullOrWhiteSpace(_phone) && _phone.Length > 1
                    ? _phone.Substring(1, _phone.Length - 1)
                    : null;
            }
        }
        public string CountryCode
        {
            get
            {
                var result = !string.IsNullOrWhiteSpace(_phone) && _phone.Length > 3
                    ? _phone.Substring(1, 2)
                    : null;
                return IsValidCountryCode(result) ? result : null;
            }
        }
    }
}
