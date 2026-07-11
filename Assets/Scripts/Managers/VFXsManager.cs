using UnityEngine;

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
            fogEffect.Play();
        }
    }
}

