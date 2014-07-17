#region License

// --------------------------------------------------------------------------- 
// <copyright file="App.xaml.cs" company="Kris Janssen">
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

namespace CFStudio.Presentation
{
    using System;
    using System.Windows;

    using CFStudio.Presentation.Common;

    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Methods

        /// <summary>
        /// Let's get started shall we?
        /// </summary>
        /// <param name="e">
        /// The StartupEventArgs.
        /// </param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

#if (DEBUG)
            this.RunInDebugMode();
#else
            RunInReleaseMode();
#endif

            // ServiceLocator.Current.GetAllInstances<IApplicationState>().
        }

        /// <summary>
        /// The app domain unhandled exception.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void AppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            this.HandleException(e.ExceptionObject as Exception);
        }

        /// <summary>
        /// The handle exception.
        /// </summary>
        /// <param name="ex">
        /// The ex.
        /// </param>
        private void HandleException(Exception ex)
        {
            if (ex == null)
            {
                return;
            }

            ErrorReporter.Show(ex);
            Environment.Exit(1);
        }

        /// <summary>
        ///     In debug mode, we do not need special application level exception handling.
        /// </summary>
        private void RunInDebugMode()
        {
            var bootstrapper = new Bootstrapper();
            bootstrapper.Run();
        }

        /// <summary>
        ///     The run in release mode.
        /// </summary>
        private void RunInReleaseMode()
        {
            AppDomain.CurrentDomain.UnhandledException += this.AppDomainUnhandledException;
            try
            {
                var bootstrapper = new Bootstrapper();
                bootstrapper.Run();
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }
        }

        #endregion
    }
}