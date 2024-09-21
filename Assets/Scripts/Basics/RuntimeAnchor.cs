using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class RuntimeAnchor<T> : ScriptableObject where T : Object
    {
        public UnityAction OnAnchorProvided;

        public bool IsSet => value != null; // Any script can check if the transform is null before using it, by just checking this bool

        public T Value => value;
        [SerializeField] T value;

        public void Provide(T value)
        {
            this.value = value;

            if (IsSet)
                OnAnchorProvided?.Invoke();
        }

        public void Unset() => value = null;
    }
}
