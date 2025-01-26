using Assets.Src.Code.Data.JsonData;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Src.Code.Clicker.UI
{
    public class HamsterHUD : MonoBehaviour, IData
    {
        [SerializeField] private HamsterSettingsHUD _settingsHUD;
        [SerializeField] private Text _timerText, _clickCountText;
        [SerializeField] private Button _settings;
        private int _clickCounter;
        private int _timer;
        private CancellationTokenSource _cancellationTimerToken;
        private const string TIMER_KEY = "HamsterTimer";
        private const string CLICK_COUNTER_KEY = "ClickCounter";

        private void Start()
        {
            Load();

            ClickerController.Instance.OnClickHandler += UpdateClickCountText;
            _settings.onClick.AddListener(OpenSettingsHUD);
            SwitchTimer(true);

            _clickCountText.text = _clickCounter.ToString();
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

        #region data
        public void Load()
        {
            if (ClickerController.Instance.DataService.CheckData(TIMER_KEY))
                ClickerController.Instance.DataService.Load<int>(TIMER_KEY, data =>
                {
                    _timer = data;
                });

            if (ClickerController.Instance.DataService.CheckData(CLICK_COUNTER_KEY))
                ClickerController.Instance.DataService.Load<int>(CLICK_COUNTER_KEY, data =>
                {
                    _clickCounter = data;
                });
        }

        public void Save()
        {
            ClickerController.Instance.DataService.Save(TIMER_KEY, _timer);
            ClickerController.Instance.DataService.Save(CLICK_COUNTER_KEY, _clickCounter);
        }
        #endregion

        private void OpenSettingsHUD()
        {
            SwitchTimer(false);
            _settingsHUD.gameObject.SetActive(true);
        }

        private void UpdateClickCountText()
        {
            _clickCounter++;

            _clickCountText.text = _clickCounter.ToString();
        }

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

        private void OnDestroy()
        {
            Save();
            SwitchTimer(false);
            ClickerController.Instance.OnClickHandler -= UpdateClickCountText;
            _settings.onClick.RemoveListener(OpenSettingsHUD);
        }
    }
}