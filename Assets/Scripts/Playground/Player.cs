using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game
{
    public class Player : MonoBehaviour
    {
        [SerializeField] float fireRate = 0.2f;
        [SerializeField] float boostSpeed = 45f;

        [System.Serializable]
        class MoveStats
        {
            public float drag = 0.2f;
            public float rotationSpeed = 25f;
            public float flowResistance = 0.9f;
        }

        [SerializeField] MoveStats defaultMovement;
        [SerializeField] MoveStats boostMovement;

        MoveStats currentMoveStats;

        [SerializeField] Rigidbody2D rb;
        [SerializeField] GameObject projectile;

        [SerializeField] LevelPhysics physics;

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
            currentMoveStats = active ? boostMovement : defaultMovement;
            isBoosting = active;
            rb.drag = currentMoveStats.drag;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            Rotate();
            AddExternalForces();

            if (isBoosting)
            {
                Accelerate();
            }
        }

        void Shoot()
        {
            Pooler.Spawn(projectile, transform.position, transform.rotation);
        }

        void AddExternalForces()
        {
            float resistance = (1f - currentMoveStats.flowResistance);
            rb.AddForce(physics.GetWaterFlowForce() * resistance);
        }

        void Accelerate()
        {
            Vector3 force = boostSpeed * movementStick.Power * transform.forward;
            //transform.position += force * Time.deltaTime;
            rb.AddForce(force);
        }

        void Rotate()
        {
            Vector3 stickDirection = movementStick.Direction2D();

            if (!movementStick.IsPressed || stickDirection == Vector3.zero)
                return;

            float rotationSpeed = currentMoveStats.rotationSpeed;

            Quaternion targetRotation = Quaternion.LookRotation(stickDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
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