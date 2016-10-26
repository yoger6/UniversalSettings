using System;
using System.Collections.Generic;

namespace UniversalSettings
{
    /// <summary>
    /// Container class for application settings.
    /// </summary>
    public class Settings
    {
        private readonly Dictionary<string, object> _settings;

        /// <summary>
        /// Creates instance of empty settings container.
        /// </summary>
        public Settings()
        {
            _settings = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets setting.
        /// </summary>
        /// <typeparam name="T">Requested type.</typeparam>
        /// <param name="settingName">Setting name.</param>
        /// <returns>The Setting</returns>
        public T Get<T>( string settingName )
        {
            ValidateName( settingName );

            if(!_settings.ContainsKey( settingName ))
            {
                throw new SettingNotSetException( settingName );
            }

            var value = _settings[settingName];

            try
            {
                return value == null ? default(T) : (T) value;
            }
            catch ( InvalidCastException e )
            {
                throw new InvalidSettingTypeRequestedException( typeof(T), value.GetType(), e);
            }
        }

        /// <summary>
        /// Creates or updates setting.
        /// </summary>
        /// <typeparam name="T">Value type.</typeparam>
        /// <param name="settingName">Name of the setting.</param>
        /// <param name="value">The value.</param>
        public void Set<T>( string settingName, T value )
        {
            if ( value == null )
            {
                throw new ArgumentNullException( nameof( value ) );
            }

            ValidateName( settingName );

            if ( _settings.ContainsKey( settingName ) )
            {
                _settings[settingName] = value;
            }
            else
            {
                _settings.Add( settingName, value );
            }
        }

        /// <summary>
        /// Determines if setting is set.
        /// </summary>
        /// <param name="settingName">Name of the setting.</param>
        /// <returns>true if setting is set; otherwise false</returns>
        public bool IsSet( string settingName )
        {
            ValidateName( settingName );

            return _settings.ContainsKey( settingName );
        }

        private void ValidateName( string settingName )
        {
            if ( settingName == null )
                throw new ArgumentNullException( nameof( settingName ) );
            if ( string.IsNullOrWhiteSpace( settingName ) )
                throw new ArgumentException(
                    "Setting name must contain at least one character and cannot be a whitespace." );
        }

        /// <summary>
        /// Provides read only, raw access to the settings.
        /// </summary>
        /// <returns></returns>
        public IReadOnlyDictionary<string, object> GetAll()
        {
            return _settings;
        }
    }
}