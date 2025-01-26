using Assets.Src.Code.Controllers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Src.Code.Clicker
{
    public class Hamster : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            ClickAction(eventData.pointerCurrentRaycast.worldPosition);
        }

        private void ClickAction(Vector2 position)
        {
            transform.DOPunchScale(new Vector2(0.25f, 0.25f), 0.1f, 1, 1)
                .OnStart(() => SoundController.Instance.PlayPunchSound());

            ClickerController.Instance.OnClickEventDataHandler?.Invoke(position);
            ClickerController.Instance.OnClickHandler?.Invoke();
        }
    }
}