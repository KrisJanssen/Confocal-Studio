#region License

// --------------------------------------------------------------------------- 
// <copyright file="ExceptionReportInfo.cs" company="Kris Janssen">
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

    /// <summary>
    ///     The exception report info.
    /// </summary>
    public class ExceptionReportInfo : Disposable
    {
        #region Fields

        /// <summary>
        ///     The _exceptions.
        /// </summary>
        private readonly List<Exception> _exceptions = new List<Exception>();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ExceptionReportInfo" /> class.
        /// </summary>
        public ExceptionReportInfo()
        {
            this.SetDefaultValues();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the app assembly.
        /// </summary>
        public Assembly AppAssembly { get; set; }

        /// <summary>
        ///     Gets or sets the custom message.
        /// </summary>
        public string CustomMessage { get; set; }

        /// <summary>
        ///     Gets or sets the exception date.
        /// </summary>
        public DateTime ExceptionDate { get; set; }

        /// <summary>
        ///     Gets the exceptions.
        /// </summary>
        public IList<Exception> Exceptions
        {
            get
            {
                return this._exceptions.AsReadOnly();
            }
        }

        /// <summary>
        ///     Gets or sets the machine name.
        /// </summary>
        public string MachineName { get; set; }

        /// <summary>
        ///     Gets or sets the main exception.
        /// </summary>
        public Exception MainException
        {
            get
            {
                return this._exceptions.Count > 0 ? this._exceptions[0] : null;
            }

            set
            {
                this._exceptions.Clear();
                this._exceptions.Add(value);
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether take screenshot.
        /// </summary>
        public bool TakeScreenshot { get; set; }

        /// <summary>
        ///     Gets or sets the user explanation.
        /// </summary>
        public string UserExplanation { get; set; }

        /// <summary>
        ///     Gets or sets the user name.
        /// </summary>
        public string UserName { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The set exceptions.
        /// </summary>
        /// <param name="exceptions">
        /// The exceptions.
        /// </param>
        public void SetExceptions(IEnumerable<Exception> exceptions)
        {
            this._exceptions.Clear();
            this._exceptions.AddRange(exceptions);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     The set default values.
        /// </summary>
        private void SetDefaultValues()
        {
            this.TakeScreenshot = false;
        }

        #endregion
    }

    /// <summary>
    ///     The default label messages.
    /// </summary>
    public static class DefaultLabelMessages
    {
        #region Constants

        /// <summary>
        ///     The default contact message top.
        /// </summary>
        public const string DefaultContactMessageTop =
            "The following details can be used to obtain support for this application";

        /// <summary>
        ///     The default explanation label.
        /// </summary>
        public const string DefaultExplanationLabel =
            "Please enter a brief explanation of events leading up to this exception";

        #endregion
    }
}