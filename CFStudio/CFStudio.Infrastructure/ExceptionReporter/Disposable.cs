#region License

// --------------------------------------------------------------------------- 
// <copyright file="Disposable.cs" company="Kris Janssen">
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
    using System.Threading;

    /// <summary>
    ///     Base class for all classes wanting to implement <see cref="IDisposable" />.
    /// </summary>
    /// <remarks>
    ///     Base on an article by Davy Brion
    ///     <see href="http://davybrion.com/blog/2008/06/disposing-of-the-idisposable-implementation/" />.
    /// </remarks>
    public abstract class Disposable : IDisposable
    {
        #region Fields

        /// <summary>
        ///     The disposed.
        /// </summary>
        private int disposed;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Disposable" /> class.
        /// </summary>
        protected Disposable()
        {
            this.disposed = 0;
        }

        /// <summary>
        ///     Finalizes an instance of the <see cref="Disposable" /> class.
        /// </summary>
        ~Disposable()
        {
            this.Dispose(false);
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets a value indicating whether is disposed.
        /// </summary>
        public bool IsDisposed
        {
            get
            {
                return this.disposed == 1;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The dispose.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The dispose.
        /// </summary>
        /// <param name="disposing">
        /// The disposing.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (Interlocked.CompareExchange(ref this.disposed, 1, 0) == 0)
            {
                if (disposing)
                {
                    this.DisposeManagedResources();
                }

                this.DisposeUnmanagedResources();
            }
        }

        /// <summary>
        ///     The dispose managed resources.
        /// </summary>
        protected virtual void DisposeManagedResources()
        {
        }

        /// <summary>
        ///     The dispose unmanaged resources.
        /// </summary>
        protected virtual void DisposeUnmanagedResources()
        {
        }

        #endregion
    }
}