using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace UniversalSettings.Serialization
{
    /// <summary>
    /// Serializes and deserializes settings using xml format.
    /// </summary>
    public class SettingsXmlSerializer
    {
        private readonly ISettingsSerializationHelper _serializationHelper;
        private readonly XmlSerializer _serializer;

        /// <summary>
        /// Creates instance of class.
        /// </summary>
        /// <param name="serializationHelper">Serialization helper</param>
        public SettingsXmlSerializer( ISettingsSerializationHelper serializationHelper )
        {
            if ( serializationHelper == null ) throw new ArgumentNullException( nameof( serializationHelper ) );

            _serializationHelper = serializationHelper;
            _serializer = new XmlSerializer( typeof(SerializableSetting[]) );
        }

        /// <summary>
        /// Serializes settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public void Serialize( Settings settings )
        {
            if ( settings == null )
            {
                throw new ArgumentNullException( nameof( settings ) );
            }

            var serializableSettings = GetSerializableSettings( settings );
            
            using ( var stream = _serializationHelper.GetEmptyStreamForWrite() )
            {
                var writerSettings = new XmlWriterSettings
                {
                    Encoding = new UTF8Encoding( false )
                };
                using ( var writer = XmlWriter.Create( stream, writerSettings) )
                {
                    _serializer.Serialize( writer, serializableSettings.ToArray() );
                }
            }
        }

        public Settings Deserialize()
        {
            using ( var stream = _serializationHelper.GetStreamForRead() )
            {
                if ( stream.Length == 0 )
                {
                    throw new InvalidDataException("Stream that should contain settings was empty.");
                }
                var settingsDtos = (SerializableSetting[])_serializer.Deserialize( stream );

                return GetParsedDtosIntoSettings( settingsDtos );
            }
        }

        private IEnumerable<SerializableSetting> GetSerializableSettings( Settings settings )
        {
            return settings.GetAll().Select( pair => new SerializableSetting
            {
                Key = pair.Key,
                Value = pair.Value,
                ValueTypeName = pair.Value.GetType().ToString()
            } );
        }

        //TODO: Refactor parser out of the class
        private Settings GetParsedDtosIntoSettings( SerializableSetting[] settingsDtos )
        {
            var settings = new Settings();

            foreach ( var serializableSetting in settingsDtos )
            {
                var valueType = Type.GetType( serializableSetting.ValueTypeName );
                var typeSwitch = new TypeSwitch<object>();
                typeSwitch.Set<int>( () => 1 );

                var parsedValue = typeSwitch.Execute( valueType );

                settings.Set( serializableSetting.Key, parsedValue );
            }

            return settings;
        }
    }
}