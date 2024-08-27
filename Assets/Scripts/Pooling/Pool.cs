using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [System.Serializable]
    ///<summary>
    ///A Pool of objects based on a Queue
    ///</summary>///
    public class Pool
    {
        [SerializeField] GameObject prefab;
        public GameObject Prefab => prefab;
        [SerializeField] int preloadSize;
        Queue<GameObject> instances = new();

        public Pool(GameObject prefab)
        {
            this.prefab = prefab;
        }

        ///<summary>
        ///Initiates prefabs of this pool based on the preloadSize
        ///</summary>///
        public void Preload()
        {
            for (int i = 0; i < preloadSize; i++)
            {
                instances.Enqueue(GameObject.Instantiate(prefab));
            }
        }


        ///<summary>
        /// Removes and returns available instance from the pool, otherwise creates a new one and returns it.
        ///</summary>///
        public GameObject GetFromPool()
        {
            return instances.Count > 0 ? instances.Dequeue() : GameObject.Instantiate(prefab);
        }

        ///<summary>
        /// Adds instance (back) to the pool
        ///</summary>///
        public void AddToPool(GameObject gameObject)
        {
            instances.Enqueue(gameObject);
        }
    }
}