using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RadioDecadance.AssetsManagement
{
    public class EntryPointComponent : MonoBehaviour
    {
        [SerializeField] private AssetReference mainMenuScene;

        private void Start()
        {
            Addressables.LoadSceneAsync(mainMenuScene);
        }
    }
}
