using UnityEngine;

namespace Game
{
    /// <summary>
    /// Feeds pooled object back into pooler after fixed amount of time
    /// </summary>
    public class DespawnTimer : MonoBehaviour
    {
        [SerializeField] bool debug;
        [SerializeField] float timer = 1f;

        private void OnEnable()
        {
            Invoke(nameof(Despawn), timer);
        }

        private void OnDisable()
        {
            CancelInvoke();
        }

        void Despawn()
        {
            Pooler.Despawn(gameObject);
        }
    }
}