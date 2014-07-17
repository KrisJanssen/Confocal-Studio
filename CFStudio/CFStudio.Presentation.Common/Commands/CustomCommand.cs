#region License

// --------------------------------------------------------------------------- 
// <copyright file="CustomCommand.cs" company="Kris Janssen">
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
    using System;
    using System.Windows.Input;

    using Microsoft.Practices.Prism.Commands;

    /// <summary>
    ///     The custom command.
    /// </summary>
    public class CustomCommand : DelegateCommand<object>, ICaptionCommand
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomCommand"/> class.
        /// </summary>
        /// <param name="caption">
        /// The caption.
        /// </param>
        /// <param name="dataObject">
        /// The data object.
        /// </param>
        /// <param name="executeMethod">
        /// The execute method.
        /// </param>
        public CustomCommand(string caption, object dataObject, Action<object> executeMethod)
            : base(executeMethod)
        {
            this.Caption = caption;
            this.DataObject = dataObject;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomCommand"/> class.
        /// </summary>
        /// <param name="caption">
        /// The caption.
        /// </param>
        /// <param name="executeMethod">
        /// The execute method.
        /// </param>
        /// <param name="dataObject">
        /// The data object.
        /// </param>
        /// <param name="canExecuteMethod">
        /// The can execute method.
        /// </param>
        public CustomCommand(
            string caption, 
            Action<object> executeMethod, 
            object dataObject, 
            Func<object, bool> canExecuteMethod)
            : base(executeMethod, canExecuteMethod)
        {
            this.Caption = caption;
            this.DataObject = dataObject;
        }

        #endregion

        #region Public Events

        /// <summary>
        ///     The can execute changed.
        /// </summary>
        public new event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }

            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the caption.
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        ///     Gets or sets the data object.
        /// </summary>
        public object DataObject { get; set; }

        #endregion
    }
}