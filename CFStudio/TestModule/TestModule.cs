// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestModule.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the TestModule type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TestModule
{
    using Microsoft.Practices.Prism.Modularity;
    using Microsoft.Practices.Prism.Regions;

    /// <summary>
    /// The test module.
    /// </summary>
    public class TestModule : IModule
    {
        private readonly IRegionManager regionManager;

        public TestModule(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }

        #region Public Methods and Operators

        /// <summary>
        /// The initialize.
        /// </summary>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public void Initialize()
        {
            regionManager.RegisterViewWithRegion("MainRegion", typeof(Views.TestView));
        }

        #endregion
    }
}