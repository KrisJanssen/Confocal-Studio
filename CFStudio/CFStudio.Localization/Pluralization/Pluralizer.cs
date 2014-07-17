// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Pluralizer.cs" company="">
//   
// </copyright>
// <summary>
//   The pluralizer.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace CFStudio.Localization.Pluralization
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    using CFStudio.Infrastructure.Settings;
    using CFStudio.Localization.Properties;

    /// <summary>
    /// The pluralizer.
    /// </summary>
    public static class Pluralizer
    {
        #region Static Fields

        /// <summary>
        /// The rules.
        /// </summary>
        private static readonly Dictionary<string, PluralizationRule> Rules =
            new Dictionary<string, PluralizationRule>();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes static members of the <see cref="Pluralizer"/> class.
        /// </summary>
        static Pluralizer()
        {
            Rules.Add("en", new EnglishPluralizationRule());
            Rules.Add("*", new DefaultPluralizationrule());
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The to plural.
        /// </summary>
        /// <param name="singular">
        /// The singular.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string ToPlural(string singular)
        {
            return GetPluralizationRule().Pluralize(singular);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The get pluralization rule.
        /// </summary>
        /// <returns>
        /// The <see cref="PluralizationRule"/>.
        /// </returns>
        private static PluralizationRule GetPluralizationRule()
        {
            if (Rules.ContainsKey(LocalSettings.CurrentLanguage))
            {
                return Rules[LocalSettings.CurrentLanguage];
            }

            return Rules["*"];
        }

        #endregion
    }

    /// <summary>
    /// The pluralization rule.
    /// </summary>
    public abstract class PluralizationRule
    {
        #region Public Methods and Operators

        /// <summary>
        /// The pluralize.
        /// </summary>
        /// <param name="singular">
        /// The singular.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public abstract string Pluralize(string singular);

        #endregion
    }

    /// <summary>
    /// The english pluralization rule.
    /// </summary>
    public class EnglishPluralizationRule : PluralizationRule
    {
        #region Static Fields

        /// <summary>
        /// The pluralizations.
        /// </summary>
        private static readonly IDictionary<string, string> Pluralizations = new Dictionary<string, string>
                                                                                 {
                                                                                     {
                                                                                         "person$", 
                                                                                         "people"
                                                                                     }, 
                                                                                     {
                                                                                         "ox$", 
                                                                                         "oxen"
                                                                                     }, 
                                                                                     {
                                                                                         "child$", 
                                                                                         "children"
                                                                                     }, 
                                                                                     {
                                                                                         "foot$", 
                                                                                         "feet"
                                                                                     }, 
                                                                                     {
                                                                                         "tooth$", 
                                                                                         "teeth"
                                                                                     }, 
                                                                                     {
                                                                                         "goose$", 
                                                                                         "geese"
                                                                                     }, 
                                                                                     {
                                                                                         "(.*)fe?", 
                                                                                         "$1ves"
                                                                                     }, 
                                                                                     {
                                                                                         "(.*)man$", 
                                                                                         "$1men"
                                                                                     }, 
                                                                                     {
                                                                                         "(.+[aeiou]y)$", 
                                                                                         "$1s"
                                                                                     }, 
                                                                                     {
                                                                                         "(.+[^aeiou])y$", 
                                                                                         "$1ies"
                                                                                     }, 
                                                                                     {
                                                                                         "(.+z)$", 
                                                                                         "$1zes"
                                                                                     }, 
                                                                                     {
                                                                                         "([m|l])ouse$", 
                                                                                         "$1ice"
                                                                                     }, 
                                                                                     {
                                                                                         "(.+)(e|i)x$", 
                                                                                         @"$1ices"
                                                                                     }, 
                                                                                     {
                                                                                         "(octop|vir)us$", 
                                                                                         "$1i"
                                                                                     }, 
                                                                                     {
                                                                                         "(.+(s|x|sh|ch))$", 
                                                                                         @"$1es"
                                                                                     }, 
                                                                                     {
                                                                                         "(.+)", 
                                                                                         @"$1s"
                                                                                     }
                                                                                 };

        /// <summary>
        /// The unpluralizables.
        /// </summary>
        private static readonly IList<string> Unpluralizables = new List<string>
                                                                    {
                                                                        "equipment", 
                                                                        "information", 
                                                                        "rice", 
                                                                        "money", 
                                                                        "species", 
                                                                        "series", 
                                                                        "fish", 
                                                                        "sheep", 
                                                                        "deer"
                                                                    };

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The pluralize.
        /// </summary>
        /// <param name="singular">
        /// The singular.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public override string Pluralize(string singular)
        {
            if (Unpluralizables.Contains(singular))
            {
                return singular;
            }

            var plural = string.Empty;
            foreach (
                var pluralization in Pluralizations.Where(pluralization => Regex.IsMatch(singular, pluralization.Key)))
            {
                plural = Regex.Replace(singular, pluralization.Key, pluralization.Value);
                break;
            }

            return plural;
        }

        #endregion
    }

    /// <summary>
    /// The default pluralizationrule.
    /// </summary>
    public class DefaultPluralizationrule : PluralizationRule
    {
        #region Public Methods and Operators

        /// <summary>
        /// The pluralize.
        /// </summary>
        /// <param name="singular">
        /// The singular.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public override string Pluralize(string singular)
        {
            return string.Format(Resources.List_f, singular);
        }

        #endregion
    }
}