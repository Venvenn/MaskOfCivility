using System;
using System.IO;
using Unity.Plastic.Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace Escalon
{
    public struct OpenEditorDataStore
    {
        public DataStore DataStore;
        public EditorDataIncludedSO IncludedSO;
        public string FilePathDataStore;

        public void Save()
        {
            DataStore.Version = Application.version;
            DataStore.DataObjects = new SerializableDictionary<Type, IData>();
            for (int i = 0; i < IncludedSO.IncludedDataWrappers.Count; i++)
            {
                IData data = ((IEditorGameData)IncludedSO.IncludedDataWrappers[i]).GetData();
                if (data != null)
                {
                    DataStore.DataObjects.Add(data.GetType(), data);
                }
            }

            string json = JsonConvert.SerializeObject(DataStore);
            File.WriteAllText(FilePathDataStore, json);
            EditorUtility.SetDirty(IncludedSO);
            AssetDatabase.SaveAssets();
        }
    }
}