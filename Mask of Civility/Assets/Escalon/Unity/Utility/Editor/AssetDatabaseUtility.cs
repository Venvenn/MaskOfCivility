using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Escalon
{
    public static class AssetDatabaseUtility
    {
        public static List<ScriptableObject> FindScriptableObjectAssetsByType(Type type, out string[] guids)
        {
            List<ScriptableObject> assets = new List<ScriptableObject>();

            guids = AssetDatabase.FindAssets($"t:{type.FullName}");

            for (int i = 0; i < guids.Length; i++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);

                var asset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPath);

                if (asset != null)
                {
                    assets.Add(asset);
                }
            }

            return assets;
        }
        public static List<T> FindScriptableObjectAssetsByType<T>(out string[] guids) where T : ScriptableObject
        {
            List<T> assets = new List<T>();

            guids = AssetDatabase.FindAssets($"t:{typeof(T).FullName}");

            for (int i = 0; i < guids.Length; i++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);

                var asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);

                if (asset != null)
                {
                    assets.Add(asset);
                }
            }

            return assets;
        }
    }
}

