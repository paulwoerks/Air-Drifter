using UnityEngine;

namespace Game
{
    public class Asteroid : MonoBehaviour
    {
        Health health;

        private void Awake()
        {
            health = GetComponent<Health>();
        }

        public void OnSpawn()
        {
            health.SetHealth();
        }

        public void Despawn()
        {
            Pooler.Despawn(gameObject);
        }
    }
}
