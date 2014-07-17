#region License

// --------------------------------------------------------------------------- 
// <copyright file="MessagingClient.cs" company="Kris Janssen">
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
    using System.Linq;
    using System.Net.Sockets;
    using System.Runtime.Remoting.Channels;
    using System.Runtime.Remoting.Channels.Tcp;
    using System.Runtime.Serialization.Formatters;
    using System.Threading;

    using CFStudio.Infrastructure.Settings;

    /// <summary>
    ///     The messaging client.
    /// </summary>
    public static class MessagingClient
    {
        #region Static Fields

        /// <summary>
        ///     The timer.
        /// </summary>
        private static readonly Timer Timer = new Timer(OnTimerTick, null, Timeout.Infinite, 1000);

        /// <summary>
        ///     The _channel.
        /// </summary>
        private static TcpChannel _channel;

        /// <summary>
        ///     The _client object.
        /// </summary>
        private static MessagingClientObject _clientObject;

        /// <summary>
        ///     The _message listener.
        /// </summary>
        private static IMessageListener _messageListener;

        /// <summary>
        ///     The _server object.
        /// </summary>
        private static MessagingServerObject _serverObject;

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets a value indicating whether is connected.
        /// </summary>
        public static bool IsConnected { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The can ping.
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool CanPing()
        {
            try
            {
                if (_serverObject != null)
                {
                    _serverObject.Ping();
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                if (IsConnected)
                {
                    Disconnect();
                }

                return false;
            }
        }

        /// <summary>
        /// The connect.
        /// </summary>
        /// <param name="messageListener">
        /// The message listener.
        /// </param>
        public static void Connect(IMessageListener messageListener)
        {
            Timer.Change(0, Timeout.Infinite);
            if (messageListener == null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(LocalSettings.MessagingServerName))
            {
                return;
            }

            _messageListener = messageListener;
            var serverProv = new BinaryServerFormatterSinkProvider { TypeFilterLevel = TypeFilterLevel.Full };

            var clientProv = new BinaryClientFormatterSinkProvider();

            IDictionary props = new Hashtable();
            props["port"] = 0;

            _channel = new TcpChannel(props, clientProv, serverProv);

            ChannelServices.RegisterChannel(_channel, false);

            string url = string.Format(
                "tcp://{0}:{1}/ChatServer", 
                LocalSettings.MessagingServerName, 
                LocalSettings.MessagingServerPort);

            try
            {
                _serverObject = (MessagingServerObject)Activator.GetObject(typeof(MessagingServerObject), url);
                _clientObject = new MessagingClientObject();
                _serverObject.Attach(_clientObject);
            }
            catch
            {
                HandleError();
                return;
            }

            IsConnected = true;
            Timer.Change(0, 1000);
        }

        /// <summary>
        ///     The disconnect.
        /// </summary>
        public static void Disconnect()
        {
            IsConnected = false;
            _messageListener = null;
            try
            {
                try
                {
                    if (_serverObject != null)
                    {
                        _serverObject.Detach(_clientObject);
                    }
                }
                catch (Exception)
                {
                    _serverObject = null;
                }
            }
            finally
            {
                if (_channel != null)
                {
                    ChannelServices.UnregisterChannel(_channel);
                    _channel = null;
                }
            }
        }

        /// <summary>
        /// The reconnect.
        /// </summary>
        /// <param name="messageListener">
        /// The message listener.
        /// </param>
        public static void Reconnect(IMessageListener messageListener)
        {
            try
            {
                Disconnect();
            }
            catch (SocketException)
            {
            }

            _messageListener = messageListener;
            Connect(_messageListener);
        }

        /// <summary>
        /// The send message.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public static void SendMessage(string message)
        {
            if (_serverObject != null)
            {
                try
                {
                    _serverObject.SetValue(string.Format("{0}", message));
                }
                catch (Exception)
                {
                    if (IsConnected)
                    {
                        Disconnect();
                    }
                }
            }
        }

        /// <summary>
        ///     The stop.
        /// </summary>
        public static void Stop()
        {
            if (!IsConnected)
            {
                return;
            }

            Timer.Dispose();
            Disconnect();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     The handle error.
        /// </summary>
        private static void HandleError()
        {
            _messageListener = null;
            _serverObject = null;
            _clientObject = null;
            if (_channel != null)
            {
                ChannelServices.UnregisterChannel(_channel);
                _channel = null;
            }

            IsConnected = false;
        }

        /// <summary>
        /// The on timer tick.
        /// </summary>
        /// <param name="state">
        /// The state.
        /// </param>
        private static void OnTimerTick(object state)
        {
            if (_clientObject != null && _messageListener != null && IsConnected)
            {
                string[] arrData;
                _clientObject.GetData(out arrData);

                foreach (string t in arrData.Distinct())
                {
                    _messageListener.ProcessMessage(t);
                }
            }
        }

        #endregion
    }
}