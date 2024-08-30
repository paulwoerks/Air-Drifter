using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Enemy : MonoBehaviour, ISpawnable
    {
        [SerializeField] int health;
        [SerializeField] int maxHealth;
        [SerializeField] float speed = 1f;
        [SerializeField] float rotationSpeed = 5f;
        [SerializeField] Transform player;

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
        }

        public void TakeDamage(int damage)
        {
            health -= damage;

            if (health <= 0)
            {
                Pooler.Despawn(gameObject);
            }
        }

        void Accelerate()
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }

        void TurnToTarget()
        {
            Vector3 targetDirection = (player.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
