using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Src.Code.Solitaire
{
    public class CardDeck : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private CardSpawner _cardSpawner;
        private int _topCardId = 51;

        public void GetCard()
        {
            if (_topCardId == 27) return;
            _cardSpawner.CardDictionary[_topCardId].CardLogic.GetCardFromDeck();
            _topCardId--;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            GetCard();
        }
    }
}