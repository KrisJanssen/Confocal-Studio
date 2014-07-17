#region License

// --------------------------------------------------------------------------- 
// <copyright file="Extensions.cs" company="Kris Janssen">
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

namespace CFStudio.Infrastructure
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Dynamic;

    /// <summary>
    ///     The extensions.
    /// </summary>
    public static class Extensions
    {
        #region Public Methods and Operators

        /// <summary>
        /// The to dynamic.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="dynamic"/>.
        /// </returns>
        public static dynamic ToDynamic(this object value)
        {
            if (value is ExpandoObject)
            {
                return value;
            }

            IDictionary<string, object> expando = new ExpandoObject();

            if (value != null)
            {
                foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(value.GetType()))
                {
                    expando.Add(property.Name, property.GetValue(value));
                }
            }

            return expando as ExpandoObject;
        }

        #endregion
    }
}