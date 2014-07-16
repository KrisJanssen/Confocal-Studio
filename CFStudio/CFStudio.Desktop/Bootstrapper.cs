// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Bootstrapper.cs" company="">
//   
// </copyright>
// <summary>
//   The bootstrapper.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace CFStudio.Desktop
{
    using System.Windows;

    using Microsoft.Practices.Prism.Modularity;
    using Microsoft.Practices.Prism.UnityExtensions;

    /// <summary>
    /// The bootstrapper.
    /// </summary>
    internal class Bootstrapper : UnityBootstrapper
    {
        #region Methods

        /// <summary>
        /// The configure module catalog.
        /// </summary>
        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();

            ModuleCatalog moduleCatalog = (ModuleCatalog)this.ModuleCatalog;
            moduleCatalog.AddModule(typeof(TestModule.TestModule));
        }

        /// <summary>
        /// The create shell.
        /// </summary>
        /// <returns>
        /// The <see cref="DependencyObject"/>.
        /// </returns>
        protected override DependencyObject CreateShell()
        {
            return new Shell();
        }

        /// <summary>
        /// The initialize shell.
        /// </summary>
        protected override void InitializeShell()
        {
            base.InitializeShell();

            Application.Current.MainWindow = (Window)this.Shell;
            Application.Current.MainWindow.Show();
        }

        #endregion
    }
}