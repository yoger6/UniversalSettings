using System;

namespace UniversalSettings
{
    /// <summary>
    /// Occurs when attempting to Get setting with invalid type parameter.
    /// </summary>
    public class InvalidSettingTypeRequestedException : SettingsException
    {
        private const string MessageBodyFirstPart = "Invalid type requested:";
        private const string MessageBodySecondPart = "Actual type is:";

        public Type RequestedType { get; }
        public Type ActualType { get; }

        /// <summary>
        /// Creates instance of this class.
        /// </summary>
        /// <param name="requestedType">Type requested by caller.</param>
        /// <param name="actualType">Stored setting type.</param>
        /// <param name="innerException">Inner exception.</param>
        public InvalidSettingTypeRequestedException( Type requestedType, Type actualType, Exception innerException ) 
            : base( $"{MessageBodyFirstPart} {requestedType}. {MessageBodySecondPart} {actualType}.", 
                innerException )
        {
            RequestedType = requestedType;
            ActualType = actualType;
        }
    }
}