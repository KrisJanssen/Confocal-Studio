// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LocalizedDisplayNameAttribute.cs" company="">
//   
// </copyright>
// <summary>
//   The localized display name attribute.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace CFStudio.Localization
{
    using System;
    using System.ComponentModel;
    using System.Linq;

    using CFStudio.Localization.Properties;

    /// <summary>
    /// The localized display name attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class LocalizedDisplayNameAttribute : DisplayNameAttribute
    {
        #region Fields

        /// <summary>
        /// The _resource name.
        /// </summary>
        private readonly string _resourceName;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizedDisplayNameAttribute"/> class.
        /// </summary>
        /// <param name="resourceName">
        /// The resource name.
        /// </param>
        public LocalizedDisplayNameAttribute(string resourceName)
        {
            this._resourceName = resourceName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizedDisplayNameAttribute"/> class.
        /// </summary>
        public LocalizedDisplayNameAttribute()
        {
            var type = this.GetType();
            this._resourceName = type.Name;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the display name.
        /// </summary>
        public override string DisplayName
        {
            get
            {
                var result = Resources.ResourceManager.GetString(this._resourceName);
                if (string.IsNullOrEmpty(result))
                {
                    result =
                        this._resourceName.Select(x => char.IsLower(x) ? x.ToString() : string.Format(" {0}", x))
                            .Aggregate((x, y) => x + y)
                            .Trim();
                }

                return result;
            }
        }

        #endregion
    }
}