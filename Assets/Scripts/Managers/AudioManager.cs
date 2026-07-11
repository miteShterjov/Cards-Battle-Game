using Cards;
using Events;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class AudioManager : Singleton<AudioManager>
    {
        [Header("SFX")]
        [SerializeField] private AudioClip playCardSfx;
        [SerializeField] private AudioClip drawCardSfx;
        [SerializeField] private AudioClip reshuffleCardSfx;
        [SerializeField] private AudioClip playerHitSfx;
        [SerializeField] private AudioClip playerHealsSfx;
        [SerializeField] private AudioClip playerDeathSfx;
        [SerializeField] private AudioClip enemyHitSfx;
        [SerializeField] private AudioClip enemyDeadSfx;

        [Header("Music")]
        [SerializeField] private AudioClip ambientMusic;
        [SerializeField] private AudioSource musicSource; 
        [SerializeField] private string battleSceneName = "GameSceneScene"; 

        private AudioSource _audioSource;

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject); 
            _audioSource = gameObject.GetComponent<AudioSource>();
        }

        private void Start()
        {
            HandleMusicForScene(SceneManager.GetActiveScene().name);
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
            EnemyEvents.OnEnemyHeal += EnemyHeals;

            SceneManager.activeSceneChanged += OnSceneChanged;
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
            EnemyEvents.OnEnemyHeal -= EnemyHeals;
            
            SceneManager.activeSceneChanged -= OnSceneChanged;
        }

        private void OnSceneChanged(Scene oldScene, Scene newScene)
        {
            HandleMusicForScene(newScene.name);
        }

        private void HandleMusicForScene(string sceneName)
        {
            Debug.Log($"HandleMusicForScene called with: {sceneName}, battleSceneName: {battleSceneName}");
            bool isBattleScene = sceneName == battleSceneName;
            Debug.Log($"isBattleScene: {isBattleScene}");

            if (isBattleScene)
            {
                musicSource.Stop();
            }
            else
            {
                Debug.Log($"musicSource.isPlaying: {musicSource.isPlaying}, ambientMusic assigned: {ambientMusic != null}, musicSource assigned: {musicSource != null}");
                if (!musicSource.isPlaying)
                {
                    musicSource.clip = ambientMusic;
                    musicSource.loop = true;
                    musicSource.Play();
                    musicSource.volume = 0.1f;
                }
            }
        }

        private void CardPlayed(CardData _) => PLaySfx(playCardSfx);
        private void DrawCardRequested() => PLaySfx(drawCardSfx);
        private void ReshuffleRequested() => PLaySfx(reshuffleCardSfx);
        private void PayerHit(int _, DamageType damageType) => PLaySfx(playerHitSfx);
        private void PlayerHeals() => PLaySfx(playerHealsSfx);
        private void PlayerDies() => PLaySfx(playerDeathSfx);
        private void EnemyHit(int _) => PLaySfx(enemyHitSfx);
        private void EnemyDeath() => PLaySfx(enemyDeadSfx);
        private void EnemyHeals() => PLaySfx(playerHealsSfx);

        private void PLaySfx(AudioClip audioClip)
        {
            if (audioClip) _audioSource.PlayOneShot(audioClip);
        }
    }
}