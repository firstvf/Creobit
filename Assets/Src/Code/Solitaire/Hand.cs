using Assets.Src.Code.Data.JsonData;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Src.Code.Solitaire
{
    public class Hand : MonoBehaviour, IData
    {
        public Action<int> OnUndoCardFromHandHandler { get; set; }

        private readonly List<int> _handList = new();
        private int _handSortingLayer;
        private float _positionFrequency;
        private bool _isUndoReady;
        private List<int> _handMoves;
        private const string HAND_MOVES_KEY = "HandMoves";

        private void Start()
        {
            CardController.Instance.OnCardTurnHandler += AddCardToHandList;
            CardController.Instance.OnLoadingDataHandler += CheckData;
        }

        #region data
        public void Load()
        {
            if (CardController.Instance.DataService.CheckData(HAND_MOVES_KEY))
            {
                CardController.Instance.DataService.Load<List<int>>(HAND_MOVES_KEY, data =>
                {
                    _handMoves = data;
                });
            }
        }

        public void Save()
        {
            CardController.Instance.DataService.Save(HAND_MOVES_KEY, _handList, data =>
            {
                Debug.Log("HandMoves saved");
            });
        }
        #endregion

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

        private void CheckData(bool isLoaded)
        {
            CardController.Instance.OnLoadingDataHandler -= CheckData;

            LoadDataTask(isLoaded).Forget();
        }

        private async UniTaskVoid LoadDataTask(bool isLoaded)
        {
            await UniTask.Delay(50);

            if (_handMoves == null || _handMoves.Count <= 0)
            {
                CardController.Instance.OnLoadingDataHandler?.Invoke(false);
                return;
            }
            else if (isLoaded)
            {
                for (int i = 0; i < _handMoves.Count; i++)
                {
                    if (CardController.Instance.CardSpawner.CardDictionary[_handMoves[i]].IsDeckCard)
                    {
                        CardController.Instance.CardDeck.GetCard();
                    }
                    else
                    {
                        CardController.Instance.CardSpawner.CardDictionary[_handMoves[i]].CardLogic.LoadCardLogic();
                    }

                    await UniTask.Delay(100);
                }

                CardController.Instance.OnLoadingDataHandler?.Invoke(false);
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