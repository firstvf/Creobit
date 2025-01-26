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

            await Addressables.LoadSceneAsync(_solitaireScene, LoadSceneMode.Single);
        }

        public async void LoadClicker()
        {
            await UniTask.Delay(1000);

            await _clickerScene.LoadSceneAsync();
        }
    }
}