using System;
using System.IO;
using Newtonsoft.Json;

namespace Escalon
{
    /// <summary>
    /// Used to manage the reading and writing of game data
    /// </summary>
    public class DataManager : Aspect
    {
        private NonBoxingDictionary<Type, IData> _data = new NonBoxingDictionary<Type, IData>();
        
        public T Read<T>() where T : struct, IData
        {
            Type type = typeof(T);
            Debug.Assert(_data.ContainsKey(type), $"DataStore does not contain any data of type {type}");
            _data.TryGetValue(typeof(T), out T data);
            return data;
        }
        
        public bool TryRead<T>(out T data) where T : struct, IData
        {
            return _data.TryGetValue(typeof(T), out data);
        }
        
        public void Write<T>(T data) where T : struct, IData
        {
            _data.Add(typeof(T), data);
        }
        
        public void TryWrite<T>(T data) where T : struct, IData
        {
            _data.TryAdd(typeof(T), data);
        }

        public bool Has<T>() where T : struct, IData
        {
            return _data.ContainsKey(typeof(T));
        }
        
        public void ClearData<T>() where T : struct
        {
            _data.Remove(typeof(T));
        }
        
        public void ClearAll()
        {
            _data.Clear();
        }

        public bool LoadDataStore(string directoryPath, string configName = "")
        {
            if (!Directory.Exists(directoryPath))
            {
                Debug.Log($"Folder {directoryPath} not found so could not load Datastore configuration");
                return false;
            }

            string[] filePaths = Directory.GetFiles(directoryPath, "*.json", SearchOption.TopDirectoryOnly);

            Debug.Assert(filePaths.Length > 0, $"[DataManager] No Data Config Files found at directory path: {directoryPath}");

            string fileToLoad = configName;
            if (string.IsNullOrEmpty(fileToLoad))
            {
                fileToLoad = filePaths[0];
            }
            
            string dataStoreJson = File.ReadAllText(fileToLoad);

            DataStore dataStore = JsonConvert.DeserializeObject<DataStore>(dataStoreJson);
            _data = new NonBoxingDictionary<Type, IData>(dataStore.DataObjects);
            return true;
        }

        public void SaveDataStore(string directoryPath)
        {
            DataStore dataStore = new DataStore()
            {
                DataObjects = _data.ToDictionary()
            };

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string dataStoreJson = JsonConvert.SerializeObject(dataStore);
            File.WriteAllText(directoryPath + "/staticDataStore.json", dataStoreJson);
        }
    }
}
