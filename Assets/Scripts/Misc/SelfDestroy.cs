using System;
using UnityEngine;

namespace Misc
{
    public class SelfDestroy : MonoBehaviour
    {
        [SerializeField] private float delay = 0.2f;
        private void Start() => Destroy(gameObject, delay);
    }
}