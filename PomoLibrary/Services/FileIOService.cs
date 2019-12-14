using PomoLibrary.Structs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace PomoLibrary.Services
{
    public class FileIOService
    {
        // Singleton Pattern with "Lazy"
        private FileIOService _fileIOService = null;
        private static Lazy<FileIOService> lazy =
            new Lazy<FileIOService>(() => new FileIOService());

        const string SessionSettingsFileName = "SessionSettings.json";
        const string CurrentSessionSettingsFileName = "CurrentSession.json";

        static StorageFolder localFolder = ApplicationData.Current.LocalFolder;
        
        public static FileIOService Instance => lazy.Value;

        private FileIOService() { }

        public async Task<PomoSessionSettings?> LoadSessionSettings()
        {
            PomoSessionSettings? sessionSettings = null;
            try
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(PomoSessionSettings));
                using (var stream = await LoadFileAsync(SessionSettingsFileName))
                {
                    sessionSettings = (PomoSessionSettings)serializer.ReadObject(stream);
                }
            }
            catch (Exception)
            {

            }

            return sessionSettings;
        }

        private async Task<Stream> LoadFileAsync(string fileName)
        {
            Stream stream = null;
            try
            {
                StorageFile file = (StorageFile)await localFolder.TryGetItemAsync(fileName);
                stream = await file.OpenStreamForReadAsync();
            }
            catch (Exception)
            {
                await localFolder.CreateFileAsync(fileName);
            }
            return stream;
        }

        public async Task SaveSessionSettingsAsync(PomoSessionSettings settingsToSave)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(PomoSessionSettings));
            using (Stream stream = await GetWriteStreamAsync(SessionSettingsFileName))
            {
                serializer.WriteObject(stream, settingsToSave);
            }
        }

        private async Task<Stream> GetWriteStreamAsync(string fileName)
        {
            var file = await localFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

            return await file.OpenStreamForWriteAsync();
        }
    }
}
