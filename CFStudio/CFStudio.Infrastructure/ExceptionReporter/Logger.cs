#region License

// --------------------------------------------------------------------------- 
// <copyright file="Logger.cs" company="Kris Janssen">
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
    using System.IO;

    using CFStudio.Infrastructure.Settings;

    /// <summary>
    ///     The logger.
    /// </summary>
    public static class Logger
    {
        #region Static Fields

        /// <summary>
        ///     The lock obj.
        /// </summary>
        private static readonly object LockObj = new Object();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The log.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public static void Log(string message)
        {
            lock (LockObj)
            {
                message += "######################### E N D #########################\r\n";
                File.AppendAllText(LocalSettings.DocumentPath + "\\log.txt", message);
            }
        }

        /// <summary>
        /// The log.
        /// </summary>
        /// <param name="exception">
        /// The exception.
        /// </param>
        public static void Log(params Exception[] exception)
        {
            var ri = new ExceptionReportInfo();
            ri.SetExceptions(exception);
            var rg = new ExceptionReportGenerator(ri);
            Log(rg.CreateExceptionReport());
        }

        #endregion
    }
}