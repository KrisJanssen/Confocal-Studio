#region License

// --------------------------------------------------------------------------- 
// <copyright file="SysInfoQuery.cs" company="Kris Janssen">
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

namespace CFStudio.Infrastructure.ExceptionReporter.SystemInfo
{
    /// <summary>
    ///     The sys info query.
    /// </summary>
    internal class SysInfoQuery
    {
        #region Fields

        /// <summary>
        ///     The _name.
        /// </summary>
        private readonly string _name;

        /// <summary>
        ///     The _query text.
        /// </summary>
        private readonly string _queryText;

        /// <summary>
        ///     The _use name as display field.
        /// </summary>
        private readonly bool _useNameAsDisplayField;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SysInfoQuery"/> class.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <param name="useNameAsDisplayField">
        /// The use name as display field.
        /// </param>
        public SysInfoQuery(string name, string query, bool useNameAsDisplayField)
        {
            this._name = name;
            this._useNameAsDisplayField = useNameAsDisplayField;
            this._queryText = query;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the display field.
        /// </summary>
        public string DisplayField
        {
            get
            {
                return this._useNameAsDisplayField ? "Name" : "Caption";
            }
        }

        /// <summary>
        ///     Gets the name.
        /// </summary>
        public string Name
        {
            get
            {
                return this._name;
            }
        }

        /// <summary>
        ///     Gets the query text.
        /// </summary>
        public string QueryText
        {
            get
            {
                return this._queryText;
            }
        }

        #endregion
    }
}