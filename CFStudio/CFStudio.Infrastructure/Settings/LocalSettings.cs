#region License

// --------------------------------------------------------------------------- 
// <copyright file="LocalSettings.cs" company="Kris Janssen">
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

namespace CFStudio.Infrastructure.Settings
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Xml;
    using System.Xml.Serialization;

    using CFStudio.Infrastructure.Helpers;

    using Microsoft.Win32;

    /// <summary>
    ///     The settings object.
    /// </summary>
    public class SettingsObject
    {
        #region Fields

        /// <summary>
        ///     The _custom settings.
        /// </summary>
        private readonly SerializableDictionary<string, string> _customSettings;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SettingsObject" /> class.
        /// </summary>
        public SettingsObject()
        {
            this._customSettings = new SerializableDictionary<string, string>();
            this.MessagingServerPort = 8080;
            this.ConnectionString = string.Empty;
            this.DefaultHtmlReportHeader = @"
<style type='text/css'> 
html
{
  font-family: 'Courier New', monospace;
} 
</style>";
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets a value indicating whether allow multiple clients.
        /// </summary>
        public bool AllowMultipleClients { get; set; }

        /// <summary>
        ///     Gets or sets the api host.
        /// </summary>
        public string ApiHost { get; set; }

        /// <summary>
        ///     Gets or sets the api port.
        /// </summary>
        public string ApiPort { get; set; }

        /// <summary>
        ///     Gets or sets the caller id device name.
        /// </summary>
        public string CallerIdDeviceName { get; set; }

        /// <summary>
        ///     Gets or sets the connection string.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        ///     Gets or sets the current language.
        /// </summary>
        public string CurrentLanguage { get; set; }

        /// <summary>
        ///     Gets the custom settings.
        /// </summary>
        public SerializableDictionary<string, string> CustomSettings
        {
            get
            {
                return this._customSettings;
            }
        }

        /// <summary>
        ///     Gets or sets the default html report header.
        /// </summary>
        public string DefaultHtmlReportHeader { get; set; }

        /// <summary>
        ///     Gets or sets the default record limit.
        /// </summary>
        public int DefaultRecordLimit { get; set; }

        /// <summary>
        ///     Gets or sets the logo path.
        /// </summary>
        public string LogoPath { get; set; }

        /// <summary>
        ///     Gets or sets the messaging server name.
        /// </summary>
        public string MessagingServerName { get; set; }

        /// <summary>
        ///     Gets or sets the messaging server port.
        /// </summary>
        public int MessagingServerPort { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether override language.
        /// </summary>
        public bool OverrideLanguage { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether override windows regional settings.
        /// </summary>
        public bool OverrideWindowsRegionalSettings { get; set; }

        /// <summary>
        ///     Gets or sets the print font family.
        /// </summary>
        public string PrintFontFamily { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether start messaging client.
        /// </summary>
        public bool StartMessagingClient { get; set; }

        /// <summary>
        ///     Gets or sets the terminal name.
        /// </summary>
        public string TerminalName { get; set; }

        /// <summary>
        ///     Gets or sets the token life time.
        /// </summary>
        public TimeSpan TokenLifeTime { get; set; }

        /// <summary>
        ///     Gets or sets the window scale.
        /// </summary>
        public double WindowScale { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The get custom value.
        /// </summary>
        /// <param name="settingName">
        /// The setting name.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public string GetCustomValue(string settingName)
        {
            return this.CustomSettings.ContainsKey(settingName) ? this.CustomSettings[settingName] : string.Empty;
        }

        /// <summary>
        /// The set custom value.
        /// </summary>
        /// <param name="settingName">
        /// The setting name.
        /// </param>
        /// <param name="settingValue">
        /// The setting value.
        /// </param>
        public void SetCustomValue(string settingName, string settingValue)
        {
            if (!this.CustomSettings.ContainsKey(settingName))
            {
                this.CustomSettings.Add(settingName, settingValue);
            }
            else
            {
                this.CustomSettings[settingName] = settingValue;
            }

            if (string.IsNullOrEmpty(settingValue))
            {
                this.CustomSettings.Remove(settingName);
            }
        }

        #endregion
    }

    /// <summary>
    ///     The local settings.
    /// </summary>
    public static class LocalSettings
    {
        #region Static Fields

        /// <summary>
        ///     The version data file path.
        /// </summary>
        private static readonly string VersionDataFilePath = DataPath + @"\version.dat";

        /// <summary>
        ///     The _culture info.
        /// </summary>
        private static CultureInfo _cultureInfo;

        /// <summary>
        ///     The _settings object.
        /// </summary>
        private static SettingsObject _settingsObject;

        /// <summary>
        ///     The _supported languages.
        /// </summary>
        private static IList<string> _supportedLanguages;

        /// <summary>
        ///     The _version data.
        /// </summary>
        private static Dictionary<string, string> _versionData;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes static members of the <see cref="LocalSettings" /> class.
        /// </summary>
        static LocalSettings()
        {
            if (!Directory.Exists(DocumentPath))
            {
                Directory.CreateDirectory(DocumentPath);
            }

            if (!Directory.Exists(DataPath))
            {
                Directory.CreateDirectory(DataPath);
            }

            if (!Directory.Exists(UserPath))
            {
                Directory.CreateDirectory(UserPath);
            }

            LoadSettings();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets a value indicating whether allow multiple clients.
        /// </summary>
        public static bool AllowMultipleClients
        {
            get
            {
                return _settingsObject.AllowMultipleClients;
            }

            set
            {
                _settingsObject.AllowMultipleClients = value;
            }
        }

        /// <summary>
        ///     Gets or sets the api host.
        /// </summary>
        public static string ApiHost
        {
            get
            {
                return _settingsObject.ApiHost;
            }

            set
            {
                _settingsObject.ApiHost = value;
                SaveSettings();
            }
        }

        /// <summary>
        ///     Gets or sets the api port.
        /// </summary>
        public static string ApiPort
        {
            get
            {
                return _settingsObject.ApiPort;
            }

            set
            {
                _settingsObject.ApiPort = value;
                SaveSettings();
            }
        }

        /// <summary>
        ///     Gets the app name.
        /// </summary>
        public static string AppName
        {
            get
            {
                return "SambaPOS3";
            }
        }

        /// <summary>
        ///     Gets or sets the app path.
        /// </summary>
        public static string AppPath { get; set; }

        /// <summary>
        ///     Gets the app version.
        /// </summary>
        public static string AppVersion
        {
            get
            {
                return CanReadVersionFromFile() ? GetVersionDat("AppVersion") : DefaultAppVersion;
            }
        }

        /// <summary>
        ///     Gets the app version date time.
        /// </summary>
        public static DateTime AppVersionDateTime
        {
            get
            {
                if (!CanReadVersionFromFile())
                {
                    return DateTime.Now;
                }

                var reg = new Regex(@"(\d\d\d\d)-(\d\d)-(\d\d) (\d\d)(\d\d)");
                Match match = reg.Match(GetVersionDat("VersionTime"));

                return new DateTime(
                    Convert.ToInt32(match.Groups[1].Value), 
                    Convert.ToInt32(match.Groups[2].Value), 
                    Convert.ToInt32(match.Groups[3].Value), 
                    Convert.ToInt32(match.Groups[4].Value), 
                    Convert.ToInt32(match.Groups[5].Value), 
                    0);
            }
        }

        /// <summary>
        ///     Gets or sets the caller id device name.
        /// </summary>
        public static string CallerIdDeviceName
        {
            get
            {
                return _settingsObject.CallerIdDeviceName;
            }

            set
            {
                _settingsObject.CallerIdDeviceName = value;
            }
        }

        /// <summary>
        ///     Gets the common settings file name.
        /// </summary>
        public static string CommonSettingsFileName
        {
            get
            {
                return DataPath + "\\SambaSettings.txt";
            }
        }

        /// <summary>
        ///     Gets or sets the connection string.
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                return _settingsObject.ConnectionString;
            }

            set
            {
                _settingsObject.ConnectionString = value;
            }
        }

        /// <summary>
        ///     Gets or sets the currency format.
        /// </summary>
        public static string CurrencyFormat { get; set; }

        /// <summary>
        ///     Gets the currency symbol.
        /// </summary>
        public static string CurrencySymbol
        {
            get
            {
                return CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol;
            }
        }

        /// <summary>
        ///     Gets or sets the current db version.
        /// </summary>
        public static long CurrentDbVersion { get; set; }

        /// <summary>
        ///     Gets or sets the current language.
        /// </summary>
        public static string CurrentLanguage
        {
            get
            {
                return _settingsObject.CurrentLanguage;
            }

            set
            {
                _settingsObject.CurrentLanguage = value;
                _cultureInfo = CultureInfo.GetCultureInfo(value);
                UpdateThreadLanguage();
                SaveSettings();
            }
        }

        /// <summary>
        ///     Gets the data path.
        /// </summary>
        public static string DataPath
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Ozgu Tech\\"
                       + AppName;
            }
        }

        /// <summary>
        ///     Gets the database label.
        /// </summary>
        public static string DatabaseLabel
        {
            get
            {
                if (ConnectionString.ToLower().Contains(".sdf"))
                {
                    return "CE";
                }

                if (ConnectionString.ToLower().Contains("data source"))
                {
                    return "SQ";
                }

                if (ConnectionString.ToLower().StartsWith("mongodb://"))
                {
                    return "MG";
                }

                if (string.IsNullOrEmpty(ConnectionString) && IsSqlce40Installed())
                {
                    return "CE";
                }

                return "TX";
            }
        }

        /// <summary>
        ///     Gets the db version.
        /// </summary>
        public static int DbVersion
        {
            get
            {
                return CanReadVersionFromFile() ? Convert.ToInt32(GetVersionDat("DbVersion")) : DefaultDbVersion;
            }
        }

        /// <summary>
        ///     Gets the decimals.
        /// </summary>
        public static int Decimals
        {
            get
            {
                return 2;
            }
        }

        /// <summary>
        ///     Gets or sets the default html report header.
        /// </summary>
        public static string DefaultHtmlReportHeader
        {
            get
            {
                return _settingsObject.DefaultHtmlReportHeader;
            }

            set
            {
                _settingsObject.DefaultHtmlReportHeader = value;
            }
        }

        /// <summary>
        ///     Gets or sets the default record limit.
        /// </summary>
        public static int DefaultRecordLimit
        {
            get
            {
                return _settingsObject.DefaultRecordLimit;
            }

            set
            {
                _settingsObject.DefaultRecordLimit = value;
            }
        }

        /// <summary>
        ///     Gets the document path.
        /// </summary>
        public static string DocumentPath
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + AppName;
            }
        }

        /// <summary>
        ///     Gets or sets the logo path.
        /// </summary>
        public static string LogoPath
        {
            get
            {
                return _settingsObject.LogoPath;
            }

            set
            {
                _settingsObject.LogoPath = value;
            }
        }

        /// <summary>
        ///     Gets or sets the messaging server name.
        /// </summary>
        public static string MessagingServerName
        {
            get
            {
                return _settingsObject.MessagingServerName;
            }

            set
            {
                _settingsObject.MessagingServerName = value;
            }
        }

        /// <summary>
        ///     Gets or sets the messaging server port.
        /// </summary>
        public static int MessagingServerPort
        {
            get
            {
                return _settingsObject.MessagingServerPort;
            }

            set
            {
                _settingsObject.MessagingServerPort = value;
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether override windows regional settings.
        /// </summary>
        public static bool OverrideWindowsRegionalSettings
        {
            get
            {
                return _settingsObject.OverrideWindowsRegionalSettings;
            }

            set
            {
                _settingsObject.OverrideWindowsRegionalSettings = value;
            }
        }

        /// <summary>
        ///     Gets or sets the print font family.
        /// </summary>
        public static string PrintFontFamily
        {
            get
            {
                if (string.IsNullOrEmpty(_settingsObject.PrintFontFamily)
                    || _settingsObject.PrintFontFamily == string.Empty)
                {
                    _settingsObject.PrintFontFamily = "Courier New";
                    SaveSettings();

                    // return "Consolas";
                }

                return _settingsObject.PrintFontFamily;
            }

            set
            {
                _settingsObject.PrintFontFamily = value;
                SaveSettings();
            }
        }

        /// <summary>
        ///     Gets or sets the printout currency format.
        /// </summary>
        public static string PrintoutCurrencyFormat { get; set; }

        /// <summary>
        ///     Gets or sets the quantity format.
        /// </summary>
        public static string QuantityFormat { get; set; }

        /// <summary>
        ///     Gets or sets the report currency format.
        /// </summary>
        public static string ReportCurrencyFormat { get; set; }

        /// <summary>
        ///     Gets or sets the report quantity format.
        /// </summary>
        public static string ReportQuantityFormat { get; set; }

        /// <summary>
        ///     Gets the settings file name.
        /// </summary>
        public static string SettingsFileName
        {
            get
            {
                return File.Exists(UserSettingsFileName) ? UserSettingsFileName : CommonSettingsFileName;
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether start messaging client.
        /// </summary>
        public static bool StartMessagingClient
        {
            get
            {
                return _settingsObject.StartMessagingClient;
            }

            set
            {
                _settingsObject.StartMessagingClient = value;
            }
        }

        /// <summary>
        ///     Gets the supported languages.
        /// </summary>
        public static IList<string> SupportedLanguages
        {
            get
            {
                return _supportedLanguages
                       ?? (_supportedLanguages =
                           new[]
                               {
                                   "en", "tr", "it", "pt-BR", "hr", "ar", "hu", "es", "id", "el", "zh-CN", "de", "sq", 
                                   "cs", "nl", "he", "fr", "ru-RU", "da", "fa", "tk-TM"
                               });
            }
        }

        /// <summary>
        ///     Gets or sets the terminal name.
        /// </summary>
        public static string TerminalName
        {
            get
            {
                return _settingsObject.TerminalName;
            }

            set
            {
                _settingsObject.TerminalName = value;
            }
        }

        /// <summary>
        ///     Gets or sets the token life time.
        /// </summary>
        public static TimeSpan TokenLifeTime
        {
            get
            {
                return _settingsObject.TokenLifeTime;
            }

            set
            {
                _settingsObject.TokenLifeTime = value;
                SaveSettings();
            }
        }

        /// <summary>
        ///     Gets the user path.
        /// </summary>
        public static string UserPath
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ozgu Tech\\" + AppName;
            }
        }

        /// <summary>
        ///     Gets the user settings file name.
        /// </summary>
        public static string UserSettingsFileName
        {
            get
            {
                return UserPath + "\\SambaSettings.txt";
            }
        }

        /// <summary>
        ///     Gets or sets the window scale.
        /// </summary>
        public static double WindowScale
        {
            get
            {
                return _settingsObject.WindowScale;
            }

            set
            {
                _settingsObject.WindowScale = value;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the default app version.
        /// </summary>
        private static string DefaultAppVersion
        {
            get
            {
                return "3.0.35 BETA";
            }
        }

        /// <summary>
        ///     Gets the default db version.
        /// </summary>
        private static int DefaultDbVersion
        {
            get
            {
                return 24;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The is sqlce 40 installed.
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool IsSqlce40Installed()
        {
            RegistryKey rk =
                Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Microsoft SQL Server Compact Edition\\v4.0");
            return rk != null;
        }

        /// <summary>
        ///     The load settings.
        /// </summary>
        public static void LoadSettings()
        {
            _settingsObject = new SettingsObject();
            string fileName = SettingsFileName;
            if (File.Exists(fileName))
            {
                var serializer = new XmlSerializer(_settingsObject.GetType());
                var reader = new XmlTextReader(fileName);
                try
                {
                    _settingsObject = serializer.Deserialize(reader) as SettingsObject;
                }
                finally
                {
                    reader.Close();
                }
            }

            if (DefaultRecordLimit == 0)
            {
                DefaultRecordLimit = 100;
            }
        }

        /// <summary>
        /// The read setting.
        /// </summary>
        /// <param name="settingName">
        /// The setting name.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string ReadSetting(string settingName)
        {
            return _settingsObject.GetCustomValue(settingName);
        }

        /// <summary>
        ///     The save settings.
        /// </summary>
        public static void SaveSettings()
        {
            try
            {
                var serializer = new XmlSerializer(_settingsObject.GetType());
                var writer = new XmlTextWriter(SettingsFileName, null);
                try
                {
                    serializer.Serialize(writer, _settingsObject);
                }
                finally
                {
                    writer.Close();
                }
            }
            catch (UnauthorizedAccessException)
            {
                if (!File.Exists(UserSettingsFileName))
                {
                    File.Create(UserSettingsFileName).Close();
                    SaveSettings();
                }
            }
            catch (IOException)
            {
            }
        }

        /// <summary>
        /// The update setting.
        /// </summary>
        /// <param name="settingName">
        /// The setting name.
        /// </param>
        /// <param name="settingValue">
        /// The setting value.
        /// </param>
        public static void UpdateSetting(string settingName, string settingValue)
        {
            _settingsObject.SetCustomValue(settingName, settingValue);
            SaveSettings();
        }

        /// <summary>
        ///     The update thread language.
        /// </summary>
        public static void UpdateThreadLanguage()
        {
            if (_cultureInfo != null)
            {
                if (OverrideWindowsRegionalSettings)
                {
                    Thread.CurrentThread.CurrentCulture = _cultureInfo;
                }

                Thread.CurrentThread.CurrentUICulture = _cultureInfo;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     The can read version from file.
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        private static bool CanReadVersionFromFile()
        {
#if DEBUG
            return false;
#else
            return File.Exists(VersionDataFilePath);
#endif
        }

        /// <summary>
        /// The get version dat.
        /// </summary>
        /// <param name="versionType">
        /// The version type.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// </exception>
        private static string GetVersionDat(string versionType)
        {
            if (_versionData == null || _versionData.Count == 0)
            {
                _versionData = new Dictionary<string, string>();
                foreach (string item in File.ReadAllLines(VersionDataFilePath))
                {
                    string[] split = item.Split('=');
                    _versionData.Add(split[0], split[1]);
                }
            }

            if (_versionData.ContainsKey(versionType))
            {
                return _versionData[versionType];
            }

            throw new ArgumentOutOfRangeException("versionType", "VersionType " + versionType + " doesn't exist!");
        }

        #endregion
    }
}