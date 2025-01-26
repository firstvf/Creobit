using Assets.Src.Code.Pool;
using UnityEngine;

namespace Assets.Src.Code.Clicker
{
    public class HamsterActionsSpawner : MonoBehaviour
    {
        [SerializeField] private Sprite[] _emotions;
        [SerializeField] private Transform _parent;
        [SerializeField] private Coin _coin;
        [SerializeField] private HamsterAction _hamsterAction;
        private ObjectPooler<Coin> _coinPooler;
        private ObjectPooler<HamsterAction> _hamsterActionsPooler;

        private void Start()
        {
            _coinPooler = new(_coin, _parent, 10);
            _hamsterActionsPooler = new(_hamsterAction, _parent, 15);

            ClickerController.Instance.OnClickEventDataHandler += SpawnCoin;
            ClickerController.Instance.OnClickEventDataHandler += SpawnActions;
        }

        private void SpawnCoin(Vector2 position)
        {
            if (Random.Range(0, 2) == 0)
                _coinPooler.GetFreeObjectFromPool().SpawnCoin(position);
        }

        private void SpawnActions(Vector2 position)
        {
            if (Random.Range(0, 4) == 0)
                _hamsterActionsPooler.GetFreeObjectFromPool()
                .CreateAction(position, _emotions[Random.Range(0, _emotions.Length)]);
        }

        private void OnDestroy()
        {
            ClickerController.Instance.OnClickEventDataHandler -= SpawnCoin;
            ClickerController.Instance.OnClickEventDataHandler -= SpawnActions;
        }
    }
}