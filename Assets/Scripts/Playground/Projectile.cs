using UnityEngine;

namespace Game
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] int damage = 1;
        [SerializeField] float speed = 5f;

        void Update()
        {
            transform.position += speed * transform.forward * Time.deltaTime;
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.TryGetComponent<Health>(out Health health))
            {
                health.TakeDamage(damage);
                Pooler.Despawn(gameObject);
            }
        }
    }
}
