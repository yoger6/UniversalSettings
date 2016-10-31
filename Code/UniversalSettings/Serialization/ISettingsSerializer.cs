using System.Threading.Tasks;

namespace UniversalSettings.Serialization
{
    public interface ISettingsSerializer
    {
        /// <summary>
        /// Serializes settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        Task Serialize( Settings settings );

        /// <summary>
        /// Deserializes settings.
        /// </summary>
        /// <returns>Settings</returns>
        Task<Settings> Deserialize();
    }
}