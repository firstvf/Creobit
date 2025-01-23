using System;
using UnityEngine;

namespace Assets.Src.Code.Solitaire
{
    public class CardController : MonoBehaviour
    {
        public static CardController Instance { get; private set; }
        public bool IsAnyCardPlaced { get; private set; }
        public Action<int> OnCardTurnHandler { get; set; }
        public Action OnMoveHandler { get; set; }
        public Action<bool> OnUnavailableMoveHandler { get; set; }
        [field: SerializeField] public Sprite CardBackSideSprite { get; private set; }
        [field: SerializeField] public bool IsRequireFastSet { get; private set; }
        [field: SerializeField] public CardSpawner CardSpawner { get; private set; }

        [SerializeField] private Transform _hand;
        [SerializeField] private CardDeck _cardDeck;
        private int _currentHandIdentifier;
        private int _handSortingLayer;
        private float _handPositionFrequency;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                return;
            }

            Destroy(gameObject);
        }

        private void Start()
        {
            OnCardTurnHandler += CheckPossibilityToFlipCard;
            OnMoveHandler += IncreaseCardHolderPositionFrequency;
            IsAnyCardPlaced = false;
            _handSortingLayer = 0;
        }

        public Vector2 GetCardHolderPosition()
        {
            return new Vector2(
                _hand.transform.position.x + _handPositionFrequency,
                _hand.transform.position.y + _handPositionFrequency);
        }

        public bool CheckPossibilityToPlaceOnHand(int identifier)
        {
            if (IsAnyCardPlaced == false) return true;

            if (identifier + 1 == _currentHandIdentifier
               || identifier - 1 == _currentHandIdentifier
               || identifier == 1 && _currentHandIdentifier == 13
               || identifier == 13 && _currentHandIdentifier == 1)
            {
                return true;
            }

            return false;
        }

        public void FindAnyAvailableMove()
        {
            bool isAnyMoveAvailable = false;

            for (int i = 0; i < CardSpawner.CardDictionary.Count; i++)
            {
                if (CardSpawner.CardDictionary[i].IsCardSet == false
                    && CardSpawner.CardDictionary[i].IsBackSide == false
                    && CheckPossibilityToPlaceOnHand(CardSpawner.CardDictionary[i].Identifier))
                {
                    isAnyMoveAvailable = true;
                    break;
                }
            }


            if (isAnyMoveAvailable == false)
            {
                for (int i = 0; i < CardSpawner.CardDictionary.Count; i++)
                    if (CardSpawner.CardDictionary[i].IsCardSet
                        || CardSpawner.CardDictionary[i].IsDeckCard)
                    {
                        OnUnavailableMoveHandler?.Invoke(false);
                        return;
                    }

                OnUnavailableMoveHandler?.Invoke(true);
            }
        }

        public int SetCardToHand(int identifier)
        {
            _handSortingLayer++;
            IsAnyCardPlaced = true;
            _currentHandIdentifier = identifier;
            return _handSortingLayer;
        }

        private void CheckPossibilityToFlipCard(int id)
        {
            if (_cardDeck.IsCardDeckEmpty)
                FindAnyAvailableMove();

            if (id == 0) // first
                TryOpenCard(0, 1, 10);
            else if (id == 9) // first
                TryOpenCard(8, 9, 18);
            else if (id == 19 || id == 20) // fourth
                TryOpenCard(19, 20, 25);
            else if (id == 21 || id == 22) // fourth
                TryOpenCard(21, 22, 26);
            else if (id == 23 || id == 24) // fourth
                TryOpenCard(23, 24, 27);
            else if (id < 10)
            {
                TryOpenCard(id - 1, id, id + 9);
                TryOpenCard(id, id + 1, id + 10);
            }
            else if (id < 19)
            {
                if (id == 10)
                {
                    TryOpenCard(10, 11, 19);
                }
                else if (id == 11)
                {
                    TryOpenCard(10, 11, 19);
                    TryOpenCard(11, 12, 20);
                }
                else if (id == 12)
                {
                    TryOpenCard(11, 12, 20);
                }
                else if (id == 13)
                {
                    TryOpenCard(13, 14, 21);
                }
                else if (id == 14)
                {
                    TryOpenCard(13, 14, 21);
                    TryOpenCard(14, 15, 22);
                }
                else if (id == 15)
                {
                    TryOpenCard(14, 15, 22);
                }
                else if (id == 16)
                {
                    TryOpenCard(16, 17, 23);
                }
                else if (id == 17)
                {
                    TryOpenCard(16, 17, 23);
                    TryOpenCard(17, 18, 24);
                }
                else if (id == 18)
                {
                    TryOpenCard(17, 18, 24);
                }
            }
        }

        private void TryOpenCard(int firstId, int secondId, int openId)
        {
            if (CardSpawner.CardDictionary[firstId].IsCardSet
            && CardSpawner.CardDictionary[secondId].IsCardSet)
            {
                CardSpawner.CardDictionary[openId].TurnCard(true);
            }
        }

        private void IncreaseCardHolderPositionFrequency()
        => _handPositionFrequency += 0.005f;

        private void OnDestroy()
        {
            OnCardTurnHandler -= CheckPossibilityToFlipCard;
            OnMoveHandler -= IncreaseCardHolderPositionFrequency;
        }
    }
}