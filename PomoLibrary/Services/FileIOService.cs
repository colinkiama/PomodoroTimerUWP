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

        const string GlobalSettingsFileName = "GlobalSettings.json";
        const string SessionSettingsFileName = "SessionSettings.json";

        static StorageFolder localFolder = ApplicationData.Current.LocalFolder;
        public static FileIOService Instance => lazy.Value;

        private FileIOService() { }

        public async Task<DysproseGlobalSettings?> LoadGlobalSettings()
        {
            DysproseGlobalSettings? globalSettings = null;
            try
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(DysproseGlobalSettings));
                using (var stream = await LoadFileAsync(GlobalSettingsFileName))
                {
                    globalSettings = (DysproseGlobalSettings)serializer.ReadObject(stream);
                }
            }
            catch (Exception)
            {

            }

            return globalSettings;
        }


        public async Task<DysproseSessionSettings?> LoadSessionSettings()
        {
            DysproseSessionSettings? sessionSettings = null;
            try
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(DysproseSessionSettings));
                using (var stream = await LoadFileAsync(SessionSettingsFileName))
                {
                    sessionSettings = (DysproseSessionSettings)serializer.ReadObject(stream);
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


        public async Task SaveGlobalSettingsAsync(DysproseGlobalSettings settingsToSave)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(DysproseGlobalSettings));
            using (Stream stream = await GetWriteStreamAsync(GlobalSettingsFileName))
            {
                serializer.WriteObject(stream, settingsToSave);
            }

        }



        public async Task SaveSessionSettingsAsync(DysproseSessionSettings settingsToSave)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(DysproseSessionSettings));
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
