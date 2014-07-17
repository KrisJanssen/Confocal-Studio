#region License

// --------------------------------------------------------------------------- 
// <copyright file="Utility.cs" company="Kris Janssen">
//   Copyright (c) 2014-2014 Kris Janssen
// </copyright>
//  This file is part of CFStudio.
//  CFStudio is an open source tool foor confocal microscopy.
//  Parts of the CFStudio codebase were re-used, with permission from
//  the SambaPOS project <https://github.com/emreeren/SambaPOS-3>.
//  CFStudio is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
// --------------------------------------------------------------------------- 
#endregion

namespace CFStudio.Infrastructure.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    ///     The utility.
    /// </summary>
    public static class Utility
    {
        #region Public Methods and Operators

        /// <summary>
        /// The add typed value.
        /// </summary>
        /// <param name="actualValue">
        /// The actual value.
        /// </param>
        /// <param name="typedValue">
        /// The typed value.
        /// </param>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string AddTypedValue(string actualValue, string typedValue, string format)
        {
            decimal amnt;
            bool stringMode = false;

            decimal.TryParse(actualValue, out amnt);
            if (actualValue.EndsWith("-") || amnt == 0)
            {
                stringMode = true;
            }
            else
            {
                decimal.TryParse(typedValue, out amnt);
                if (amnt == 0)
                {
                    stringMode = true;
                }
            }

            string dc = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

            if (typedValue == "." || typedValue == ",")
            {
                actualValue += dc;
                return actualValue;
            }

            if (stringMode)
            {
                return actualValue + typedValue;
            }

            string fmt = "0";
            string rfmt = format;

            if (actualValue.Contains(dc))
            {
                int dCount = actualValue.Length - actualValue.IndexOf(dc);

                fmt = "0.".PadRight(dCount + 2, '0');
                rfmt = format.PadRight(dCount + rfmt.Length, '0');
            }

            string amount = string.IsNullOrEmpty(actualValue) ? "0" : Convert.ToDouble(actualValue).ToString(fmt);
            if (amount.Contains(dc))
            {
                amount = amount.Substring(0, amount.Length - 1);
            }

            double dbl = Convert.ToDouble(amount + typedValue);
            return dbl.ToString(rfmt);
        }

        /// <summary>
        /// The generate check digit.
        /// </summary>
        /// <param name="idWithoutCheckdigit">
        /// The id without checkdigit.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        public static int GenerateCheckDigit(string idWithoutCheckdigit)
        {
            const string validChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVYWXZ_";
            idWithoutCheckdigit = idWithoutCheckdigit.Trim().ToUpper();

            int sum = 0;

            for (int i = 0; i < idWithoutCheckdigit.Length; i++)
            {
                char ch = idWithoutCheckdigit[idWithoutCheckdigit.Length - i - 1];
                if (validChars.IndexOf(ch) == -1)
                {
                    throw new Exception(ch + " is an invalid character");
                }

                int digit = ch - 48;
                int weight;
                if (i % 2 == 0)
                {
                    weight = (2 * digit) - digit / 5 * 9;
                }
                else
                {
                    weight = digit;
                }

                sum += weight;
            }

            sum = Math.Abs(sum) + 10;
            return (10 - (sum % 10)) % 10;
        }

        /// <summary>
        ///     The get date based unique string.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public static string GetDateBasedUniqueString()
        {
            DateTime date = DateTime.Now;
            return string.Format(
                "{0}{1:00}{2:00}{3:00}{4:00}{5:000}", 
                date.Year, 
                date.Month, 
                date.Day, 
                date.Hour, 
                date.Minute, 
                date.Millisecond);
        }

        /// <summary>
        /// The is numeric type.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsNumericType(Type type)
        {
            if (type == null)
            {
                return false;
            }

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.Single:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return true;
                case TypeCode.Object:
                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        return IsNumericType(Nullable.GetUnderlyingType(type));
                    }

                    return false;
            }

            return false;
        }

        /// <summary>
        /// The is valid file.
        /// </summary>
        /// <param name="fileName">
        /// The file name.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsValidFile(string fileName)
        {
            fileName = fileName.Trim();
            if (fileName == "." || !fileName.Contains("."))
            {
                return false;
            }

            bool result = false;
            try
            {
                new FileInfo(fileName);
                result = true;
            }
            catch (ArgumentException)
            {
            }
            catch (PathTooLongException)
            {
            }
            catch (NotSupportedException)
            {
            }

            return result;
        }

        /// <summary>
        /// The random string.
        /// </summary>
        /// <param name="length">
        /// The length.
        /// </param>
        /// <param name="allowedChars">
        /// The allowed chars.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// </exception>
        /// <exception cref="ArgumentException">
        /// </exception>
        public static string RandomString(
            int length, 
            string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789")
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException("length", "length cannot be less than zero.");
            }

            if (string.IsNullOrEmpty(allowedChars))
            {
                throw new ArgumentException("allowedChars may not be empty.");
            }

            const int byteSize = 0x100;
            char[] allowedCharSet = new HashSet<char>(allowedChars).ToArray();
            if (byteSize < allowedCharSet.Length)
            {
                throw new ArgumentException(
                    string.Format("allowedChars may contain no more than {0} characters.", byteSize));
            }

            using (var rng = new RNGCryptoServiceProvider())
            {
                var result = new StringBuilder();
                var buf = new byte[128];
                while (result.Length < length)
                {
                    rng.GetBytes(buf);
                    for (int i = 0; i < buf.Length && result.Length < length; ++i)
                    {
                        int outOfRangeStart = byteSize - (byteSize % allowedCharSet.Length);
                        if (outOfRangeStart <= buf[i])
                        {
                            continue;
                        }

                        result.Append(allowedCharSet[buf[i] % allowedCharSet.Length]);
                    }
                }

                return result.ToString();
            }
        }

        /// <summary>
        /// The validate check digit.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool ValidateCheckDigit(string id)
        {
            if (id.Length < 2)
            {
                return false;
            }

            int cd = Convert.ToInt32(id.Last().ToString());
            return cd == GenerateCheckDigit(id.Remove(id.Length - 1));
        }

        #endregion
    }
}