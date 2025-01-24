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
        [SerializeField] private Button _undoButton;
        [SerializeField] private GameObject _disableInputScreen;
        [SerializeField] private GameObject _endGameScreen;
        [SerializeField] private Text _endGameText;
        private int _movesCounter;

        private void Start()
        {
            _movesCounterText.text = "Moves: " + _movesCounter.ToString();
            CardController.Instance.OnMoveHandler += ChangeMovesCounter;
            CardController.Instance.OnUnavailableMoveHandler += GameOver;
            _continueButton.onClick.AddListener(ContinueGame);
            _restartButton.onClick.AddListener(RestartGame);
            _undoButton.onClick.AddListener(CardController.Instance.Hand.Undo);
        }

        private void ChangeMovesCounter()
        {
            _movesCounter++;
            _movesCounterText.text = "Moves: " + _movesCounter.ToString();
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

        private void GameOver(bool isWinGame)
        {
            Debug.Log("IS game over? " + isWinGame);

            if (isWinGame)
                _endGameText.text = "Win";
            else
                _endGameText.text = "Game over";

            _endGameScreen.SetActive(true);
        }

        private void OnDestroy()
        {
            CardController.Instance.OnMoveHandler -= ChangeMovesCounter;
            CardController.Instance.OnUnavailableMoveHandler -= GameOver;
            _continueButton.onClick.RemoveListener(ContinueGame);
            _restartButton.onClick.RemoveListener(RestartGame);
            _undoButton.onClick.RemoveListener(CardController.Instance.Hand.Undo);
        }
    }
}