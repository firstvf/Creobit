using UnityEngine;

namespace Assets.Src.Code.Data.Bundle
{
    [CreateAssetMenu(fileName = "new Card bundle", menuName = "CardsData")]
    public class CardBundle : ScriptableObject
    {
        [field: SerializeField]
        public CardData CardData { get; private set; }
    }
}