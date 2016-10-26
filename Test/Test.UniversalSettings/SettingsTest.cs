using System;
using System.Collections.Generic;
using UniversalSettings;
using Uwp.UnitTesting.Portable;
using Xunit;

namespace Test.UniversalSettings
{
    public class SettingsTest
    {
        private readonly Settings _settings;

        public SettingsTest()
        {
            _settings = new Settings();
        }

        [Fact]
        public void Get_Throws_WhenSettingNameIsNotValid()
        {
            StringAssert.ThrowsWhenNullEmptyOrWhitespace(
                ( name ) => _settings.Get<object>( name ) );
        }

        [Fact]
        public void Get_Throws_WhenSettingIsNotSet()
        {
            var validation = new Action(()=>_settings.Get<object>( "not existing at this point" ));

            Assert.Throws<SettingNotSetException>( validation );
        }

        [Fact]
        public void Set_Throws_WhenSettingNameIsNotValid()
        {
            StringAssert.ThrowsWhenNullEmptyOrWhitespace(
                ( name ) => _settings.Set( name, "some value" ) );
        }

        [Fact]
        public void Set_Throws_WhenSettingValueIsNull()
        {
            var validation = new Action( () => _settings.Set( "proper name", (object) null ) );

            Assert.Throws<ArgumentNullException>( validation );
        }

        [Fact]
        public void IsSet_Throws_WhenSettingNameIsNotValid()
        {
            StringAssert.ThrowsWhenNullEmptyOrWhitespace(
                ( name ) => _settings.IsSet( name ) );
        }

        [Fact]
        public void IsSet_ReturnsFalse_WhenSettingDoesNotExist()
        {
            var result = _settings.IsSet( "proper name" );

            Assert.False( result );
        }

        [Fact]
        public void Set_AddsSetting()
        {
            var settingName = "set name";
            _settings.Set( settingName, "irrelevant value" );

            var isSet = _settings.IsSet( settingName );

            Assert.True( isSet );
        }

        [Fact]
        public void Set_OverwritesSettingWithTheSameName()
        {
            var settingName = "the number";
            var expectedValue = 1;
            _settings.Set( settingName, 0 );

            _settings.Set( settingName, expectedValue );

            Assert.Equal( expectedValue, _settings.Get<object>( settingName ) );
        }

        [Fact]
        public void Get_Throws_WhenCalledWithInvalidType()
        {
            var settingName = "the number";
            _settings.Set( settingName, 1 );

            var validation = new Action(()=>_settings.Get<string>( settingName ));

            Assert.Throws<InvalidSettingTypeRequestedException>( validation );
        }

        [Fact]
        public void GetAll_ReturnsReadOnlyDictionaryWithSettings()
        {
            var expectedKey = "the number";
            var expectedValue = 0;
            _settings.Set( expectedKey,expectedValue );

            var settingsDictionary = _settings.GetAll();

            Assert.True( settingsDictionary.ContainsKey(expectedKey) );
            Assert.True( (int)settingsDictionary[expectedKey] == expectedValue );
        }
    }
}