using System;
using System.Linq;
using UnityEngine;

namespace Assets.Src.Code.Solitaire
{
    public class CardController : MonoBehaviour
    {
        public static CardController Instance { get; private set; }
        public Action<int> OnCardTurnHandler { get; set; }
        [field: SerializeField] public Sprite CardBackSideSprite { get; private set; }
        [field: SerializeField] public bool IsRequireFastSet { get; private set; }
        [SerializeField] private Transform _cardHolder;
        [SerializeField] private CardSpawner _cardSpawner;

        private int _currentCardHolderIdentifier;
        private bool _isAnyCardPlace;
        private int _cardHolderSortingLayer;
        private float _cardHolderPositionFrequency;

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
            //OnCardTurnHandler += IncreaseCardHolderPositionFrequency;
            OnCardTurnHandler += CheckPossibilityToFlipCard;
            _isAnyCardPlace = false;
            _cardHolderSortingLayer = 0;
        }

        public Vector2 GetCardHolderPosition()
        {
            return new Vector2(
                _cardHolder.transform.position.x + _cardHolderPositionFrequency,
                _cardHolder.transform.position.y + _cardHolderPositionFrequency);
        }

        private void IncreaseCardHolderPositionFrequency()
        => _cardHolderPositionFrequency += 0.05f;

        public bool CheckCardHolder(int identifier)
        {
            if (_isAnyCardPlace == false) return true;

            if (identifier + 1 == _currentCardHolderIdentifier
               || identifier - 1 == _currentCardHolderIdentifier
               || identifier == 1 && _currentCardHolderIdentifier == 13
               || identifier == 13 && _currentCardHolderIdentifier == 1)
            {
                return true;
            }

            return false;
        }

        private void CheckPossibilityToFlipCard(int id)
        {
            _cardHolderPositionFrequency += 0.005f;

            if (id == 0)
                CheckCardId(0, 1, 10);
            else if (id == 9)
                CheckCardId(8, 9, 18);
            else if (id == 10)
                CheckCardId(10, 11, 19);
            else if (id == 18)
                CheckCardId(17, 18, 24);
            else if (id == 19 || id == 20)
                CheckCardId(19, 20, 25);
            else if (id == 21 || id == 22)
                CheckCardId(21, 22, 26);
            else if (id == 23 || id == 24)
                CheckCardId(23, 24, 27);
            else if (id < 10)
            {
                CheckCardId(id - 1, id, id + 9);
                CheckCardId(id, id + 1, id + 10);
            }
            else if (id < 19)
            {
                CheckCardId(id - 1, id, id + 7);
                CheckCardId(id, id + 1, id + 8);
            }
            else Debug.LogError("This card from the deck");
        }

        private void OpenCard(int id)
        => _cardSpawner.CardDictionary[id].TurnCard(true);

        private void CheckCardId(int firstId, int secondId, int openId)
        {
            if (_cardSpawner.CardDictionary[firstId].IsCardSet
            && _cardSpawner.CardDictionary[secondId].IsCardSet)
            {
                OpenCard(openId);
            }
        }

        public int SetToCardHolder(int identifier)
        {
            _cardHolderSortingLayer++;
            _isAnyCardPlace = true;
            _currentCardHolderIdentifier = identifier;
            return _cardHolderSortingLayer;
        }

        private void OnDestroy()
        {
            OnCardTurnHandler -= CheckPossibilityToFlipCard;
        }
    }
}