using UnityEditor;
using UnityEditor.AddressableAssets.Build.DataBuilders;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEditor.Build;
using UnityEditor.Build.Profile;
using UnityEngine;

// Extend your current build script or BuildScriptPackedMode
namespace RadioDecadance.AssetsManagement.Editor
{
    [CreateAssetMenu(fileName = "CustomAddressablesBuildScript", menuName = "Addressables/Custom Addressables Build Script")]
    public class CustomAddressablesBuildScript : BuildScriptPackedMode
    
    {
        // The label used to mark groups that should be debug-only
        
        [Tooltip("If group name contains this label it will be included only if define is set in Player Settings")]
        [SerializeField] private LabelToDefineFilter[] labelToDefineFilters;

        [System.Serializable]
        private struct LabelToDefineFilter
        {
            public string label;
            public string define;
        }

        private static string GetCombinedScriptingDefines()
        {
            var currentBuildTarget =
                NamedBuildTarget.FromBuildTargetGroup(EditorUserBuildSettings.selectedBuildTargetGroup);

            string defines = PlayerSettings.GetScriptingDefineSymbols(currentBuildTarget);

            // 2. Get Build Profile Defines (additive overrides introduced in Unity 6)
            // Check if the Build Profile API is available and an active profile exists

            var activeProfile = BuildProfile.GetActiveBuildProfile();

            if (activeProfile == null)
            {
                return defines;
            }

            // The scriptingDefines property exposes the defines set on that profile
            string[] profileDefines = activeProfile.scriptingDefines;

            if (profileDefines == null)
            {
                return defines;
            }

            foreach (string define in profileDefines)
            {
                if (defines.Contains(define))
                {
                    continue;
                }

                defines += ";";
                defines += define;
            }

            // 3. Combine and return as a single semicolon-separated string
            return defines;
        }

        protected override string ProcessBundledAssetSchema(BundledAssetGroupSchema schema,
            AddressableAssetGroup assetGroup,
            AddressableAssetsBuildContext aaContext)
        {
            if (schema == null || !schema.IncludeInBuild)
            {
                return string.Empty;
            }

            foreach (LabelToDefineFilter filter in labelToDefineFilters)
            {
                if (!schema.name.Contains(filter.label))
                {
                    return base.ProcessBundledAssetSchema(schema, assetGroup, aaContext);
                }

                string buildDefines = GetCombinedScriptingDefines();
                bool isDebugBuild = buildDefines.Contains(filter.define);

                //debug assets in non debug build excluded
                if (!isDebugBuild)
                {
                    return string.Empty;
                }
            }
            
            //non debug assets are always included

            return base.ProcessBundledAssetSchema(schema, assetGroup, aaContext);
        }
    }
}