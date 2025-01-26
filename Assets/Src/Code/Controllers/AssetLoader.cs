using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Assets.Src.Code.Controllers
{
    public class AssetLoader : MonoBehaviour
    {
        public static AssetLoader Instance { get; private set; }

        [SerializeField] private AssetReference _solitaireScene;
        [SerializeField] private AssetReference _clickerScene;        

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                return;
            }

            Destroy(gameObject);
        }

        public async void LoadSolitaire()
        {
            await UniTask.Delay(1000);

            try
            {
                await _solitaireScene.LoadSceneAsync();
            }
            catch (System.Exception)
            {
                Debug.Log("<color=green>Asset reference is null. Load by path</color>");
                await Addressables.LoadSceneAsync("Assets/Src/Scenes/Solitaire.unity", LoadSceneMode.Single);
                throw;
            }
        }

        public async void LoadClicker()
        {
            await UniTask.Delay(1000);

            try
            {
                await _clickerScene.LoadSceneAsync();
            }
            catch (System.Exception)
            {
                Debug.Log("<color=green>Asset reference is null. Load by path</color>");
                await Addressables.LoadSceneAsync("Assets/Src/Scenes/Clicker.unity", LoadSceneMode.Single);
                throw;
            }
        }
    }
}