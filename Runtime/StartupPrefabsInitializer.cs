using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace RadioDecadance.AssetsManagement
{
    /// <summary>
    ///     General pre-instantiator for Addressable prefabs that should exist before the first scene loads.
    ///     Configuration is loaded from PrefabsBootstrapSettings ScriptableObject.
    /// </summary>
    public static class StartupPrefabsInitializer
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            StartupPrefabsSettings settings = StartupPrefabsSettings.GetOrCreateSettings();

            if (settings == null)
            {
                Debug.LogError("PrefabsBootstrapSettings not found. Skipping preloading.");
                return;
            }

            Debug.Log("Preloading Addressable prefabs before first scene load...");

            foreach (var entry in settings.PrefabsToHandle)
            {
                if (entry.assetReference == null || !entry.assetReference.RuntimeKeyIsValid())
                {
                    continue;
                }

                LoadAndInstantiate(entry);
            }
        }

        private static void LoadAndInstantiate(BootstrapAssetEntry entry)
        {
            AsyncOperationHandle<GameObject> opHandle = entry.assetReference.LoadAssetAsync<GameObject>();

            opHandle.Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    GameObject prefab = handle.Result;

                    if (prefab != null)
                    {
                        GameObject instance = Object.Instantiate(prefab);
                        instance.name = prefab.name + " (Auto-Generated)";
                        Object.DontDestroyOnLoad(instance);
                        Debug.Log($"Addressable prefab '{prefab.name}' instantiated and marked as DontDestroyOnLoad.");
                    }
                    else
                    {
                        if (entry.isRequired)
                        {
                            Debug.LogError($"Loaded prefab for AssetReference '{entry.assetReference.RuntimeKey}' is null.");
                        }
                    }
                }
                else
                {
                    if (entry.isRequired)
                    {
                        Debug.LogError($"Failed to load prefab with AssetReference '{entry.assetReference.RuntimeKey}'. Status: {handle.Status}");
                    }
                }
            };

            // Ensure completion in initialization phase
            if (!opHandle.IsDone)
            {
                opHandle.WaitForCompletion();
            }
        }
    }
}