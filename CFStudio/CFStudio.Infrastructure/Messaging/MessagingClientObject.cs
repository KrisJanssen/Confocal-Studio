#region License

// --------------------------------------------------------------------------- 
// <copyright file="MessagingClientObject.cs" company="Kris Janssen">
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

namespace CFStudio.Infrastructure.Messaging
{
    using System;
    using System.Collections;

    /// <summary>
    ///     The messaging client object.
    /// </summary>
    public class MessagingClientObject : MarshalByRefObject, IObserver
    {
        #region Fields

        /// <summary>
        ///     The _new data.
        /// </summary>
        private readonly ArrayList _newData = new ArrayList();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The get data.
        /// </summary>
        /// <param name="arrData">
        /// The arr data.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public int GetData(out string[] arrData)
        {
            arrData = new string[this._newData.Count];
            this._newData.CopyTo(arrData);
            this._newData.Clear();
            return arrData.Length;
        }

        /// <summary>
        ///     The initialize lifetime service.
        /// </summary>
        /// <returns>
        ///     The <see cref="object" />.
        /// </returns>
        public override object InitializeLifetimeService()
        {
            return null;
        }

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="data">
        /// The data.
        /// </param>
        /// <param name="objState">
        /// The obj state.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool Update(ISubject sender, string data, short objState)
        {
            this._newData.Add(data);
            return true;
        }

        #endregion
    }
}