#region License

// --------------------------------------------------------------------------- 
// <copyright file="DateFuncParser.cs" company="Kris Janssen">
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
    using System.Linq;

    /// <summary>
    ///     The date func parser.
    /// </summary>
    public static class DateFuncParser
    {
        #region Static Fields

        /// <summary>
        ///     The operators.
        /// </summary>
        private static readonly char[] Operators = { '+', '-' };

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The parse.
        /// </summary>
        /// <param name="expression">
        /// The expression.
        /// </param>
        /// <param name="currentValue">
        /// The current value.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string Parse(string expression, string currentValue)
        {
            string result = expression ?? string.Empty;
            string correctedExpression = result.ToLower().Trim();
            if (correctedExpression.ToLower() == "today")
            {
                result = DateTime.Today.ToShortDateString();
            }
            else if (correctedExpression.ToLower().StartsWith("today"))
            {
                result = ParseDateExpression(correctedExpression);
            }
            else if (correctedExpression.IndexOfAny(Operators) > -1)
            {
                result = ExecuteDateExpression(correctedExpression, currentValue);
            }

            return !string.IsNullOrEmpty(result) ? result : expression;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The execute date expression.
        /// </summary>
        /// <param name="expression">
        /// The expression.
        /// </param>
        /// <param name="currentValue">
        /// The current value.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string ExecuteDateExpression(string expression, string currentValue)
        {
            DateTime currentDate;
            if (!DateTime.TryParse(currentValue, out currentDate) && !string.IsNullOrEmpty(currentValue))
            {
                return string.Empty;
            }

            if (string.IsNullOrEmpty(currentValue))
            {
                currentDate = DateTime.Today;
            }

            string[] parts = expression.Split(Operators, StringSplitOptions.RemoveEmptyEntries);

            string val = parts[0].Trim();
            if (!IsNumber(val))
            {
                return string.Empty;
            }

            int quantity = Convert.ToInt32(val);

            return UpdateDate(currentDate, expression, quantity);
        }

        /// <summary>
        /// The is number.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private static bool IsNumber(string value)
        {
            return value.All(x => "1234567890".Contains(x));
        }

        /// <summary>
        /// The parse date expression.
        /// </summary>
        /// <param name="expression">
        /// The expression.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string ParseDateExpression(string expression)
        {
            string[] parts = expression.Split(Operators, StringSplitOptions.RemoveEmptyEntries);
            int quantity = 1;
            if (parts.Length > 1)
            {
                string val = parts[1].Trim();
                if (!IsNumber(val))
                {
                    return string.Empty;
                }

                quantity = Convert.ToInt32(val);
            }

            return UpdateDate(DateTime.Today, expression, quantity);
        }

        /// <summary>
        /// The update date.
        /// </summary>
        /// <param name="date">
        /// The date.
        /// </param>
        /// <param name="expression">
        /// The expression.
        /// </param>
        /// <param name="quantity">
        /// The quantity.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string UpdateDate(DateTime date, string expression, int quantity)
        {
            return expression.Contains("-")
                       ? date.AddDays(0 - quantity).ToShortDateString()
                       : date.AddDays(quantity).ToShortDateString();
        }

        #endregion
    }
}