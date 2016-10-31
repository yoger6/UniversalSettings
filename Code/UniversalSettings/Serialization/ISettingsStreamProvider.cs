using System.IO;
using System.Threading.Tasks;

namespace UniversalSettings.Serialization
{
    /// <summary>
    /// Provides streams for serialization.
    /// </summary>
    public interface ISettingsStreamProvider
    {
        Task<Stream> GetEmptyStreamForWrite();
        Task<Stream> GetStreamForRead();
    }
}