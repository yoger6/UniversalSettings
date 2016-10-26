namespace UniversalSettings.Serialization
{
    /// <summary>
    /// Setting DTO
    /// </summary>
    public class SerializableSetting
    {
        public string Key { get; set;}
        public object Value { get; set; }
        /// <summary>
        /// Type name that can be used to parse setting value.
        /// </summary>
        public string ValueTypeName { get; set; }
    }
}