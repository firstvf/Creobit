using Assets.Src.Code.Controllers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Src.Code.Solitaire
{
    public class CardLogic : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        [SerializeField] private Card _card;
        private bool _isCollideWithCardHolder;

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (CheckCardPossibility()) return;

            transform.DOScale(1.5f, 0.25f)
                .OnStart(() => SoundController.Instance.PlayCardDealSound());
            _card.SetStartPosition(transform.position);
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
            SetCardToPosition(_card.StartPosition, _card.InitialSortingLayer);
        }

        public void GetCardFromDeck()
        {
            SetCardToHand();
            _card.SwitchCardSide();
            _card.SpriteRenderer.sprite = _card.CardSprite;
        }

        public void LoadCardLogic()
        {
            _card.SetStartPosition(transform.position);
            SetCardToHand();
        }

        private void SetCardToHand()
        {
            _card.SwitchCardSet(true);
            int sortingLayer = CardController.Instance.Hand.GetSortingLayer();

            SetCardToPosition(CardController.Instance.Hand.GetHandPosition(), sortingLayer);
            CardController.Instance.OnMoveHandler?.Invoke();

            CardController.Instance.OnCardTurnHandler?.Invoke(_card.Id);
        }

        private bool CheckCardPossibility()
        => _card.IsBackSide || _card.IsCardSet;

        private void SetCardToPosition(Vector2 position, int sortingOrder)
        {
            _card.SpriteRenderer.sortingOrder = sortingOrder;
            transform.DOScale(1f, 0.25f);
            transform.DOMove(position, 0.2f)
                .OnStart(() => _card.SetCardRayCast(false))
                .OnComplete(() => DoneMoveAction());
        }

        private void DoneMoveAction()
        {
            SoundController.Instance.PlayCardSetSound();
            _card.SetCardRayCast(true);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        => _isCollideWithCardHolder = true;

        private void OnTriggerExit2D(Collider2D collision)
        => _isCollideWithCardHolder = false;
    }
}