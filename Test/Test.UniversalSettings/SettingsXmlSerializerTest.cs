using System;
using System.IO;
using Common.UnitTesting;
using Moq;
using UniversalSettings;
using UniversalSettings.Serialization;
using Xunit;
using ConstructorAssert = Uwp.UnitTesting.Portable.ConstructorAssert;

namespace Test.UniversalSettings
{
    public class SettingsXmlSerializerTest
    {
        private readonly Mock<ISettingsSerializationHelper> _serializationHelperMock;
        private readonly SettingsXmlSerializer _serializer;

        public SettingsXmlSerializerTest()
        {
            _serializationHelperMock = new Mock<ISettingsSerializationHelper>();
            _serializer = new SettingsXmlSerializer( _serializationHelperMock.Object );
        }

        [Fact]
        public void Constructor_Throws_WhenSerializationHelperIsNull()
        {
            ConstructorAssert.ThrowsOnAnyNullArgument<SettingsXmlSerializer>();
        }

        [Fact]
        public void Serialize_Throws_WhenSettingsAreNull()
        {
            var validation = new Action(()=>_serializer.Serialize(null));

            Assert.Throws<ArgumentNullException>( validation );
        }

        [Fact]
        public void Serialize_Throws_WhenCantGetWriteStream()
        {
            //It'll be just null ref exception, should it be helpers responsibility
            //to throw when unable to provide valid stream?
        }

        [Fact]
        public void Serialize_WritesIntoSettingsStream()
        {
            using ( var stream = new SpyStream() )
            {
                _serializationHelperMock.Setup( x => x.GetEmptyStreamForWrite() )
                                        .Returns( stream );

                _serializer.Serialize( GetSampleSettings() );

                Assert.Equal( ExpectedSerializedSampleSettings(), stream.ToString() );
            }
        }
        
        [Fact]
        public void Deserialize_Throws_WhenReadingEmptyStream()
        {
            using ( var stream = new SpyStream() )
            {
                _serializationHelperMock.Setup( x => x.GetStreamForRead() )
                                        .Returns( stream );

                var validation = new Action( () => _serializer.Deserialize() );

                Assert.Throws<InvalidDataException>( validation );
            }
        }

        [Fact]
        public void Deserialize_ReturnsSettingsFromStream()
        {
            using ( var stream = new SpyStream(ExpectedSerializedSampleSettings()) )
            {
                _serializationHelperMock.Setup( x => x.GetStreamForRead() )
                                        .Returns( stream );

                var settings = _serializer.Deserialize();

                Assert.True( settings.IsSet( "number" ) );
                Assert.Equal( 1, settings.Get<int>( "number" ) );
            }
        }
        
        private string ExpectedSerializedSampleSettings()
        {
            return "<?xml version=\"1.0\" encoding=\"utf-8\"?><ArrayOfSerializableSetting xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><SerializableSetting><Key>number</Key><Value xsi:type=\"xsd:int\">1</Value><ValueTypeName>System.Int32</ValueTypeName></SerializableSetting></ArrayOfSerializableSetting>";
        }

        private Settings GetSampleSettings()
        {
            var settings = new Settings();
            settings.Set( "number", 1 );

            return settings;
        }
    }
}