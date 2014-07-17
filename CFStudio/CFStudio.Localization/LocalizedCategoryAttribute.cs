// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LocalizedCategoryAttribute.cs" company="">
//   
// </copyright>
// <summary>
//   The localized category attribute.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace CFStudio.Localization
{
    using System.ComponentModel;

    using CFStudio.Localization.Properties;

    /// <summary>
    /// The localized category attribute.
    /// </summary>
    public class LocalizedCategoryAttribute : CategoryAttribute
    {
        #region Fields

        /// <summary>
        /// The _resource name.
        /// </summary>
        private readonly string _resourceName;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizedCategoryAttribute"/> class.
        /// </summary>
        /// <param name="resourceName">
        /// The resource name.
        /// </param>
        public LocalizedCategoryAttribute(string resourceName)
            : base()
        {
            this._resourceName = resourceName;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The get localized string.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        protected override string GetLocalizedString(string value)
        {
            return Resources.ResourceManager.GetString(this._resourceName);
        }

        #endregion
    }
}