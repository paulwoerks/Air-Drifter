using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
    public class CopyPosition : MonoBehaviour
    {
        [SerializeField] Vector3 offset;
        [SerializeField] Transform target;

        void Update() { transform.position = target.position + offset; }
    }
}