using Assets.Src.Code.Data.JsonData;
using System;
using UnityEngine;

namespace Assets.Src.Code.Clicker
{
    public class ClickerController : MonoBehaviour
    {
        public static ClickerController Instance { get; private set; }
        public readonly IDataService DataService = new JsonToFileService();
        public Action<Vector2> OnClickEventDataHandler { get; set; }
        public Action OnClickHandler { get; set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                return;
            }

            Destroy(gameObject);
        }
    }
}