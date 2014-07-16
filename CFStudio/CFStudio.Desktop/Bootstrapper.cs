namespace CFStudio.Desktop
{
    using System.Net.Mime;
    using System.Windows;

    using Microsoft.Practices.Prism.Modularity;
    using Microsoft.Practices.Prism.UnityExtensions;
    using Microsoft.Practices.Unity;

    /// <summary>
    /// The bootstrapper.
    /// </summary>
    class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return new Shell();
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();

            Application.Current.MainWindow = (Window)this.Shell;
            Application.Current.MainWindow.Show();
        }

        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();
        }
    }
}
