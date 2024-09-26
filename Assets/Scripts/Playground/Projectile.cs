using UnityEngine;

namespace Game
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] int damage = 1;
        [SerializeField] float speed = 5f;

        void Update()
        {
            transform.position += speed * transform.right * Time.deltaTime;
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Deal Damage");
            DealDamage(other.gameObject);
        }

        void DealDamage(GameObject target)
        {
            if (target.TryGetComponent(out Health health))
            {
                health.TakeDamage(damage);
                Pooler.Despawn(gameObject);
            }
        }
    }
}
