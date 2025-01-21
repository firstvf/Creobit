using System;
using UnityEngine;

namespace Assets.Src.Code.Data.Bundle
{
    [Serializable]
    public class CardData
    {
        [field: SerializeField] public Sprite[] SpriteArray { get; private set; }
        [field: SerializeField] public int[] IdentifierArray { get; private set; }        
    }
}