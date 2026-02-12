#if UNITY_EDITOR
using UnityEditor;

namespace RadioDecadance.AssetsManagement.Editor
{
    [InitializeOnLoad]
    public static class PrefabsBootstrapInitializerEditor
    {
        static PrefabsBootstrapInitializerEditor()
        {
            // Ensure settings exist on startup
            EditorApplication.delayCall += () => StartupPrefabsSettings.GetOrCreateSettings();
        }
    }
}
#endif
