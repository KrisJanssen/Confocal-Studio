#region License

// --------------------------------------------------------------------------- 
// <copyright file="CategoryCommand.cs" company="Kris Janssen">
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

    /// <summary>
    /// The category command.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public class CategoryCommand<T> : CaptionCommand<T>, ICategoryCommand
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryCommand{T}"/> class.
        /// </summary>
        /// <param name="caption">
        /// The caption.
        /// </param>
        /// <param name="category">
        /// The category.
        /// </param>
        /// <param name="executeMethod">
        /// The execute method.
        /// </param>
        public CategoryCommand(string caption, string category, Action<T> executeMethod)
            : base(caption, executeMethod)
        {
            this.Caption = caption;
            this.Category = category;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryCommand{T}"/> class.
        /// </summary>
        /// <param name="caption">
        /// The caption.
        /// </param>
        /// <param name="category">
        /// The category.
        /// </param>
        /// <param name="executeMethod">
        /// The execute method.
        /// </param>
        /// <param name="canExecuteMethod">
        /// The can execute method.
        /// </param>
        public CategoryCommand(string caption, string category, Action<T> executeMethod, Func<T, bool> canExecuteMethod)
            : this(caption, category, string.Empty, executeMethod, canExecuteMethod)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryCommand{T}"/> class.
        /// </summary>
        /// <param name="caption">
        /// The caption.
        /// </param>
        /// <param name="category">
        /// The category.
        /// </param>
        /// <param name="imageSource">
        /// The image source.
        /// </param>
        /// <param name="executeMethod">
        /// The execute method.
        /// </param>
        public CategoryCommand(string caption, string category, string imageSource, Action<T> executeMethod)
            : base(caption, executeMethod)
        {
            this.Category = category;
            this.ImageSource = imageSource;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryCommand{T}"/> class.
        /// </summary>
        /// <param name="caption">
        /// The caption.
        /// </param>
        /// <param name="category">
        /// The category.
        /// </param>
        /// <param name="imageSource">
        /// The image source.
        /// </param>
        /// <param name="executeMethod">
        /// The execute method.
        /// </param>
        /// <param name="canExecuteMethod">
        /// The can execute method.
        /// </param>
        public CategoryCommand(
            string caption, 
            string category, 
            string imageSource, 
            Action<T> executeMethod, 
            Func<T, bool> canExecuteMethod)
            : base(caption, executeMethod, canExecuteMethod)
        {
            this.Category = category;
            this.ImageSource = imageSource;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the category.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        ///     Gets or sets the image source.
        /// </summary>
        public string ImageSource { get; set; }

        /// <summary>
        ///     Gets or sets the order.
        /// </summary>
        public int Order { get; set; }

        #endregion
    }
}