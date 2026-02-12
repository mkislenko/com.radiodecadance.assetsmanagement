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
        private const string FolderPath = "Assets/Resources/RadioDecadance";
        private const string FileNamePath = "StartupPrefabsSettings.asset";
        private const string FullPath = FolderPath + "/" + FileNamePath;

        [SerializeField]
        private List<BootstrapAssetEntry> prefabsToHandle = new List<BootstrapAssetEntry>();

        public List<BootstrapAssetEntry> PrefabsToHandle => prefabsToHandle;
        
        // ProjectSD

        public static StartupPrefabsSettings GetOrCreateSettings()
        {
#if UNITY_EDITOR
            
            var settings = UnityEditor.AssetDatabase.LoadAssetAtPath<StartupPrefabsSettings>(FullPath);
            if (settings == null)
            {
                settings = ScriptableObject.CreateInstance<StartupPrefabsSettings>();
        
                if (!Directory.Exists(FolderPath))
                {
                    Directory.CreateDirectory(FolderPath);
                }
                UnityEditor.AssetDatabase.CreateAsset(settings, FullPath);
                UnityEditor.AssetDatabase.SaveAssets();
            }
        
            return settings;
#else
            return Resources.Load<PrefabsBootstrapSettings>("PrefabsBootstrapSettings");
#endif
        }
    }
}
