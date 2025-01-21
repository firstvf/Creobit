using Assets.Src.Code.Controllers;
using DG.Tweening;
using UnityEngine;

namespace Assets.Src.Code.Solitaire
{
    public class Card : MonoBehaviour
    {
        [field: SerializeField] public SpriteRenderer SpriteRenderer { get; private set; }
        public int Identifier { get; private set; }
        public bool IsCardSet { get; private set; }
        public int InitialSortingLayer { get; private set; }
        public int Id { get; private set; }
        public bool IsBackSide { get; private set; }
        public Sprite CardSprite { get; private set; }
        public bool IsDeckCard { get; private set; }

        public void SwitchCardSet(bool isSet) => IsCardSet = isSet;
        public void SwitchCardSide() => IsBackSide = IsDeckCard = false;

        public void InitCard(Sprite sprite, int identifier)
        {
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

        public void TurnCard(bool isRequireToTween = false)
        {
            if (IsBackSide == true)
            {
                if (!isRequireToTween)
                    TurnAroundCardAction();
                else transform.DOPunchScale(Vector2.up, 0.1f, 5, 0.5f)
                        .OnStart(() => SoundController.Instance.PlayCardTurnAroundSound())
                        .OnComplete(() => TurnAroundCardAction());
            }
            else
            {
                IsBackSide = true;
                SpriteRenderer.sprite = CardController.Instance.CardBackSideSprite;
            }
        }

        private void SortCardAction(bool isRequireToTurnCard, Transform parent)
        {
            SoundController.Instance.PlayCardSetSound();
            transform.SetParent(parent);
            if (isRequireToTurnCard)
                TurnCard();
        }

        private void TurnAroundCardAction()
        {
            IsBackSide = false;
            SpriteRenderer.sprite = CardSprite;
        }
    }
}