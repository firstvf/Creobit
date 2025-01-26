using Assets.Src.Code.Controllers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Src.Code.Solitaire.UI
{
    public class SettingsHUD : MonoBehaviour
    {
        [SerializeField] private GameObject _settingsScreen;
        [SerializeField] private GameObject _startChoiceScreen;
        [Space]
        [SerializeField]
        private Button _openSettings, _continue, _menu, _newGame, _loadGame, _clickerGame;

        private void Start()
        {
            _continue.onClick.AddListener(Continue);
            _menu.onClick.AddListener(Menu);
            _newGame.onClick.AddListener(NewGame);
            _loadGame.onClick.AddListener(LoadGame);
            _openSettings.onClick.AddListener(OpenSettingsHud);
            _clickerGame.onClick.AddListener(LoadClickerGame);
        }

        private void OpenSettingsHud()
        {
            _settingsScreen.SetActive(true);
            CardController.Instance.GameHud.SwitchTimer(false);
        }

        private void Continue()
        {
            _settingsScreen.SetActive(false);
            CardController.Instance.GameHud.SwitchTimer(true);
        }

        private void Menu()
        {
            _menu.onClick.RemoveAllListeners();
            SceneManager.LoadScene(0);
        }

        private void LoadClickerGame()
        {
            _clickerGame.onClick.RemoveAllListeners();
            AssetLoader.Instance.LoadClicker();
        }

        private void NewGame()
        {
            _settingsScreen.SetActive(false);
            _startChoiceScreen.SetActive(false);
            CardController.Instance.StartGame(false);
        }

        private void LoadGame()
        {
            _settingsScreen.SetActive(false);
            _startChoiceScreen.SetActive(false);
            CardController.Instance.StartGame(true);
        }

        private void OnDestroy()
        {
            _continue.onClick.RemoveListener(Continue);
            _menu.onClick.RemoveAllListeners();
            _newGame.onClick.RemoveListener(NewGame);
            _loadGame.onClick.RemoveListener(LoadGame);
            _openSettings.onClick.RemoveListener(OpenSettingsHud);
            _clickerGame.onClick.RemoveAllListeners();
        }
    }
}