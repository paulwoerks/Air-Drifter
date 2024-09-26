using System.Collections;
using UnityEngine;
using UnityEngine.Events;


namespace Game
{
    public class Player : MonoBehaviour, IKillable
    {
        [SerializeField] float fireRate = 0.2f;
        [SerializeField] float boostSpeed = 45f;
        [SerializeField] Transform model;

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

        Rigidbody rb;
        [SerializeField] GameObject projectile;

        [SerializeField] UnityEvent OnStartBoost;
        [SerializeField] UnityEvent OnEndBoost;

        [SerializeField] LevelPhysics physics;
        [SerializeField] TransformAnchor playerAnchor;

        JoystickReader movementStick;
        bool isBoosting = false;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            movementStick = new("Movement");
            SetBoostMode(false);
        }

        private void OnEnable()
        {
            playerAnchor.Provide(transform);
        }

        private void OnDisable()
        {
            playerAnchor.Unset();
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

        public void Die()
        {
            Debug.Log("Player dead!");
        }

        #region Movement
        void SetBoostMode(bool active)
        {
            currentMoveStats = active ? boostMovement : defaultMovement;
            isBoosting = active;
            rb.drag = currentMoveStats.drag;
            if (isBoosting) { OnStartBoost.Invoke(); } else { OnEndBoost.Invoke(); }
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            //Rotate();
            //Rotate2();
            Rotate3();
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
            Vector2 externalForces = physics.GetForces();
            if (externalForces == Vector2.zero) return;

            float resistance = (1f - currentMoveStats.flowResistance);
            rb.AddForce(externalForces * resistance);
        }

        void Accelerate()
        {
            Vector3 force = boostSpeed * movementStick.Power * transform.right;
            rb.AddForce(force);
        }

        void Rotate()
        {
            Vector3 stickDirection = movementStick.GetDirection2D();

            if (!movementStick.IsPressed || stickDirection == Vector3.zero)
                return;

            float rotationSpeed = currentMoveStats.rotationSpeed;

            Quaternion targetRotation = Quaternion.LookRotation(stickDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 1000 * rotationSpeed * Time.deltaTime);
        }

        void Rotate3()
        {
            float offset = 0f;
            Vector2 inputDirection = movementStick.GetDirection2D();
            Vector3 direction = new Vector3(inputDirection.x, inputDirection.y, 0) + transform.position;

            Debug.DrawLine(transform.position, direction, Color.red);
            float angle = Mathf.Atan2(inputDirection.y, inputDirection.x) * Mathf.Rad2Deg + offset;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), currentMoveStats.rotationSpeed * Time.deltaTime);

            model.localEulerAngles = new Vector3(angle * 2, 0, 0);
        }

        IEnumerator ShootRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(fireRate);
                Shoot();
            }
        }
        #endregion
    }
}