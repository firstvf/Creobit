using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Src.Code.Solitaire
{
    public class CardDeck : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private CardSpawner _cardSpawner;
        private int _topCardId = 51; // top id card deck

        public void GetCard()
        {
            if (_topCardId == 27) return; // bot id card deck
            _cardSpawner.CardDictionary[_topCardId].CardLogic.GetCardFromDeck();
            _topCardId--;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            GetCard();
        }
    }
}