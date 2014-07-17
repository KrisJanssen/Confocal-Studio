#region License

// --------------------------------------------------------------------------- 
// <copyright file="Messages.cs" company="Kris Janssen">
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
    /// <summary>
    ///     The messages.
    /// </summary>
    public static class Messages
    {
        #region Constants

        /// <summary>
        ///     The ticket refresh message.
        /// </summary>
        public const string TicketRefreshMessage = "TICKET_REFRESH";

        /// <summary>
        ///     The widget refresh message.
        /// </summary>
        public const string WidgetRefreshMessage = "WIDGET_REFRESH";

        #endregion
    }
}