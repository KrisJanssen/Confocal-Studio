#region License

// --------------------------------------------------------------------------- 
// <copyright file="SysInfoResultMapper.cs" company="Kris Janssen">
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
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    ///     Map SysInfoResults to human-readable formats
    /// </summary>
    public static class SysInfoResultMapper
    {
        #region Public Methods and Operators

        /// <summary>
        /// create a string representation of a list of SysInfoResults, customised specifically (eg 2-level deep)
        /// </summary>
        /// <param name="results">
        /// The results.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string CreateStringList(IEnumerable<SysInfoResult> results)
        {
            var stringBuilder = new StringBuilder();

            foreach (SysInfoResult result in results)
            {
                stringBuilder.AppendLine(result.Name);

                foreach (string nodeValueParent in result.Nodes)
                {
                    stringBuilder.AppendLine("-" + nodeValueParent);

                    foreach (SysInfoResult childResult in result.ChildResults)
                    {
                        foreach (string nodeValue in childResult.Nodes)
                        {
                            stringBuilder.AppendLine("--" + nodeValue);
                        }
                    }
                }

                stringBuilder.AppendLine();
            }

            return stringBuilder.ToString();
        }

        #endregion
    }
}