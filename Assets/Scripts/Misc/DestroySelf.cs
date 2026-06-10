using UnityEngine;

namespace Misc
{
    public class DestroySelf : MonoBehaviour
    {
        [Header("Destroy Self Config")] 
        [SerializeField] private float delay;
        
        public void DestroyThis() => Destroy(gameObject, delay);
    }
}
