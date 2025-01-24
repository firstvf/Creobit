using Assets.Src.Code.Data.Bundle;
using Assets.Src.Code.Pool;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Src.Code.Solitaire
{
    public class CardSpawner : MonoBehaviour
    {
        [SerializeField] private Transform _firstCardGroup, _secondCardGroup, _thirdCardGroup, _fourthCardGroup;
        [SerializeField] private Transform _cardDeck, _cardHand;
        [SerializeField] private Card _cardPrefab;
        [SerializeField] private CardBundle _bundle;
        private ObjectPooler<Card> _cardPooler;

        public readonly Dictionary<int, Card> CardDictionary = new();

        private void Start()
        {
            CreatePack();
            MixCardDeck();

            StartCoroutine(DealCards());
        }

        private void CreatePack()
        {
            _cardPooler = new(_cardPrefab, _cardDeck, 52);
            for (int i = 0; i < _cardPooler.GetList().Count; i++)
                _cardPooler.GetList()[i].InitCard(_bundle.CardData.SpriteArray[i]
                    , _bundle.CardData.IdentifierArray[i]);
        }

        private void MixCardDeck()
        {
            var list = _cardPooler.GetList().OrderBy((x) => Random.value).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                CardDictionary.Add(i, list[i]);
            }
        }

        private IEnumerator DealCards()
        {
            GroupCards(24, 150, 28);

            StartCoroutine(GroupCards(3, 160, _fourthCardGroup, false, 25));
            if (!CardController.Instance.IsRequireFastSet)
                yield return new WaitForSeconds(0.5f);
            else yield return null;
            StartCoroutine(GroupCards(6, 170, _thirdCardGroup, false, 19));
            if (!CardController.Instance.IsRequireFastSet)
                yield return new WaitForSeconds(1f);
            else yield return null;
            StartCoroutine(GroupCards(9, 180, _secondCardGroup, false, 10));
            if (!CardController.Instance.IsRequireFastSet)
                yield return new WaitForSeconds(1.8f);
            else yield return null;
            StartCoroutine(GroupCards(10, 190, _firstCardGroup, true, 0));

            yield return new WaitForSeconds(2f);

            for (int i = 0; i < CardDictionary.Count; i++)
            {
                CardDictionary[i].SetCardRayCast(true);
            }
        }

        private IEnumerator GroupCards(int count, int sortingLayer, Transform group, bool isRequireToTurnCard, int startingId)
        {
            for (int i = 0; i < count; i++)
            {
                var card = CardDictionary[startingId + i];
                card.gameObject.SetActive(true);
                card.SortCard(sortingLayer, isRequireToTurnCard, i + startingId, group);
                yield return new WaitForSeconds(0.15f);
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