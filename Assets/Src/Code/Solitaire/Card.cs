using Assets.Src.Code.Controllers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Src.Code.Solitaire
{
    public class Card : MonoBehaviour
    {
        [field: SerializeField] public SpriteRenderer SpriteRenderer { get; private set; }
        [field: SerializeField] public CardLogic CardLogic { get; private set; }
        public int Identifier { get; private set; }
        public bool IsCardSet { get; private set; }
        public int InitialSortingLayer { get; private set; }
        public int Id { get; private set; }
        public bool IsBackSide { get; private set; }
        public Sprite CardSprite { get; private set; }
        public bool IsDeckCard { get; private set; }
        public Vector3 StartPosition { get; private set; }
        [SerializeField] private Image _image;

        public void SwitchCardSet(bool isSet) => IsCardSet = isSet;
        public void SwitchCardSide() => IsBackSide = false;
        public void SetCardRayCast(bool isAble) => _image.raycastTarget = isAble;
        public void SetStartPosition(Vector3 position) => StartPosition = position;

        public void InitCard(Sprite sprite, int identifier)
        {
            SetCardRayCast(false);
            IsBackSide = true;
            SpriteRenderer.sprite = CardController.Instance.CardBackSideSprite;
            CardSprite = sprite;
            Identifier = identifier;
        }

        public void SortCard(int initialSortingLayer, bool isRequireToTurnCard, int id, Transform parent = null, bool isDeckCard = false)
        {
            IsDeckCard = isDeckCard;
            Id = id;
            InitialSortingLayer = initialSortingLayer;
            if (parent != null)
                transform.DOMove(parent.transform.position, 0.1f)
                    .OnComplete(() => SortCardAction(isRequireToTurnCard, parent));
        }

        public void UndoAction()
        {
            IsCardSet = false;
            SpriteRenderer.sortingOrder = InitialSortingLayer;

            if (IsDeckCard)
            {
                IsBackSide = true;
                SpriteRenderer.sprite = CardController.Instance.CardBackSideSprite;

                transform.DOLocalMove(StartPosition, 0.15f)
                    .OnStart(() => SetCardRayCast(false))
                    .OnComplete(() => SetCardRayCast(true));
            }
            else
            {
                transform.DOMove(StartPosition, 0.15f)
                    .OnStart(() => SetCardRayCast(false))
                    .OnComplete(() => SetCardRayCast(true));
            }

            CardController.Instance.OnMoveHandler?.Invoke();
        }

        public void TurnCard(bool isRequireToTween = false, bool isUndoAction = false)
        {
            if (isUndoAction || IsBackSide == false)
            {
                IsBackSide = true;
                SpriteRenderer.sprite = CardController.Instance.CardBackSideSprite;
            }
            else if (IsBackSide == true)
            {
                if (!isRequireToTween)
                    TurnAroundCardAction();
                else transform.DOPunchScale(Vector2.up, 0.1f, 5, 0.5f)
                        .OnStart(() => SoundController.Instance.PlayCardTurnAroundSound())
                        .OnComplete(() => TurnAroundCardAction());
            }
        }

        private void SortCardAction(bool isRequireToTurnCard, Transform parent)
        {
            SoundController.Instance.PlayCardSetSound();
            transform.SetParent(parent);
            if (isRequireToTurnCard)
                TurnCard();
            StartPosition = transform.position;
        }

        private void TurnAroundCardAction()
        {
            IsBackSide = false;
            SpriteRenderer.sprite = CardSprite;
        }
    }
}