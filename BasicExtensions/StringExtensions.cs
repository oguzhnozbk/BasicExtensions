using BasicExtensions.Attribute;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BasicExtensions
{
    public static class StringExtensions
    {
        public static string PadLeft(this object data, char d = '0', int count = 2) => data.ToString().PadLeft(count, d);
        public static string PadRight(this object data, char d = '0', int count = 2) => data.ToString().PadRight(count, d);
        public static string ToCustomSubstring(this string data, int size = 11) => (data ?? "").Length <= size ? data : data.Substring(0, size);
        public static bool IsGuid(this string value) => Guid.TryParse(value, out Guid _);
        public static string CreateId(this string prefix, string id, int length = 16, bool isYear = true, bool isMonth = false, bool isDay = false) => $"{prefix}{(isYear ? DateTime.Now.Year.ToString() : "")}{(isMonth ? DateTime.Now.Month.ToString().PadLeft() : "")}{(isDay ? DateTime.Now.Day.ToString().PadLeft() : "")}{id.PadLeft(count: length - (prefix.Length + id.Length + (isYear ? 4 : 0) + (isMonth ? 2 : 0) + (isDay ? 2 : 0)) + id.Length)}";
        public static string ToOnlyNumericValue(this string value) => Regex.Replace(value, "[^0-9]", "");
        public static bool IsValidPassword(this string password, int minCount = 8, int maxCount = 32, bool isUpper = true, bool isLower = true, bool isDigit = false, bool isSpecial = false) => password.Length < minCount || password.Length > maxCount || (isUpper && !password.AnyUpperChar()) || (isLower && !password.AnyLowerChar()) || (isDigit && !password.AnyDigitChar()) || (isSpecial && !password.AnySpecialChar()) ? false : true;
        public static bool AnyUpperChar(this string value) => value.ToArray().Any(s => char.IsUpper(s));
        public static bool AnyLowerChar(this string value) => value.ToArray().Any(s => char.IsLower(s));
        public static bool AnyDigitChar(this string value) => value.ToArray().Any(s => char.IsDigit(s));
        public static bool AnySpecialChar(this string value) => value.ToArray().Any(s => char.IsPunctuation(s));
        public static bool IsValidEmail(this string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(email);
        }
        public static bool IsValidTckn(this string tckn)
        {
            if (string.IsNullOrWhiteSpace(tckn) || !tckn.All(char.IsDigit) || tckn.Length != 11 || tckn.StartsWith("0"))
                return false;
            var singles = Convert.ToInt32(tckn[0].ToString()) + Convert.ToInt32(tckn[2].ToString()) + Convert.ToInt32(tckn[4].ToString()) + Convert.ToInt32(tckn[6].ToString()) + Convert.ToInt32(tckn[8].ToString());
            var couples = Convert.ToInt32(tckn[1].ToString()) + Convert.ToInt32(tckn[3].ToString()) + Convert.ToInt32(tckn[5].ToString()) + Convert.ToInt32(tckn[7].ToString());
            var digit10 = (singles * 7 - couples) % 10;
            var total = (singles + couples + Convert.ToInt32(tckn[9].ToString())) % 10;
            if (digit10 != Convert.ToInt32(tckn[9].ToString()))
                return false;
            if (total != Convert.ToInt32(tckn[10].ToString()))
                return false;
            else return true;
        }
        public static bool IsValidVkn(this string vkn)
        {
            if (string.IsNullOrWhiteSpace(vkn) || !vkn.All(char.IsDigit) || vkn.Length != 10)
                return false;
            var v1 = (Convert.ToInt32(vkn[0].ToString()) + 9) % 10;
            var v2 = (Convert.ToInt32(vkn[1].ToString()) + 8) % 10;
            var v3 = (Convert.ToInt32(vkn[2].ToString()) + 7) % 10;
            var v4 = (Convert.ToInt32(vkn[3].ToString()) + 6) % 10;
            var v5 = (Convert.ToInt32(vkn[4].ToString()) + 5) % 10;
            var v6 = (Convert.ToInt32(vkn[5].ToString()) + 4) % 10;
            var v7 = (Convert.ToInt32(vkn[6].ToString()) + 3) % 10;
            var v8 = (Convert.ToInt32(vkn[7].ToString()) + 2) % 10;
            var v9 = (Convert.ToInt32(vkn[8].ToString()) + 1) % 10;
            var v_last_digit = Convert.ToInt32(vkn[9].ToString());

            var v11 = (v1 * 512) % 9;
            var v22 = (v2 * 256) % 9;
            var v33 = (v3 * 128) % 9;
            var v44 = (v4 * 64) % 9;
            var v55 = (v5 * 32) % 9;
            var v66 = (v6 * 16) % 9;
            var v77 = (v7 * 8) % 9;
            var v88 = (v8 * 4) % 9;
            var v99 = (v9 * 2) % 9;

            if (v1 != 0 && v11 == 0) v11 = 9;
            if (v2 != 0 && v22 == 0) v22 = 9;
            if (v3 != 0 && v33 == 0) v33 = 9;
            if (v4 != 0 && v44 == 0) v44 = 9;
            if (v5 != 0 && v55 == 0) v55 = 9;
            if (v6 != 0 && v66 == 0) v66 = 9;
            if (v7 != 0 && v77 == 0) v77 = 9;
            if (v8 != 0 && v88 == 0) v88 = 9;
            if (v9 != 0 && v99 == 0) v99 = 9;
            var total = v11 + v22 + v33 + v44 + v55 + v66 + v77 + v88 + v99;

            if (total % 10 == 0) total = 0;
            else total = (10 - (total % 10));
            if (total == v_last_digit)
                return true;
            return false;
        }
        public static bool IsValidVknOrTckn(this string kno) => kno.IsValidTckn() || kno.IsValidVkn();
        public static bool IsValidPhoneFormat(this string phone) => phone.ToPhoneFormatLength10().Length == 10;
        public static string ToPhoneFormatLength10(this string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return "";
            var phoneNumber = phone.ToOnlyNumericValue();
            if (phoneNumber.Length == 10)
                return phoneNumber;
            if (phoneNumber.StartsWith("90") && phoneNumber.Length == 12)
                return phoneNumber.Substring(2, phoneNumber.Length - 2);
            if (phoneNumber.StartsWith("0") && phoneNumber.Length == 11)
                return phoneNumber.Substring(1, phoneNumber.Length - 1);
            return "";
        }
        public static string ToPhoneFormat(this string phone, string format = Models.PhoneFormat.Format1)
        {
            if (!phone.IsValidPhoneFormat())
                return "";
            var p = phone.ToPhoneFormatLength10();
            switch (format)
            {
                case Models.PhoneFormat.Format1:
                    return p;
                case Models.PhoneFormat.Format2:
                    return $"0{p}";
                case Models.PhoneFormat.Format3:
                    return $"0 {p.Substring(0, 3)} {p.Substring(3, 3)} {p.Substring(6, 4)}";
                case Models.PhoneFormat.Format4:
                    return $"0 {p.Substring(0, 3)} {p.Substring(3, 3)} {p.Substring(6, 2)} {p.Substring(8, 2)}";
                case Models.PhoneFormat.Format5:
                    return $"0 ({p.Substring(0, 3)}) {p.Substring(3, 3)} {p.Substring(6, 4)}";
                case Models.PhoneFormat.Format6:
                    return $"0 ({p.Substring(0, 3)}) {p.Substring(3, 3)} {p.Substring(6, 2)} {p.Substring(8, 2)}";
                case Models.PhoneFormat.Format7:
                    return $"90 ({p.Substring(0, 3)}) {p.Substring(3, 3)} {p.Substring(6, 4)}";
                case Models.PhoneFormat.Format8:
                    return $"90 ({p.Substring(0, 3)}) {p.Substring(3, 3)} {p.Substring(6, 2)} {p.Substring(8, 2)}";
                case Models.PhoneFormat.Format9:
                    return $"+90 ({p.Substring(0, 3)}) {p.Substring(3, 3)} {p.Substring(6, 4)}";
                case Models.PhoneFormat.Format10:
                    return $"+90 ({p.Substring(0, 3)}) {p.Substring(3, 3)} {p.Substring(6, 2)} {p.Substring(8, 2)}";
                default:
                    return p;
            }
        }
        public static string ToHtmlTable<T>(this List<T> entities, string cssId = null, string cssClass = null)
        {
            var html = new StringBuilder();
            html.Append($"<table id='{(string.IsNullOrWhiteSpace(cssId) ? "tableData" : cssId)}' class='{(string.IsNullOrWhiteSpace(cssClass) ? "table table-hover" : cssClass)}'>");
            html.Append("<thead>");
            html.Append("<tr>");
            var type = typeof(T);
            foreach (var property in type.GetProperties())
            {
                if (!property.CanRead || !property.CanWrite)
                    continue;
                if (!TypeDescriptor.GetConverter(property.PropertyType).CanConvertFrom(typeof(string)))
                    continue;
                var colAttr = property.GetCustomAttributes(typeof(HtmlTableColumnAttribute), false);
                string name;
                if (colAttr == null || colAttr.Length == 0)
                    name = property.Name;
                else
                    name = (colAttr[0] as HtmlTableColumnAttribute).PropertyName;
                html.Append($"<th>{name}</th>");
            }
            html.Append("</tr>");
            html.Append("</thead>");
            html.Append("<tbody>");
            foreach (var item in entities)
            {
                html.Append("<tr>");
                foreach (var property in type.GetProperties())
                {
                    if (!property.CanRead || !property.CanWrite)
                        continue;
                    if (!TypeDescriptor.GetConverter(property.PropertyType).CanConvertFrom(typeof(string)))
                        continue;
                    var value = property.GetValue(item, null);
                    html.Append($"<td>{value}</td>");
                }
                html.Append("</tr>");
            }
            html.Append("</tbody>");
            html.Append("</table>");
            return default;
        }
        public static string ToString(this bool value, string correct, string wrong) => value ? correct : wrong;
        public static string ToStringJoin<T>(this IEnumerable<T> list, string separator) => list != null && list.Any() ? string.Join(separator, list) : "";
        public static string TrimWithNullCheck(this string value) => !string.IsNullOrWhiteSpace(value) ? value.Trim() : null;
        public static List<string> SplitWithNullCheck(this string value, char separator) => !string.IsNullOrWhiteSpace(value) ? value.Split(separator).ToList() : new List<string>();
        public static string ReplaceWithNullCheck(this string value, string oldVal, string newVal) => !string.IsNullOrWhiteSpace(value) ? value.Replace(oldVal, newVal) : null;
        public static string RemoveEnd(this string value, int length) => !string.IsNullOrWhiteSpace(value) && value.Length > length ? value.Substring(0, value.Length - length) : null;
        public static string RemoveStart(this string value, int length) => !string.IsNullOrWhiteSpace(value) && value.Length > length ? value.Substring(length) : null;
        public static string Remove(this string value, params string[] arg)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;
            foreach (var item in arg)
            {
                value = value.Replace(item, "");
            }
            return value;
        }
        public static byte[] ToBytes(this string value) => Encoding.UTF8.GetBytes(value);
        public static string ToCapitalize(this string value) => !string.IsNullOrWhiteSpace(value) ? value.Length > 1 ? $"{value.Substring(0, 1).ToUpper()}{value.Substring(1).ToLower()}" : value.ToUpper() : null;
        public static string ToCapitalize(this string value, char separator) => !string.IsNullOrWhiteSpace(value) ? value.Split(separator).Select(s => s.ToCapitalize()).ToStringJoin(" ") : null;
        public static bool IsNullOrWhiteSpace(this string value) => string.IsNullOrWhiteSpace(value);
        public static bool HasValue(this string value) => !value.IsNullOrWhiteSpace();

    }
}
