using Assets.Src.Code.Controllers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Src.Code.Clicker.UI
{
    public class HamsterSettingsHUD : MonoBehaviour
    {
        [SerializeField] private HamsterHUD _hud;
        [SerializeField] private Button _continue, _solitaire, _menu;
        [SerializeField] private GameObject _spawner;

        private void Start()
        {
            _continue.onClick.AddListener(Continue);
            _solitaire.onClick.AddListener(PlaySolitaire);
            _menu.onClick.AddListener(LoadMenu);
        }

        private void OnEnable()
        {
            _spawner.SetActive(false);
        }

        private void Continue()
        {
            _hud.SwitchTimer(true);
            gameObject.SetActive(false);
            _spawner.SetActive(true);
        }

        private void PlaySolitaire()
        {
            _solitaire.onClick.RemoveAllListeners();
            AssetLoader.Instance.LoadSolitaire();
        }
        private void LoadMenu()
        {
            _menu.onClick.RemoveAllListeners();
            SceneManager.LoadScene(0);
        }

        private void OnDestroy()
        {
            _continue.onClick.RemoveListener(Continue);
            _menu.onClick.RemoveAllListeners();
            _solitaire.onClick.RemoveAllListeners();
        }
    }
}