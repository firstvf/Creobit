using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Src.Code.Clicker
{
    public class HamsterAction : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        public void CreateAction(Vector2 position, Sprite sprite)
        {
            transform.position = position;
            _spriteRenderer.sprite = sprite;

            _rigidbody.AddForce(
                new Vector2(Random.Range(1, 3), Random.Range(1, 3))
                , ForceMode2D.Impulse);

            DisableAction().Forget();
        }

        private async UniTaskVoid DisableAction()
        {
            await UniTask.Delay(3000);
            gameObject.SetActive(false);
        }
    }
}