using UnityEngine;

namespace Game
{
    public interface IKillable
    {
        public void Die();
    }
    public class Enemy : MonoBehaviour, ISpawnable, IDespawnable, IKillable
    {
        [SerializeField] float speed = 1f;
        [SerializeField] float rotationSpeed = 5f;

        [SerializeField] float flowResistance = .5f;
        Rigidbody rb;
        Health health;
        [SerializeField] LevelPhysics physics;
        [SerializeField] TransformAnchor player;
        [SerializeField] GroupAnchors group;

        void Awake()
        {
            rb = GetComponent<Rigidbody>();
            health = GetComponent<Health>();
            OnSpawn();
        }

        public void OnSpawn()
        {
            group.Add(gameObject);
            health.SetHealth();
        }

        public void OnDespawn()
        {
            group.Remove(gameObject);
        }

        // Update is called once per frame
        void Update()
        {
            TurnToTarget();
            Accelerate();
            AddExternalForces();
        }

        public void Die()
        {
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
            if (physics == null) return;
            Vector2 externalForces = physics.GetForces();
            if (externalForces == Vector2.zero) return;

            float resistance = (1f - flowResistance);
            rb.AddForce(externalForces * resistance);
        }
    }
}
