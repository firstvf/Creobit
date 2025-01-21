using UnityEngine;

namespace Assets.Src.Code.Controllers
{
    public class SoundController : MonoBehaviour
    {
        public static SoundController Instance { get; private set; }
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _cardDeal, _cardSet, _cardTurnAround;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                return;
            }

            Destroy(gameObject);
        }

        public void PlayCardDealSound() => _audioSource.PlayOneShot(_cardDeal);
        public void PlayCardSetSound() => _audioSource.PlayOneShot(_cardSet);
        public void PlayCardTurnAroundSound() => _audioSource.PlayOneShot(_cardTurnAround,0.5f);
    }
}