using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Src.Code.Solitaire
{
    public class CardDeck : MonoBehaviour, IPointerClickHandler
    {
        public bool IsCardDeckEmpty { get; private set; }
        private int _topCardId = 51; // top id card deck

        public void GetCard()
        {
            if (_topCardId <= 27) // bot id card deck
                return;

            if (_topCardId <= 28)
                IsCardDeckEmpty = true;

            CardController.Instance.CardSpawner.CardDictionary[_topCardId].CardLogic.GetCardFromDeck();
            _topCardId--;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            GetCard();
        }
    }
}