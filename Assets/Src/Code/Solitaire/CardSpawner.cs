using Assets.Src.Code.Data.Bundle;
using Assets.Src.Code.Data.JsonData;
using Assets.Src.Code.Pool;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Src.Code.Solitaire
{
    public class CardSpawner : MonoBehaviour, IData
    {
        public readonly Dictionary<int, Card> CardDictionary = new();

        [SerializeField] private Transform _firstCardGroup, _secondCardGroup, _thirdCardGroup, _fourthCardGroup;
        [SerializeField] private Transform _cardDeck, _cardHand;
        [SerializeField] private Card _cardPrefab;
        [SerializeField] private CardBundle _bundle;
        private ObjectPooler<Card> _cardPooler;
        private bool _isInitialize;
        private const string CARD_DATA_KEY = "CardData";

        public void StartGame()
        {
            CreatePack();
        }

        #region data
        public void Load()
        {
            if (CardController.Instance.DataService.CheckData(CARD_DATA_KEY))
            {
                CardController.Instance.DataService.Load<Dictionary<int, int>>(CARD_DATA_KEY, data =>
                {
                    CreatePack(data);
                });
            }
            else
            {
                CreatePack();
            }
        }

        public void Save()
        {
            if (_isInitialize)
            {
                Dictionary<int, int> cardDictionary = new();

                for (int i = 0; i < CardDictionary.Count; i++)
                    cardDictionary.Add(i, CardDictionary[i].SpriteDataIdentifier);

                CardController.Instance.DataService.Save(CARD_DATA_KEY, cardDictionary, data =>
                {
                    Debug.Log("Card dictionary saved");
                });
            }
        }
        #endregion

        private void CreatePack(Dictionary<int, int> data = null)
        {
            _cardPooler = new(_cardPrefab, _cardDeck, 52);

            if (data == null)
            {
                for (int i = 0; i < _cardPooler.PoolList.Count; i++)
                    _cardPooler.PoolList[i].InitCard(_bundle.CardData.SpriteArray[i]
                        , _bundle.CardData.IdentifierArray[i], i);

                MixCardDeck();
            }
            else
            {
                for (int i = 0; i < _cardPooler.PoolList.Count; i++)
                    _cardPooler.PoolList[i].InitCard(_bundle.CardData.SpriteArray[data[i]]
                        , _bundle.CardData.IdentifierArray[data[i]], data[i]);

                for (int i = 0; i < _cardPooler.PoolList.Count; i++)
                    CardDictionary.Add(i, _cardPooler.PoolList[i]);

                DealCards().Forget();
            }
        }

        private void MixCardDeck()
        {
            var list = _cardPooler.PoolList.OrderBy((x) => Random.value).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                CardDictionary.Add(i, list[i]);
            }

            DealCards().Forget();
        }

        private async UniTaskVoid DealCards()
        {
            _isInitialize = true;

            GroupCards(24, 150, 28);

            GroupCards(3, 160, _fourthCardGroup, false, 25).Forget();
            await UniTask.Delay(500);
            GroupCards(6, 170, _thirdCardGroup, false, 19).Forget();
            await UniTask.Delay(800);

            GroupCards(9, 180, _secondCardGroup, false, 10).Forget();
            await UniTask.Delay(1400);

            GroupCards(10, 190, _firstCardGroup, true, 0).Forget();

            await UniTask.Delay(2000);

            for (int i = 0; i < CardDictionary.Count; i++)
                CardDictionary[i].SetCardRayCast(true);

            CardController.Instance.OnLoadingDataHandler?.Invoke(true);
        }

        private async UniTaskVoid GroupCards(int count, int sortingLayer, Transform group, bool isRequireToTurnCard, int startingId)
        {
            for (int i = 0; i < count; i++)
            {
                var card = CardDictionary[startingId + i];
                card.gameObject.SetActive(true);
                card.SortCard(sortingLayer, isRequireToTurnCard, i + startingId, group);
                await UniTask.Delay(150);
            }
        }

        private void GroupCards(int count, int sortingLayer, int startingId)
        {
            for (int i = 0; i < count; i++)
            {
                var card = CardDictionary[startingId + i];
                card.gameObject.SetActive(true);
                card.SortCard(sortingLayer, isRequireToTurnCard: false, i + startingId, isDeckCard: true);
            }
        }
    }
}