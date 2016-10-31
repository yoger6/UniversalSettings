using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace UniversalSettings.Serialization
{
    /// <summary>
    /// Serializes and deserializes settings using xml format.
    /// </summary>
    public class SettingsXmlSerializer : ISettingsSerializer
    {
        private readonly ISettingsStreamProvider _streamProvider;
        private readonly XmlSerializer _serializer;

        /// <summary>
        /// Creates instance of class.
        /// </summary>
        /// <param name="streamProvider">Serialization helper</param>
        public SettingsXmlSerializer( ISettingsStreamProvider streamProvider )
        {
            if ( streamProvider == null ) throw new ArgumentNullException( nameof( streamProvider ) );

            _streamProvider = streamProvider;
            _serializer = new XmlSerializer( typeof(SerializableSetting[]) );
        }

        /// <summary>
        /// Serializes settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public async Task Serialize( Settings settings )
        {
            if ( settings == null )
            {
                throw new ArgumentNullException( nameof( settings ) );
            }

            var serializableSettings = GetSerializableSettings( settings );
            
            using ( var stream = await _streamProvider.GetEmptyStreamForWrite() )
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

        /// <summary>
        /// Deserializes settings.
        /// </summary>
        /// <returns>Settings</returns>
        public async Task<Settings> Deserialize()
        {
            using ( var stream = await _streamProvider.GetStreamForRead() )
            {
                if ( stream.Length == 0 )
                {
                    throw new InvalidDataException("Stream that should contain settings was empty.");
                }
                var settingsDtos = (SerializableSetting[])_serializer.Deserialize( stream );

                return GetSettingsWithDeserializedDtos( settingsDtos );
            }
        }

        private IEnumerable<SerializableSetting> GetSerializableSettings( Settings settings )
        {
            return settings.GetAll().Select( pair => new SerializableSetting
            {
                Key = pair.Key,
                Value = pair.Value
            } );
        }

        private Settings GetSettingsWithDeserializedDtos( SerializableSetting[] settingsDtos )
        {
            var settings = new Settings();

            foreach ( var serializableSetting in settingsDtos )
            {
                settings.Set( serializableSetting.Key, serializableSetting.Value );
            }

            return settings;
        }
    }
}