using System;
using UnityEngine;

namespace Assets.Src.Code.Solitaire
{
    public class CardController : MonoBehaviour
    {
        public static CardController Instance { get; private set; }
        public Action<int> OnCardTurnHandler { get; set; }
        public Action OnMoveHandler { get; set; }
        public Action<bool> OnUnavailableMoveHandler { get; set; }
        [field: SerializeField] public Sprite CardBackSideSprite { get; private set; }
        [field: SerializeField] public bool IsRequireFastSet { get; private set; }
        [field: SerializeField] public CardSpawner CardSpawner { get; private set; }
        [field: SerializeField] public Hand Hand { get; private set; }
        [SerializeField] private CardDeck _cardDeck;

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
            Hand.OnUndoCardFromHandHandler += UndoAction;
            Hand.OnUndoCardFromHandHandler += CheckPossibilityToFlipCard;
        }

        public bool CheckPossibilityToPlaceOnHand(int identifier)
        {
            if (Hand.CheckIsAnyCardPlaced() == false) return true;

            if (identifier + 1 == GetIdentifier()
               || identifier - 1 == GetIdentifier()
               || identifier == 1 && GetIdentifier() == 13
               || identifier == 13 && GetIdentifier() == 1)
            {
                return true;
            }

            return false;
        }

        private int GetIdentifier()
        => CardSpawner.CardDictionary[Hand.GetLastCardId()].Identifier;

        private void UndoAction(int id)
        => CardSpawner.CardDictionary[id].UndoAction();

        private void FindAnyAvailableMove()
        {
            Debug.Log("FIND ANY AVAILABLE MOVE");

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
                    if (CardSpawner.CardDictionary[i].IsCardSet == false
                        && CardSpawner.CardDictionary[i].IsDeckCard == false)
                    {
                        OnUnavailableMoveHandler?.Invoke(false);
                        return;
                    }

                OnUnavailableMoveHandler?.Invoke(true);
            }
        }

        private bool CheckIsAnyCardOnDesk()
        {
            for (int i = 0; i < CardSpawner.CardDictionary.Count; i++)
                if (CardSpawner.CardDictionary[i].IsCardSet == false
                    && CardSpawner.CardDictionary[i].IsDeckCard == false)
                {
                    return false;
                }

            return true;
        }

        private void CheckPossibilityToFlipCard(int id)
        {
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



            if (_cardDeck.IsCardDeckEmpty || (CheckIsAnyCardOnDesk() && _cardDeck.IsCardDeckEmpty == false))
            {
                FindAnyAvailableMove();
            }
        }

        private void TryOpenCard(int firstId, int secondId, int openId)
        {
            if (CardSpawner.CardDictionary[firstId].IsCardSet
            && CardSpawner.CardDictionary[secondId].IsCardSet)
            {
                CardSpawner.CardDictionary[openId].TurnCard(isRequireToTween: true);
            }
            else
            {
                CardSpawner.CardDictionary[openId].TurnCard(isUndoAction: true);
            }
        }

        private void OnDestroy()
        {
            OnCardTurnHandler -= CheckPossibilityToFlipCard;
            Hand.OnUndoCardFromHandHandler -= UndoAction;
            Hand.OnUndoCardFromHandHandler -= CheckPossibilityToFlipCard;
        }
    }
}