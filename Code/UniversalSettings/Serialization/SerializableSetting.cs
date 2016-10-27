using System.Xml.Serialization;

namespace UniversalSettings.Serialization
{
    /// <summary>
    /// Setting DTO
    /// </summary>
    [XmlRoot( "Setting" )]
    public class SerializableSetting
    {
        [XmlElement( "Key" )]
        public string Key { get; set; }

        [XmlElement( "Value" )]
        public object Value { get; set; }

        ///// <summary>
        ///// Type name that can be used to parse setting value.
        ///// </summary>
        //[XmlAttribute( "ValueType" )]
        //public string ValueTypeName { get; set; }
    }
}