#region License

// --------------------------------------------------------------------------- 
// <copyright file="SysInfoRetriever.cs" company="Kris Janssen">
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
    using System;
    using System.Collections.Generic;
    using System.Management;

    /// <summary>
    ///     Retrieves system information using WMI
    /// </summary>
    internal class SysInfoRetriever : IDisposable
    {
        #region Fields

        /// <summary>
        ///     The _sys info query.
        /// </summary>
        private SysInfoQuery _sysInfoQuery;

        /// <summary>
        ///     The _sys info result.
        /// </summary>
        private SysInfoResult _sysInfoResult;

        /// <summary>
        ///     The _sys info searcher.
        /// </summary>
        private ManagementObjectSearcher _sysInfoSearcher;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The dispose.
        /// </summary>
        public void Dispose()
        {
            this._sysInfoSearcher.Dispose();
        }

        /// <summary>
        /// Retrieve system information, using the given SysInfoQuery to determine what information to retrieve
        /// </summary>
        /// <param name="sysInfoQuery">
        /// the query to determine what information to retrieve
        /// </param>
        /// <returns>
        /// a SysInfoResult ie containing the results of the query
        /// </returns>
        public SysInfoResult Retrieve(SysInfoQuery sysInfoQuery)
        {
            this._sysInfoQuery = sysInfoQuery;
            this._sysInfoSearcher =
                new ManagementObjectSearcher(string.Format("SELECT * FROM {0}", this._sysInfoQuery.QueryText));
            this._sysInfoResult = new SysInfoResult(this._sysInfoQuery.Name);

            foreach (ManagementObject managementObject in this._sysInfoSearcher.Get())
            {
                this._sysInfoResult.AddNode(
                    managementObject.GetPropertyValue(this._sysInfoQuery.DisplayField).ToString().Trim());
                this._sysInfoResult.AddChildren(this.GetChildren(managementObject));
            }

            return this._sysInfoResult;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The get children.
        /// </summary>
        /// <param name="managementObject">
        /// The management object.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        private IEnumerable<SysInfoResult> GetChildren(ManagementBaseObject managementObject)
        {
            SysInfoResult childResult = null;
            ICollection<SysInfoResult> childList = new List<SysInfoResult>();

            foreach (PropertyData propertyData in managementObject.Properties)
            {
                if (childResult == null)
                {
                    childResult = new SysInfoResult(this._sysInfoQuery.Name + "_Child");
                    childList.Add(childResult);
                }

                string nodeValue = string.Format("{0} = {1}", propertyData.Name, Convert.ToString(propertyData.Value));
                childResult.Nodes.Add(nodeValue);
            }

            return childList;
        }

        #endregion
    }
}