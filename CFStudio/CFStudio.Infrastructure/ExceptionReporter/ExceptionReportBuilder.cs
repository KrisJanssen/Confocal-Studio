#region License

// --------------------------------------------------------------------------- 
// <copyright file="ExceptionReportBuilder.cs" company="Kris Janssen">
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
    using System.Text;

    using CFStudio.Infrastructure.ExceptionReporter.SystemInfo;
    using CFStudio.Infrastructure.Settings;

    /// <summary>
    ///     The exception report builder.
    /// </summary>
    internal class ExceptionReportBuilder
    {
        #region Fields

        /// <summary>
        ///     The _report info.
        /// </summary>
        private readonly ExceptionReportInfo _reportInfo;

        /// <summary>
        ///     The _sys info results.
        /// </summary>
        private readonly IEnumerable<SysInfoResult> _sysInfoResults;

        /// <summary>
        ///     The _string builder.
        /// </summary>
        private StringBuilder _stringBuilder;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionReportBuilder"/> class.
        /// </summary>
        /// <param name="reportInfo">
        /// The report info.
        /// </param>
        public ExceptionReportBuilder(ExceptionReportInfo reportInfo)
        {
            this._reportInfo = reportInfo;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionReportBuilder"/> class.
        /// </summary>
        /// <param name="reportInfo">
        /// The report info.
        /// </param>
        /// <param name="sysInfoResults">
        /// The sys info results.
        /// </param>
        public ExceptionReportBuilder(ExceptionReportInfo reportInfo, IEnumerable<SysInfoResult> sysInfoResults)
            : this(reportInfo)
        {
            this._sysInfoResults = sysInfoResults;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The create references string.
        /// </summary>
        /// <param name="assembly">
        /// The assembly.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string CreateReferencesString(Assembly assembly)
        {
            var stringBuilder = new StringBuilder();

            foreach (AssemblyName assemblyName in assembly.GetReferencedAssemblies())
            {
                stringBuilder.AppendLine(string.Format("{0}, Version={1}", assemblyName.Name, assemblyName.Version));
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        ///     The build.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public string Build()
        {
            this._stringBuilder = new StringBuilder().AppendLine("-----------------------------");

            this.BuildGeneralInfo();
            this.BuildExceptionInfo();
            this.BuildAssemblyInfo();
            this.BuildSysInfo();

            return this._stringBuilder.ToString();
        }

        #endregion

        #region Methods

        /// <summary>
        /// The exception hierarchy to string.
        /// </summary>
        /// <param name="exception">
        /// The exception.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string ExceptionHierarchyToString(Exception exception)
        {
            Exception currentException = exception;
            var stringBuilder = new StringBuilder();
            int count = 0;

            while (currentException != null)
            {
                if (count++ == 0)
                {
                    stringBuilder.AppendLine("Top-level Exception");
                }
                else
                {
                    stringBuilder.AppendLine("Inner Exception " + (count - 1));
                }

                stringBuilder.AppendLine("Type:        " + currentException.GetType())
                    .AppendLine("Message:     " + currentException.Message)
                    .AppendLine("Source:      " + currentException.Source);

                if (currentException.StackTrace != null)
                {
                    stringBuilder.AppendLine("Stack Trace: " + currentException.StackTrace.Trim());
                }

                stringBuilder.AppendLine();
                currentException = currentException.InnerException;
            }

            string exceptionString = stringBuilder.ToString();
            return exceptionString.TrimEnd();
        }

        /// <summary>
        ///     The build assembly info.
        /// </summary>
        private void BuildAssemblyInfo()
        {
            this._stringBuilder.AppendLine("[Assembly Info]")
                .AppendLine()
                .AppendLine(CreateReferencesString(this._reportInfo.AppAssembly))
                .AppendLine("-----------------------------")
                .AppendLine();
        }

        /// <summary>
        ///     The build exception info.
        /// </summary>
        private void BuildExceptionInfo()
        {
            for (int index = 0; index < this._reportInfo.Exceptions.Count; index++)
            {
                Exception exception = this._reportInfo.Exceptions[index];

                this._stringBuilder.AppendLine(string.Format("[Exception Info {0}]", index + 1))
                    .AppendLine()
                    .AppendLine(ExceptionHierarchyToString(exception))
                    .AppendLine()
                    .AppendLine("-----------------------------")
                    .AppendLine();
            }
        }

        /// <summary>
        ///     The build general info.
        /// </summary>
        private void BuildGeneralInfo()
        {
            this._stringBuilder.AppendLine("[General Info]")
                .AppendLine()
                .AppendLine("Application: CFStudio")
                .AppendLine("Version:     " + LocalSettings.AppVersion)
                .AppendLine("Region:      " + LocalSettings.CurrentLanguage)
                .AppendLine("DB:          " + LocalSettings.DatabaseLabel)
                .AppendLine("Machine:     " + this._reportInfo.MachineName)
                .AppendLine("User:        " + this._reportInfo.UserName)
                .AppendLine("Date:        " + this._reportInfo.ExceptionDate.ToShortDateString())
                .AppendLine("Time:        " + this._reportInfo.ExceptionDate.ToShortTimeString())
                .AppendLine();

            this._stringBuilder.AppendLine("User Explanation:")
                .AppendLine()
                .AppendFormat("{0} said \"{1}\"", this._reportInfo.UserName, this._reportInfo.UserExplanation)
                .AppendLine()
                .AppendLine("-----------------------------")
                .AppendLine();
        }

        /// <summary>
        ///     The build sys info.
        /// </summary>
        private void BuildSysInfo()
        {
            this._stringBuilder.AppendLine("[System Info]").AppendLine();
            this._stringBuilder.Append(SysInfoResultMapper.CreateStringList(this._sysInfoResults));
            this._stringBuilder.AppendLine("-----------------------------").AppendLine();
        }

        #endregion
    }
}