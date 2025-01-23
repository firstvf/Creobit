using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Src.Code.Solitaire
{
    public class Hand : MonoBehaviour
    {
        [SerializeField] private Button _undoButton;

        private readonly List<int> _handList = new();

        private void Start()
        {
            CardController.Instance.OnCardTurnHandler += AddCardToHandList;
            _undoButton.onClick.AddListener(Undo);
        }

        private void AddCardToHandList(int cardId)
        {
            Debug.Log("add card");
            _handList.Add(cardId);
        }

        private void Undo()
        {
            if (CardController.Instance.IsAnyCardPlaced)
            {
                int lastId = _handList.Count - 1;
                CardController.Instance.CardSpawner
                    .CardDictionary[_handList[lastId]].UndoAction();
                _handList.Remove(lastId);
            }
            else
            {
                Debug.Log(_handList.Count);
                Debug.Log("undo unavailable");
            }
        }

        private void OnDestroy()
        {
            CardController.Instance.OnCardTurnHandler -= AddCardToHandList;
            _undoButton.onClick.RemoveListener(Undo);
        }
    }
}