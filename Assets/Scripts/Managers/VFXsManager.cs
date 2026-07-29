using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class VFXsManager : MonoBehaviour
    {
        [Header("VFXs")]
        [SerializeField] private ParticleSystem twinkleEffect;
        [SerializeField] private ParticleSystem fogEffect;
        
        private void Start()
        {
            twinkleEffect.Play();
            ManageFogEffectBasedOnScene();
        }

        private void OnEnable()
        {
            ManageFogEffectBasedOnScene();
        }

        private void ManageFogEffectBasedOnScene()
        {
            if (SceneManager.GetActiveScene().buildIndex == 0) fogEffect.Play();
            else fogEffect.Stop();
        }
    }
}

