using Assets.Src.Code.Controllers;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Src.Code.Clicker
{
    public class Coin : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody;        

        public void SpawnCoin(Vector2 position)
        {
            transform.position = position;
            SoundController.Instance.PlayCoinSound();

            _rigidbody.AddForce(
                new Vector2(Random.Range(0, 2), Random.Range(0, 2))
                , ForceMode2D.Impulse);

            DisableCoin().Forget();
        }

        private async UniTaskVoid DisableCoin()
        {
            await UniTask.Delay(2000);
            gameObject.SetActive(false);
        }
    }
}