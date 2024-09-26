using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class Health : MonoBehaviour
    {
        [Header("Stats")]
        [SerializeField] int maxHP = 1;
        [SerializeField] int hp = 1;

        [Header("Events")]
        [SerializeField] UnityEvent OnTakeDamage;
        [SerializeField] UnityEvent OnDie;

        IKillable killable;

        void Awake()
        {
            killable = GetComponent<IKillable>();
        }

        public void SetHealth(int newMaxHP = 0, bool resetHealth = true)
        {
            maxHP = newMaxHP > 0 ? newMaxHP : this.maxHP;
            if (resetHealth) hp = newMaxHP;
        }

        public void TakeDamage(int damage)
        {
            if (damage <= 0) return;

            hp -= damage;
            OnTakeDamage.Invoke();


            bool isDead = hp <= 0;
            if (isDead)
            {
                hp = 0;

                killable?.Die();

                OnDie.Invoke();
            }
        }
    }
}
