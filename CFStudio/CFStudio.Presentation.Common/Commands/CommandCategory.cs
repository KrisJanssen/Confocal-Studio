#region License

// --------------------------------------------------------------------------- 
// <copyright file="CommandCategory.cs" company="Kris Janssen">
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

namespace CFStudio.Presentation.Common.Commands
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    ///     The dashboard command category.
    /// </summary>
    public class DashboardCommandCategory
    {
        #region Fields

        /// <summary>
        ///     The _commands.
        /// </summary>
        private readonly List<ICategoryCommand> _commands;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardCommandCategory"/> class.
        /// </summary>
        /// <param name="category">
        /// The category.
        /// </param>
        public DashboardCommandCategory(string category)
        {
            this.Category = category;
            this._commands = new List<ICategoryCommand>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the category.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        ///     Gets the commands.
        /// </summary>
        public IEnumerable<ICategoryCommand> Commands
        {
            get
            {
                return this._commands.OrderBy(x => x.Order);
            }
        }

        /// <summary>
        ///     Gets or sets the order.
        /// </summary>
        public int Order { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The add command.
        /// </summary>
        /// <param name="command">
        /// The command.
        /// </param>
        public void AddCommand(ICategoryCommand command)
        {
            this._commands.Add(command);
        }

        #endregion
    }
}