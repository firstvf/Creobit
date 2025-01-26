using Assets.Src.Code.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Src.Code.Menu
{
    public class MenuHUD : MonoBehaviour
    {
        [SerializeField] private Button _solitaireGame;
        [SerializeField] private Button _clickerGame;

        private void Start()
        {
            _solitaireGame.onClick.AddListener(LoadSolitaire);
            _clickerGame.onClick.AddListener(LoadClicker);
        }

        private void LoadSolitaire()
        {
            _solitaireGame.onClick.RemoveAllListeners();
            AssetLoader.Instance.LoadSolitaire();
        }

        private void LoadClicker()
        {
            _clickerGame.onClick.RemoveAllListeners();
            AssetLoader.Instance.LoadClicker();
        }

        private void OnDestroy()
        {
            _solitaireGame.onClick.RemoveAllListeners();
            _clickerGame.onClick.RemoveAllListeners();
        }
    }
}