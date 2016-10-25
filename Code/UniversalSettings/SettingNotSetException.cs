using System;

namespace UniversalSettings
{
    /// <summary>
    /// Occurs when attempting to Get setting that is not Set.
    /// </summary>
    public class SettingNotSetException : SettingsException
    {
        private const string MessageBody = "Following setting is not set:";

        /// <summary>
        /// Creates instance of this class.
        /// </summary>
        /// <param name="settingName">Name of setting that caused exception.</param>
        public SettingNotSetException( string settingName )
            : base( $"{MessageBody}{settingName}" )
        {
        }
    }
}