using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed;

    void Update()
    {
        transform.position += speed * transform.forward * Time.deltaTime;
    }
}
