using Assets.Src.Code.Data.JsonData;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Src.Code.Solitaire.UI
{
    public class GameHUD : MonoBehaviour, IData
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
        private int _timer;
        private CancellationTokenSource _cancellationTimerToken;
        private const string TIMER_KEY = "timer";

        private void Start()
        {
            CardController.Instance.OnLoadingDataHandler += SwitchDisableInputScreen;

            _movesCounterText.text = "Moves: " + _movesCounter.ToString();
            CardController.Instance.OnMoveHandler += ChangeMovesCounter;
            CardController.Instance.OnUnavailableMoveHandler += GameOver;
            _continueButton.onClick.AddListener(ContinueGame);
            _restartButton.onClick.AddListener(RestartGame);
            _undoButton.onClick.AddListener(CardController.Instance.Hand.Undo);
        }

        #region data
        public void Load()
        {
            if (CardController.Instance.DataService.CheckData(TIMER_KEY))
                CardController.Instance.DataService.Load<int>(TIMER_KEY, data =>
                {
                    _timer = data;
                });
        }

        public void Save()
        {
            CardController.Instance.DataService.Save(TIMER_KEY, _timer, data =>
            {
                Debug.Log("Timer saved");
            });
        }
        #endregion

        private async UniTaskVoid Timer(CancellationTokenSource cancelToken)
        {
            while (!cancelToken.IsCancellationRequested)
            {
                await UniTask.Delay(1000);
                _timer++;
                int minutes = Mathf.FloorToInt(_timer / 60f);
                int seconds = Mathf.FloorToInt(_timer % 60f);
                _timerText.text = $"{minutes:00}:{seconds:00}";
            }
        }

        private void SwitchDisableInputScreen(bool isActive)
        {
            _disableInputScreen.SetActive(isActive);

            SwitchTimer(!isActive);
        }

        public void SwitchTimer(bool isActive)
        {
            if (isActive)
            {
                _cancellationTimerToken = new CancellationTokenSource();

                Timer(_cancellationTimerToken).Forget();
            }
            else _cancellationTimerToken?.Cancel();
        }

        private void ChangeMovesCounter()
        {
            _movesCounter++;
            _movesCounterText.text = "Moves: " + _movesCounter.ToString();
        }

        private void ContinueGame()
        {
            SwitchTimer(true);
            _endGameScreen.gameObject.SetActive(false);
        }

        private void RestartGame()
        => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        private void GameOver(bool isWinGame)
        {
            SwitchTimer(false);

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
            CardController.Instance.OnLoadingDataHandler -= SwitchDisableInputScreen;
        }
    }
}