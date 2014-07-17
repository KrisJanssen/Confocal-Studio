#region License

// --------------------------------------------------------------------------- 
// <copyright file="QuantityFuncParser.cs" company="Kris Janssen">
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
    using System.Linq;

    /// <summary>
    ///     The quantity func parser.
    /// </summary>
    public static class QuantityFuncParser
    {
        #region Enums

        /// <summary>
        ///     The operations.
        /// </summary>
        private enum Operations
        {
            /// <summary>
            ///     The set.
            /// </summary>
            Set, 

            /// <summary>
            ///     The add.
            /// </summary>
            Add, 

            /// <summary>
            ///     The subtract.
            /// </summary>
            Subtract
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The parse.
        /// </summary>
        /// <param name="quantityFunc">
        /// The quantity func.
        /// </param>
        /// <param name="currentQuantity">
        /// The current quantity.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string Parse(string quantityFunc, string currentQuantity)
        {
            if (!IsFunc(quantityFunc))
            {
                return quantityFunc;
            }

            int quantity;
            if (!int.TryParse(currentQuantity, out quantity) && !string.IsNullOrEmpty(currentQuantity))
            {
                return quantityFunc;
            }

            return Parse(quantityFunc, quantity).ToString();
        }

        /// <summary>
        /// The parse.
        /// </summary>
        /// <param name="quantityFunc">
        /// The quantity func.
        /// </param>
        /// <param name="currentQuantity">
        /// The current quantity.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public static int Parse(string quantityFunc, int currentQuantity)
        {
            if (string.IsNullOrEmpty(quantityFunc))
            {
                return 0;
            }

            int value;
            Operations operation = GetFunc(quantityFunc);
            string trimmed = quantityFunc.Trim('-', '+', ' ');
            int.TryParse(trimmed, out value);
            if (operation == Operations.Add)
            {
                return currentQuantity + value;
            }

            if (operation == Operations.Subtract)
            {
                return currentQuantity - value;
            }

            return value;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The contains char.
        /// </summary>
        /// <param name="set">
        /// The set.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private static bool ContainsChar(string set, char value)
        {
            return set.ToCharArray().Any(x => x == value);
        }

        /// <summary>
        /// The get func.
        /// </summary>
        /// <param name="quantityFunc">
        /// The quantity func.
        /// </param>
        /// <returns>
        /// The <see cref="Operations"/>.
        /// </returns>
        private static Operations GetFunc(string quantityFunc)
        {
            if (!IsFunc(quantityFunc))
            {
                return Operations.Set;
            }

            if (quantityFunc.StartsWith("+"))
            {
                return Operations.Add;
            }

            if (quantityFunc.StartsWith("-"))
            {
                return Operations.Subtract;
            }

            return Operations.Set;
        }

        /// <summary>
        /// The is func.
        /// </summary>
        /// <param name="quantityFunc">
        /// The quantity func.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private static bool IsFunc(string quantityFunc)
        {
            if (string.IsNullOrEmpty(quantityFunc))
            {
                return false;
            }

            if (quantityFunc.Length == 1)
            {
                return false;
            }

            char operation = quantityFunc[0];
            if ("+-".All(x => x != operation))
            {
                return false;
            }

            string value = quantityFunc.Substring(1);
            return value.All(x => ContainsChar("1234567890", x));
        }

        #endregion
    }
}