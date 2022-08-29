using System;
using System.Linq;

namespace BasicExtensions
{
    public static class NumberExtensitons
    {
        public static decimal CalculatePercentage(this decimal value, decimal percent) => (value * percent / 100M).Round(4);
        public static double CalculatePercentage(this double value, decimal percent) => value.ToDecimal().CalculatePercentage(percent).ToDouble();
        public static float CalculatePercentage(this float value, decimal percent) => value.ToDecimal().CalculatePercentage(percent).ToFloat();
        public static decimal IncludePercentage(this decimal price, decimal percent) => Math.Round(price * (1.0M + percent / 100.0M), 4);
        public static double IncludePercentage(this double price, decimal percent) => price.ToDecimal().IncludePercentage(percent).ToDouble();
        public static float IncludePercentage(this float price, decimal percent) => price.ToDecimal().IncludePercentage(percent).ToFloat();
        public static decimal Round(this decimal value, int decimals = 2) => Math.Round(value, decimals);
        public static double Round(this double value, int decimals = 2) => Math.Round(value, decimals);
        public static float Round(this float value, int decimals = 2) => Math.Round(value.ToDecimal(), decimals).ToFloat();
        public static decimal SubtractPercentage(this decimal price, decimal percent) => Math.Round(price / (1.0M + percent / 100.0M), 4);
        public static double SubtractPercentage(this double price, decimal percent) => price.ToDecimal().SubtractPercentage(percent).ToDouble();
        public static float SubtractPercentage(this float price, decimal percent) => price.ToDecimal().SubtractPercentage(percent).ToFloat();
        public static decimal ToDecimal(this decimal? value) => value ?? 0;
        public static decimal ToDecimal(this short value) => Convert.ToDecimal(value);
        public static decimal? ToDecimal(this short? value) => value.HasValue ? Convert.ToDecimal(value.Value) : (decimal?)null;
        public static decimal ToDecimal(this int value) => Convert.ToDecimal(value);
        public static decimal? ToDecimal(this int? value) => value.HasValue ? Convert.ToDecimal(value.Value) : (decimal?)null;
        public static decimal ToDecimal(this long value) => Convert.ToDecimal(value);
        public static decimal? ToDecimal(this long? value) => value.HasValue ? Convert.ToDecimal(value.Value) : (decimal?)null;
        public static decimal ToDecimal(this float value) => Convert.ToDecimal(value);
        public static decimal? ToDecimal(this float? value) => value.HasValue ? Convert.ToDecimal(value.Value) : (decimal?)null;
        public static decimal ToDecimal(this double value) => Convert.ToDecimal(value);
        public static decimal? ToDecimal(this double? value) => value.HasValue ? Convert.ToDecimal(value.Value) : (decimal?)null;
        public static decimal ToDecimal(this string value) => decimal.TryParse(value, out decimal result) ? result : 0;
        public static decimal? ToDecimalNullable(this string value) => decimal.TryParse(value, out decimal result) ? result : (decimal?)null;
        public static double ToDouble(this double? value) => value ?? 0;
        public static double ToDouble(this short value) => Convert.ToDouble(value);
        public static double? ToDouble(this short? value) => value.HasValue ? Convert.ToDouble(value) : (double?)null;
        public static double ToDouble(this int value) => Convert.ToDouble(value);
        public static double? ToDouble(this int? value) => value.HasValue ? Convert.ToDouble(value) : (double?)null;
        public static double ToDouble(this long value) => Convert.ToDouble(value);
        public static double? ToDouble(this long? value) => value.HasValue ? Convert.ToDouble(value) : (double?)null;
        public static double ToDouble(this float value) => Convert.ToDouble(value);
        public static double? ToDouble(this float? value) => value.HasValue ? Convert.ToDouble(value) : (double?)null;
        public static double ToDouble(this decimal value) => Convert.ToDouble(value);
        public static double? ToDouble(this decimal? value) => value.HasValue ? Convert.ToDouble(value): (double?)null;
        public static double ToDouble(this string value) => double.TryParse(value, out double result) ? result : 0;
        public static double? ToDoubleNullable(this string value) => double.TryParse(value, out double result) ? result : (double?)null;
        public static float ToFloat(this float? value) => value ?? 0;
        public static float ToFloat(this short value) => Convert.ToSingle(value);
        public static float? ToFloat(this short? value) => value.HasValue ? Convert.ToSingle(value) : (float?)null;
        public static float ToFloat(this int value) => Convert.ToSingle(value);
        public static float? ToFloat(this int? value) => value.HasValue ? Convert.ToSingle(value) : (float?)null;
        public static float ToFloat(this long value) => Convert.ToSingle(value);
        public static float? ToFloat(this long? value) => value.HasValue ? Convert.ToSingle(value) : (float?)null;
        public static float ToFloat(this double value) => Convert.ToSingle(value);
        public static float? ToFloat(this double? value) => value.HasValue ? Convert.ToSingle(value) : (float?)null;
        public static float ToFloat(this decimal value) => Convert.ToSingle(value);
        public static float? ToFloat(this decimal? value) => value.HasValue ? Convert.ToSingle(value) : (float?)null;
        public static float ToFloat(this string value) => float.TryParse(value, out float result) ? result : 0;
        public static float? ToFloatNullable(this string value) => float.TryParse(value, out float result) ? result : (float?)null;
        public static int ToInt(this int? value) => value ?? 0;
        public static int ToInt(this short value) => value;
        public static int? ToInt(this short? value) => value;
        public static int ToInt(this long value) => (int)value;
        public static int? ToInt(this long? value) => (int?)value;
        public static int ToInt(this float value) => (int)value;
        public static int? ToInt(this float? value) => (int?)value;
        public static int ToInt(this double value) => (int)value;
        public static int? ToInt(this double? value) => (int?)value;
        public static int ToInt(this decimal value) => (int)value;
        public static int? ToInt(this decimal? value) => (int?)value;
        public static int ToInt(this string value) => int.TryParse(value, out int result) ? result : 0;
        public static int? ToIntNullable(this string value) => int.TryParse(value, out int result) ? result : (int?)null;
        public static long ToLong(this long? value) => value ?? 0;
        public static long ToLong(this short value) => value;
        public static long? ToLong(this short? value) => value;
        public static long ToLong(this int value) => value;
        public static long? ToLong(this int? value) => value;
        public static long ToLong(this float value) => (long)value;
        public static long? ToLong(this float? value) => (long?)value;
        public static long ToLong(this double value) => (long)value;
        public static long? ToLong(this double? value) => (long?)value;
        public static long ToLong(this decimal value) => (long)value;
        public static long? ToLong(this decimal? value) => (long?)value;
        public static long ToLong(this string value) => long.TryParse(value, out long result) ? result : 0;
        public static long? ToLongNullable(this string value) => long.TryParse(value, out long result) ? result : (long?)null;
        public static short ToShort(this short? value) => value ?? 0;
        public static short ToShort(this long value) => (short)value;
        public static short? ToShort(this long? value) => (short?)value;
        public static short ToShort(this int value) => (short)value;
        public static short? ToShort(this int? value) => (short?)value;
        public static short ToShort(this float value) => (short)value;
        public static short? ToShort(this float? value) => (short?)value;
        public static short ToShort(this double value) => (short)value;
        public static short? ToShort(this double? value) => (short?)value;
        public static short ToShort(this decimal value) => (short)value;
        public static short? ToShort(this decimal? value) => (short?)value;
        public static short ToShort(this string value) => short.TryParse(value, out short result) ? result : (short)0;
        public static short? ToShortNullable(this string value) => short.TryParse(value, out short result) ? result : (short?)null;
        public static decimal ToRealDecimal(this string value)
        {
            value = value.Trim();
            value = value.StartsWith(",") || value.StartsWith(".") ? $"0{value}" : value;
            if (string.IsNullOrWhiteSpace(value))
                return 0;
            if (value.Replace(".", "").Replace(",", "").Any(s => !char.IsDigit(s)))
                return 0;
            if (value.Any(s => s == '.' || s == ','))
            {
                var index = value.IndexOf('.') != -1 ? value.IndexOf('.') : value.IndexOf(',');
                var dS = value.Substring(0, index).ToInt();
                var dE = dS + 1;
                var result1 = value.Replace(',', '.').ToDecimal();
                var result2 = value.Replace('.', ',').ToDecimal();
                var result = result1 >= dS && result1 <= dE ? result1 : result2 >= dS && result2 <= dE ? result2 : dS;
                return result;
            }
            else
            {
                return value.ToDecimal();
            }
        }
        public static int ToProcessing(this int total, int value)
        {
            var d = total.ToDecimal().CalculatePercentage(value.ToDecimal());
            return (d < 0 ? 0 : d > 100 ? 100 : d).ToInt();
        }
    }
}
