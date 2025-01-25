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
        [SerializeField] private Button _openSettings;
        [SerializeField] private Button _continue;
        [SerializeField] private Button _menu;
        [SerializeField] private Button _restart;
        [SerializeField] private Button _save;
        [SerializeField] private Button _newGame;
        [SerializeField] private Button _loadGame;

        private void Start()
        {
            _continue.onClick.AddListener(Continue);
            _menu.onClick.AddListener(Menu);
            _restart.onClick.AddListener(Restart);
            _save.onClick.AddListener(Save);
            _newGame.onClick.AddListener(NewGame);
            _loadGame.onClick.AddListener(LoadGame);
            _openSettings.onClick.AddListener(OpenSettingsHud);
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
        => SceneManager.LoadScene(0);

        private void Restart()
        => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        private void Save()
        => CardController.Instance.Save();

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
            _menu.onClick.RemoveListener(Menu);
            _restart.onClick.RemoveListener(Restart);
            _save.onClick.RemoveListener(Save);
            _newGame.onClick.RemoveListener(NewGame);
            _loadGame.onClick.RemoveListener(LoadGame);
            _openSettings.onClick.RemoveListener(OpenSettingsHud);
        }
    }
}