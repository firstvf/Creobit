using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Src.Code.Solitaire
{
    public class Hand : MonoBehaviour
    {
        public Action<int> OnUndoCardFromHandHandler { get; set; }

        private readonly List<int> _handList = new();
        private int _handSortingLayer;
        private float _positionFrequency;
        private bool _isUndoReady;

        private void Start()
        {
            CardController.Instance.OnCardTurnHandler += AddCardToHandList;
        }

        public bool CheckIsAnyCardPlaced() => _handList.Count > 0;
        public int GetLastCardId() => _handList[^1];

        public int GetSortingLayer()
        {
            _handSortingLayer++;
            return _handSortingLayer;
        }

        public Vector2 GetHandPosition()
        {
            _positionFrequency += 0.005f;

            return new Vector2(
                transform.position.x + _positionFrequency,
                transform.position.y + _positionFrequency);
        }

        public void Undo()
        {
            if (CheckIsAnyCardPlaced() && _isUndoReady == false)
            {
                _isUndoReady = true;
                UndoCooldownCoroutine().Forget();
                _positionFrequency -= 0.005f;
                _handSortingLayer--;

                OnUndoCardFromHandHandler?.Invoke(GetLastCardId());
                _handList.Remove(GetLastCardId());
            }
        }

        private async UniTaskVoid UndoCooldownCoroutine()
        {
            await UniTask.Delay(250);
            _isUndoReady = false;
        }

        private void AddCardToHandList(int cardId)
        => _handList.Add(cardId);

        private void OnDestroy()
        {
            CardController.Instance.OnCardTurnHandler -= AddCardToHandList;
        }
    }
}