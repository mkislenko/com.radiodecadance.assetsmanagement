using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RadioDecadance.AssetsManagement
{
    [Serializable]
    public struct BootstrapAssetEntry
    {
        public AssetReference assetReference;
        [Tooltip("If true will log error if the asset is not present in the Addressables system.")]
        public bool isRequired;
    }

    /// <summary>
    /// Configuration for prefabs that should be instantiated at startup and be Don'tDestroyOnLoad.
    /// </summary>
    public class StartupPrefabsSettings : ScriptableObject
    {
        private const string ResourcesFolderPath = "Assets/Resources";
        private const string FolderPath = "RadioDecadance";
        private const string FileName = "StartupPrefabsSettings";
        private const string FileExtension = ".asset";
        private const string FullFileName = FileName + FileExtension;
        private const string FullAssetPath = ResourcesFolderPath + "/" + FolderPath + "/" + FullFileName;
        private const string ResourcesPath = FolderPath + "/" + FileName;

        [SerializeField]
        private List<BootstrapAssetEntry> prefabsToHandle = new List<BootstrapAssetEntry>();

        public List<BootstrapAssetEntry> PrefabsToHandle => prefabsToHandle;
        
        // ProjectSD

        public static StartupPrefabsSettings GetOrCreateSettings()
        {
#if UNITY_EDITOR
            
            var settings = UnityEditor.AssetDatabase.LoadAssetAtPath<StartupPrefabsSettings>(FullAssetPath);
            if (settings == null)
            {
                settings = ScriptableObject.CreateInstance<StartupPrefabsSettings>();
        
                if (!Directory.Exists(FolderPath))
                {
                    Directory.CreateDirectory(FolderPath);
                }
                UnityEditor.AssetDatabase.CreateAsset(settings, FullAssetPath);
                UnityEditor.AssetDatabase.SaveAssets();
            }
        
            return settings;
#else
            var settings = Resources.Load<StartupPrefabsSettings>(ResourcesPath);

            if (settings == null)
            {
                Debug.LogError($"PrefabsBootstrapSettings not found on path: {ResourcesPath}]");
            }
            
            return settings;
#endif
        }
    }
}
