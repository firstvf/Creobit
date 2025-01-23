using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Src.Code.Solitaire.UI
{
    public class GameHUD : MonoBehaviour
    {
        [SerializeField] private Text _timerText;
        [SerializeField] private Text _movesCounterText;
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _restartButton;
        [SerializeField] private GameObject _disableInputScreen;
        [SerializeField] private GameObject _endGameScreen;
        [SerializeField] private Text _endGameText;
        private int _movesCounter;

        private void Start()
        {
            _movesCounterText.text = _movesCounter.ToString();
            CardController.Instance.OnMoveHandler += ChangeMovesCounter;
            CardController.Instance.OnUnavailableMoveHandler += GameOver;
            _continueButton.onClick.AddListener(ContinueGame);
            _restartButton.onClick.AddListener(RestartGame);
        }

        private void ChangeMovesCounter()
        {
            _movesCounter++;
            _movesCounterText.text = _movesCounter.ToString();
        }

        private void ContinueGame()
        {
            Debug.Log("Continue game");
            _endGameScreen.gameObject.SetActive(false);
        }

        private void RestartGame()
        {
            Debug.Log("Restart game");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void GameOver(bool isGameOver)
        {
            Debug.Log("IS game over? " + isGameOver);

            if (isGameOver)
                _endGameText.text = "Game over";
            else _endGameText.text = "Win";

            _endGameScreen.SetActive(true);
        }

        private void OnDestroy()
        {
            CardController.Instance.OnMoveHandler -= ChangeMovesCounter;
            CardController.Instance.OnUnavailableMoveHandler -= GameOver;
            _continueButton.onClick.RemoveListener(ContinueGame);
            _restartButton.onClick.RemoveListener(RestartGame);
        }
    }
}