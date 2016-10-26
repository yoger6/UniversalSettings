using System.IO;

namespace UniversalSettings.Serialization
{
    /// <summary>
    /// Provides streams for serialization.
    /// </summary>
    public interface ISettingsSerializationHelper
    {
        Stream GetEmptyStreamForWrite();
        Stream GetStreamForRead();
    }
}