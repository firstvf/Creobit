using Assets.Src.Code.Controllers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Src.Code.Solitaire
{
    public class CardLogic : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        [SerializeField] private Card _card;
        private Vector2 _previousPosition;
        private bool _isCollideWithCardHolder;

        private void OnTriggerEnter2D(Collider2D collision)
        => _isCollideWithCardHolder = true;

        private void OnTriggerExit2D(Collider2D collision)
        => _isCollideWithCardHolder = false;

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (CheckCardPossibility()) return;

            transform.DOScale(1.5f, 0.25f)
                .OnStart(() => SoundController.Instance.PlayCardDealSound());
            _previousPosition = transform.position;
            _card.SpriteRenderer.sortingOrder = 200;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (CheckCardPossibility()) return;

            transform.position = eventData.pointerCurrentRaycast.worldPosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (CheckCardPossibility()) return;

            if (_isCollideWithCardHolder && CardController.Instance.CheckPossibilityToPlaceOnHand(_card.Identifier))
            {
                SetCardToHand();
                return;
            }

            // wrong spot
            SetCardToPosition(_previousPosition, _card.InitialSortingLayer);
        }

        private void SetCardToHand()
        {
            _card.SwitchCardSet(true);
            int sortingLayer = CardController.Instance.SetCardToHand(_card.Identifier);
            SetCardToPosition(CardController.Instance.GetCardHolderPosition(), sortingLayer);
            CardController.Instance.OnCardTurnHandler?.Invoke(_card.Id);
            CardController.Instance.OnMoveHandler?.Invoke();
        }

        private bool CheckCardPossibility()
        => _card.IsBackSide || _card.IsCardSet;

        private void SetCardToPosition(Vector2 position, int sortingOrder)
        {
            _card.SpriteRenderer.sortingOrder = sortingOrder;
            transform.DOScale(1f, 0.25f);
            transform.DOMove(position, 0.2f)
                .OnComplete(() => SoundController.Instance.PlayCardSetSound());
        }

        public void GetCardFromDeck()
        {
            _card.SwitchCardSide();
            SetCardToHand();
            _card.SpriteRenderer.sprite = _card.CardSprite;
        }
    }
}