using System;
using UniversalSettings;
using UniversalSettings.Serialization;
using Xunit;

namespace Test.UniversalSettings
{
    public class TypeSwitchWithReturnTest
    {
        private readonly TypeSwitch<int> _switch;

        public TypeSwitchWithReturnTest()
        {
            _switch = new TypeSwitch<int>();
        }

        [Fact]
        public void Execute_InvokesActionAssignedToType()
        {
            _switch.Set( typeof(int), () => 0 );
            _switch.Set( typeof(string), () => 1 );

            var result = _switch.Execute( typeof(string) );

            Assert.Equal( 1, result );
        }

        [Fact]
        public void ExecutesFallbackActionAssignedInConstructorWhenExecutedTypeNotFoundOrNull()
        {
            var swithWithFallback = new TypeSwitch<int>( () => 0 );
            swithWithFallback.Set( typeof(int), () => 1 );

            var result = swithWithFallback.Execute( typeof(bool) );

            Assert.Equal( 0, result );
        }

        [Fact]
        public void ThrowsIfExecutedTypeIsNotSetAndNoFallbackSet()
        {
            var execute = new Action( () => _switch.Execute( typeof(bool) ) );

            Assert.Throws<InvalidOperationException>( execute );
        }

        [Fact]
        public void ThrowsIfExecutedTypeIsNullAndNoFallbackSet()
        {
            var setting = new Action( () => _switch.Execute( null ) );

            Assert.Throws<InvalidOperationException>( setting );
        }

        [Fact]
        public void ThrowsIfTypeIsAlreadySet()
        {
            var setting = new Action( () => _switch.Set( typeof(int), () => 0 ) );

            setting.Invoke();

            Assert.Throws<ArgumentException>( setting );
        }

        [Fact]
        public void ThrowsIfTypeIsSetWithNullAction()
        {
            var setting = new Action( () => _switch.Set( typeof(int), null ) );

            Assert.Throws<ArgumentNullException>( setting );
        }
    }
}