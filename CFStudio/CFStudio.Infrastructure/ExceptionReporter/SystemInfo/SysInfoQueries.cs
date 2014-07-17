#region License

// --------------------------------------------------------------------------- 
// <copyright file="SysInfoQueries.cs" company="Kris Janssen">
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
    ///     The sys info queries.
    /// </summary>
    internal static class SysInfoQueries
    {
        #region Static Fields

        /// <summary>
        ///     The machine.
        /// </summary>
        public static readonly SysInfoQuery Machine = new SysInfoQuery("Machine", "Win32_ComputerSystem", true);

        /// <summary>
        ///     The operating system.
        /// </summary>
        public static readonly SysInfoQuery OperatingSystem = new SysInfoQuery(
            "Operating System", 
            "Win32_OperatingSystem", 
            false);

        #endregion
    }
}