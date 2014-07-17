#region License

// --------------------------------------------------------------------------- 
// <copyright file="ExceptionReportGenerator.cs" company="Kris Janssen">
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

namespace CFStudio.Infrastructure.ExceptionReporter
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using CFStudio.Infrastructure.ExceptionReporter.SystemInfo;

    /// <summary>
    ///     The exception report generator.
    /// </summary>
    public class ExceptionReportGenerator : Disposable
    {
        #region Fields

        /// <summary>
        ///     The _report info.
        /// </summary>
        private readonly ExceptionReportInfo _reportInfo;

        /// <summary>
        ///     The _sys info results.
        /// </summary>
        private readonly List<SysInfoResult> _sysInfoResults = new List<SysInfoResult>();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionReportGenerator"/> class.
        /// </summary>
        /// <param name="reportInfo">
        /// The report info.
        /// </param>
        /// <exception cref="ExceptionReportGeneratorException">
        /// </exception>
        public ExceptionReportGenerator(ExceptionReportInfo reportInfo)
        {
            if (reportInfo == null)
            {
                throw new ExceptionReportGeneratorException("reportInfo cannot be null");
            }

            this._reportInfo = reportInfo;

            this._reportInfo.ExceptionDate = DateTime.UtcNow;
            this._reportInfo.UserName = Environment.UserName;
            this._reportInfo.MachineName = Environment.MachineName;

            if (this._reportInfo.AppAssembly == null)
            {
                this._reportInfo.AppAssembly = Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly();
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The create exception report.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public string CreateExceptionReport()
        {
            IList<SysInfoResult> sysInfoResults = this.GetOrFetchSysInfoResults();
            var reportBuilder = new ExceptionReportBuilder(this._reportInfo, sysInfoResults);
            return reportBuilder.Build();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     The get or fetch sys info results.
        /// </summary>
        /// <returns>
        ///     The <see cref="IList" />.
        /// </returns>
        internal IList<SysInfoResult> GetOrFetchSysInfoResults()
        {
            if (this._sysInfoResults.Count == 0)
            {
                this._sysInfoResults.AddRange(CreateSysInfoResults());
            }

            return this._sysInfoResults.AsReadOnly();
        }

        /// <summary>
        ///     The dispose managed resources.
        /// </summary>
        protected override void DisposeManagedResources()
        {
            this._reportInfo.Dispose();
            base.DisposeManagedResources();
        }

        /// <summary>
        ///     The create sys info results.
        /// </summary>
        /// <returns>
        ///     The <see cref="IEnumerable" />.
        /// </returns>
        private static IEnumerable<SysInfoResult> CreateSysInfoResults()
        {
            var retriever = new SysInfoRetriever();
            var results = new List<SysInfoResult>
                              {
                                  retriever.Retrieve(SysInfoQueries.OperatingSystem)
                                      .Filter(
                                          new[]
                                              {
                                                  "CodeSet", "CurrentTimeZone", "FreePhysicalMemory", 
                                                  "OSArchitecture", "OSLanguage", "Version"
                                              }), 
                                  retriever.Retrieve(SysInfoQueries.Machine)
                                      .Filter(
                                          new[]
                                              {
                                                  "Machine", "UserName", "TotalPhysicalMemory", 
                                                  "Manufacturer", "Model"
                                              }), 
                              };
            return results;
        }

        #endregion
    }

    /// <summary>
    ///     The exception report generator exception.
    /// </summary>
    [Serializable]
    internal class ExceptionReportGeneratorException : Exception
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionReportGeneratorException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public ExceptionReportGeneratorException(string message)
            : base(message)
        {
        }

        #endregion
    }
}