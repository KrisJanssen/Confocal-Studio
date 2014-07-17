#region License

// --------------------------------------------------------------------------- 
// <copyright file="MessagingServerObject.cs" company="Kris Janssen">
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
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    ///     The messaging server object.
    /// </summary>
    public class MessagingServerObject : MarshalByRefObject, ISubject
    {
        #region Fields

        /// <summary>
        ///     The _clients.
        /// </summary>
        private readonly IList<IObserver> _clients = new List<IObserver>();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The attach.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        public void Attach(IObserver client)
        {
            Debug.WriteLine("observer bağlandı.");
            this._clients.Add(client);
        }

        /// <summary>
        /// The detach.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        public void Detach(IObserver client)
        {
            this._clients.Remove(client);
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
        /// The notify.
        /// </summary>
        /// <param name="clientData">
        /// The client data.
        /// </param>
        /// <param name="objState">
        /// The obj state.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool Notify(string clientData, short objState)
        {
            for (int i = this._clients.Count - 1; i >= 0; i--)
            {
                try
                {
                    this._clients[i].Update(this, clientData, objState);
                }
                catch (Exception)
                {
                    this._clients.RemoveAt(i);
                }
            }

            return true;
        }

        /// <summary>
        ///     The ping.
        /// </summary>
        public void Ping()
        {
        }

        /// <summary>
        /// The set value.
        /// </summary>
        /// <param name="clientData">
        /// The client data.
        /// </param>
        public void SetValue(string clientData)
        {
            this.Notify(clientData, 0);
        }

        #endregion
    }
}