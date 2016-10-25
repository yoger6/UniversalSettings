using System;

namespace UniversalSettings
{
    /// <summary>
    /// Wraps exceptions thrown by settings class.
    /// </summary>
    public class SettingsException : Exception
    {
        /// <summary>
        /// Creates instance of this class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public SettingsException( string message )
            : base( message )
        {
        }

        /// <summary>
        /// Creates instance of this class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="innerException">Inner exception</param>
        public SettingsException( string message, Exception innerException )
            : base( message, innerException )
        {
        }
    }
}