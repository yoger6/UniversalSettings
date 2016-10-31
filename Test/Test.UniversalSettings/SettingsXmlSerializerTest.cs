using System;
using System.IO;
using System.Threading.Tasks;
using Common.UnitTesting;
using Moq;
using UniversalSettings;
using UniversalSettings.Serialization;
using Xunit;

namespace Test.UniversalSettings
{
    public class SettingsXmlSerializerTest
    {
        private readonly Mock<ISettingsStreamProvider> _streamProviderMock;
        private readonly SettingsXmlSerializer _serializer;

        public SettingsXmlSerializerTest()
        {
            _streamProviderMock = new Mock<ISettingsStreamProvider>();
            _serializer = new SettingsXmlSerializer( _streamProviderMock.Object );
        }

        [Fact]
        public void Constructor_Throws_WhenSerializationHelperIsNull()
        {
            ConstructorAssert.ThrowsOnAnyNullArgument<SettingsXmlSerializer>();
        }

        [Fact]
        public async Task Serialize_Throws_WhenSettingsAreNull()
        {
            var validation = new Func<Task>(()=>_serializer.Serialize(null));

            await Assert.ThrowsAsync<ArgumentNullException>( validation );
        }

        [Fact]
        public async Task Serialize_WritesIntoSettingsStream()
        {
            using ( var stream = new SpyStream() )
            {
                _streamProviderMock.Setup( x => x.GetEmptyStreamForWrite() )
                                   .Returns( Task.FromResult(  (Stream)stream ));

                await _serializer.Serialize( GetSampleSettings() );

                Assert.Equal( ExpectedSerializedSampleSettings(), stream.ToString() );
            }
        }
        
        [Fact]
        public async Task Deserialize_Throws_WhenReadingEmptyStream()
        {
            using ( var stream = new SpyStream() )
            {
                _streamProviderMock.Setup( x => x.GetStreamForRead() )
                                   .Returns( Task.FromResult( (Stream)stream ) );

                var validation = new Func<Task>( () => _serializer.Deserialize() );

                await Assert.ThrowsAsync<InvalidDataException>( validation );
            }
        }

        [Fact]
        public async Task Deserialize_ReturnsSettingsFromStream()
        {
            using ( var stream = new SpyStream(ExpectedSerializedSampleSettings()) )
            {
                _streamProviderMock.Setup( x => x.GetStreamForRead() )
                                   .Returns( Task.FromResult( (Stream)stream ) );

                var settings = await _serializer.Deserialize();
                
                Assert.Equal( 1, settings.Get<int>( "number" ) );
                Assert.Equal( false, settings.Get<bool>( "condition" ) );
                Assert.Equal( 2.05, settings.Get<double>( "double number" ) );
                Assert.Equal( "hello world!", settings.Get<string>( "text" ) );
            }
        }
        
        private string ExpectedSerializedSampleSettings()
        {
            return @"<?xml version=""1.0"" encoding=""utf-8""?><ArrayOfSerializableSetting xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""><SerializableSetting><Key>number</Key><Value xsi:type=""xsd:int"">1</Value></SerializableSetting><SerializableSetting><Key>condition</Key><Value xsi:type=""xsd:boolean"">false</Value></SerializableSetting><SerializableSetting><Key>double number</Key><Value xsi:type=""xsd:double"">2.05</Value></SerializableSetting><SerializableSetting><Key>text</Key><Value xsi:type=""xsd:string"">hello world!</Value></SerializableSetting></ArrayOfSerializableSetting>";
        }

        private Settings GetSampleSettings()
        {
            var settings = new Settings();
            settings.Set( "number", 1 );
            settings.Set( "condition", false );
            settings.Set( "double number", 2.05 );
            settings.Set( "text", "hello world!" );

            return settings;
        }
    }
}