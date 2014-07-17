#region License

// --------------------------------------------------------------------------- 
// <copyright file="CaptionCommand.cs" company="Kris Janssen">
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
    using System.ComponentModel;
    using System.Windows.Input;

    using CFStudio.Presentation.Common.Annotations;

    using Microsoft.Practices.Prism.Commands;

    /// <summary>
    /// The caption command.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public class CaptionCommand<T> : DelegateCommand<T>, ICaptionCommand, INotifyPropertyChanged
    {
        #region Fields

        /// <summary>
        ///     The _caption.
        /// </summary>
        private string _caption;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CaptionCommand{T}"/> class.
        /// </summary>
        /// <param name="caption">
        /// The caption.
        /// </param>
        /// <param name="executeMethod">
        /// The execute method.
        /// </param>
        public CaptionCommand(string caption, Action<T> executeMethod)
            : base(executeMethod)
        {
            this.Caption = caption;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CaptionCommand{T}"/> class.
        /// </summary>
        /// <param name="caption">
        /// The caption.
        /// </param>
        /// <param name="executeMethod">
        /// The execute method.
        /// </param>
        /// <param name="canExecuteMethod">
        /// The can execute method.
        /// </param>
        public CaptionCommand(string caption, Action<T> executeMethod, Func<T, bool> canExecuteMethod)
            : base(executeMethod, canExecuteMethod)
        {
            this.Caption = caption;
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

        /// <summary>
        ///     The property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the caption.
        /// </summary>
        public string Caption
        {
            get
            {
                return this._caption;
            }

            set
            {
                this._caption = value;
                this.OnPropertyChanged("Caption");
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The on property changed.
        /// </summary>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}