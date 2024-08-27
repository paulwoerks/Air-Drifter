using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game
{
    public class Player : MonoBehaviour
    {
        [Header("Default")]
        [SerializeField] float defaultDrag = 0.2f;
        [SerializeField] float defaultRotationSpeed = 25f;
        [SerializeField] float gravity = 0.5f;
        [SerializeField] float fireRate = 0.2f;

        [Header("Boost")]
        [SerializeField] float boostRotationSpeed = 1f;
        [SerializeField] float moveSpeed = 35f;
        [SerializeField] float boostDrag = 3f;

        [SerializeField] Rigidbody2D rb;
        [SerializeField] GameObject projectile;

        float RotationSpeed => isBoosting ? boostRotationSpeed : defaultRotationSpeed;
        float Drag => isBoosting ? boostDrag : defaultDrag;
        float Gravity => isBoosting ? 0 : gravity;

        JoystickReader movementStick;

        bool isBoosting = false;

        private void Awake()
        {
            movementStick = new("Movement");
            SetBoostMode(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SetBoostMode(true);
                StopAllCoroutines();
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                SetBoostMode(false);
                StartCoroutine(ShootRoutine());
            }
        }

        void SetBoostMode(bool active)
        {
            isBoosting = active;
            SetGravity();
            rb.drag = Drag;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            Rotate();

            if (isBoosting)
            {
                Accelerate();
            }
        }

        void Shoot()
        {
            Pooler.Spawn(projectile, transform.position, transform.rotation);
        }

        void SetGravity()
        {
            rb.gravityScale = Gravity;
        }

        void Accelerate()
        {
            float speed = moveSpeed * movementStick.Power;
            Vector3 force = speed * transform.forward;
            //transform.position += force * Time.deltaTime;
            rb.AddForce(force);
        }

        void Rotate()
        {
            Vector3 stickDirection = movementStick.Direction2D();
            if (!movementStick.IsPressed || stickDirection == Vector3.zero)
                return;

            Quaternion targetRotation = Quaternion.LookRotation(stickDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);
        }

        IEnumerator ShootRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(fireRate);
                Shoot();
            }
        }
    }
}