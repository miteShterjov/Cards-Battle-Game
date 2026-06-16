using Cards;
using Events;
using UnityEngine;

namespace Managers
{
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] private AudioClip playCardSfx;
        [SerializeField] private AudioClip drawCardSfx;
        [SerializeField] private AudioClip reshuffleCardSfx;
        [SerializeField] private AudioClip playerHitSfx;
        [SerializeField] private AudioClip playerHealsSfx;
        [SerializeField] private AudioClip playerDeathSfx;
        [SerializeField] private AudioClip enemyHitSfx;
        [SerializeField] private AudioClip enemyDeadSfx;
        
        private AudioSource _audioSource;

        protected override void Awake()
        {
            base.Awake();
            _audioSource = gameObject.GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            PlayerEvents.OnCardPlayed += CardPlayed;
            PlayerEvents.OnDrawCardRequested += DrawCardRequested;
            PlayerEvents.OnReshuffleRequested += ReshuffleRequested;
            PlayerEvents.OnPlayerHit += PayerHit;
            PlayerEvents.OnPlayerHealed += PlayerHeals;
            PlayerEvents.OnPlayerDeath += PlayerDies;
            EnemyEvents.OnEnemyHit += EnemyHit;
            EnemyEvents.OnEnemyDeath += EnemyDeath;
        }

        private void OnDisable()
        {
            PlayerEvents.OnCardPlayed -= CardPlayed;
            PlayerEvents.OnDrawCardRequested -= DrawCardRequested;
            PlayerEvents.OnReshuffleRequested -= ReshuffleRequested;
            PlayerEvents.OnPlayerHit -= PayerHit;
            PlayerEvents.OnPlayerHealed -= PlayerHeals;
            PlayerEvents.OnPlayerDeath -= PlayerDies;
            EnemyEvents.OnEnemyHit -= EnemyHit;
            EnemyEvents.OnEnemyDeath -= EnemyDeath;
        }
        
        private void CardPlayed(CardData _) => PLaySfx(playCardSfx);
        private void DrawCardRequested() => PLaySfx(drawCardSfx);
        private void ReshuffleRequested() => PLaySfx(reshuffleCardSfx);
        private void PayerHit(int _) => PLaySfx(playerHitSfx);
        private void PlayerHeals() => PLaySfx(playerHealsSfx);
        private void PlayerDies() => PLaySfx(playerDeathSfx);
        private void EnemyHit(int _) => PLaySfx(enemyHitSfx);
        private void EnemyDeath() => PLaySfx(enemyDeadSfx);

        private void PLaySfx(AudioClip audioClip)
        {
            if (audioClip) _audioSource.PlayOneShot(audioClip);
        }
    }
}