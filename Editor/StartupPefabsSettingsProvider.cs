#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace RadioDecadance.AssetsManagement.Editor
{
    public class StartupPefabsSettingsProvider
    {
        private SerializedObject _serializedSettings;
        
        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {
            var provider = new SettingsProvider("Project/RadioDecadance/Start Up Preload", SettingsScope.Project)
            {
                keywords = new HashSet<string>(new[] { "Bootstrap", "Prefabs", "RadioDecadance" }),
                label = "Prefabs Bootstrap Settings",
                activateHandler = (searchContext, rootElement) =>
                {
                    var settings = StartupPrefabsSettings.GetOrCreateSettings();

                    rootElement.Add(new Label("Stores prefabs that will be always spawned before first scene loaded."));
                    
                    var title = new Label("Prefabs Bootstrap Settings")
                    {
                        style =
                        {
                            fontSize = 20,
                            unityFontStyleAndWeight = FontStyle.Bold,
                            marginBottom = 10,
                            marginTop = 10,
                            marginLeft = 5
                        }
                    };
                    
                    rootElement.Add(title);

                    rootElement.Add(new InspectorElement(settings));
                }
                
            };
            
            return provider;
        }

        [SettingsProvider]
        public static SettingsProvider CreateParentSettingsProvider()
        {
            var provider = new SettingsProvider("Project/RadioDecadance", SettingsScope.Project)
            {
                activateHandler = (searchContext, rootElement) =>
                {
                    var label = new Label("Just chill and setup your project in a sub-category for Radio Decadance settings.")
                    {
                        style =
                        {
                            marginTop = 10,
                            marginLeft = 10
                        }
                    };
                    rootElement.Add(label);
                }
            };

            return provider;
        }
    }
}
#endif
