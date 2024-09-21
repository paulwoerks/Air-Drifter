using UnityEngine;

namespace Game
{
    public class Enemy : MonoBehaviour, ISpawnable
    {
        [SerializeField] int health;
        [SerializeField] int maxHealth;
        [SerializeField] float speed = 1f;
        [SerializeField] float rotationSpeed = 5f;

        [SerializeField] float flowResistance = .5f;
        [SerializeField] Rigidbody2D rb;
        [SerializeField] LevelPhysics physics;
        [SerializeField] TransformAnchor player;

        void Awake()
        {
            OnSpawn();
        }

        public void OnSpawn()
        {
            health = maxHealth;
        }

        // Update is called once per frame
        void Update()
        {
            TurnToTarget();
            Accelerate();
            AddExternalForces();
        }

        public void TakeDamage(int damage)
        {
            health -= damage;

            if (health <= 0)
            {
                health = 0;
                Die();
            }
        }

        void Die()
        {
            Debug.Log("Die");
            Pooler.Despawn(gameObject);
        }

        void Accelerate()
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }

        void TurnToTarget()
        {
            Vector3 targetDirection = (player.Value.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        void AddExternalForces()
        {
            float resistance = (1f - flowResistance);
            rb.AddForce(physics.GetWaterFlowForce() * resistance);
        }
    }
}
