using UnityEngine;

namespace Guinea
{
    public class PlayableNPC : Controller
    {
        [SerializeField]
        private Transform player;
        [SerializeField]
        private LayerMask whatIsGround, whatIsPlayer;

        // Patrolling
        private bool walkPointSet;
        [SerializeField]
        private float walkPointRange;

        // Attacking
        [SerializeField]
        private float timeBetweenAttacks;
        bool alreadyAttacked;

        // Sensors
        [Header("Sensors")]
        [SerializeField]
        private float sensorLength;
        [SerializeField]
        private Vector3 sensorOffset;
        [SerializeField]
        private float sideLength;
        [SerializeField]
        private float sensorAngle;
        [SerializeField]
        private float sidewaySensorLength;
        // private int flag = 0;
        private Vector3 sensorPos;

        #region State
        [Header("Ranges")]
        [SerializeField]
        private float sightRange, attackRange;

        private bool playerInSightRange, playerInAttackRange;
        private Vector3 target;
        #endregion

        #region MoveAndObstacle
        private enum AVOID_STATE
        {
            NORMAL,
            AVOIDING,
        }
        private AVOID_STATE avoidLeft = AVOID_STATE.NORMAL;
        private AVOID_STATE avoidRight = AVOID_STATE.NORMAL;
        private AVOID_STATE avoidForward = AVOID_STATE.NORMAL;
        private Vector3 direction = Vector3.zero;
        private bool brake = false;
        private float obstacle_steer = 0f;
        #endregion

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, sightRange);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }

        void Update()
        {
            // Check for Sight and Attack Range
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
            if (!playerInSightRange && !playerInAttackRange) Patrolling();
            if (playerInSightRange && !playerInAttackRange) ChasePlayer();
            if (playerInAttackRange) AttackPlayer();
        }
        void FixedUpdate()
        {
            CalculateMove();
            moveController.Move(direction.z, direction.x, brake);
        }


        private void Patrolling()
        {
            if (!walkPointSet) SearchTarget();

            Vector3 distanceToWalkPoint = transform.position - target;
            // WalkPoint Reached
            if (distanceToWalkPoint.magnitude < 1.0f)
            {
                walkPointSet = false;
            }
        }

        private void SearchTarget()
        {
            // Calculate Random point in Range
            float randomZ = Random.Range(0f, walkPointRange);
            float randomX = Random.Range(-walkPointRange, walkPointRange);

            target = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

            if (Physics.Raycast(target, -transform.up, 2.0f, whatIsGround))
            {
                walkPointSet = true;
            }
        }

        private void ChasePlayer()
        {
            target = player.position;
        }

        private void AttackPlayer()
        {
            if (!alreadyAttacked)
            {
                // Attack 
                shot.Shoot(true);
                // Attack END

                alreadyAttacked = true;
                Invoke("ResetAttack", timeBetweenAttacks);
            }
        }

        private void ResetAttack()
        {
            alreadyAttacked = false;
        }

        private void Sensors()
        {
            #region Reset Sensor
            avoidLeft = AVOID_STATE.NORMAL;
            avoidRight = AVOID_STATE.NORMAL;
            avoidForward = AVOID_STATE.NORMAL;
            obstacle_steer = 0f;
            #endregion

            sensorPos = transform.position + transform.forward * sensorOffset.z + transform.up * sensorOffset.y + transform.right * sensorOffset.x;
            Vector3 rightAngle = Quaternion.AngleAxis(sensorAngle, transform.up) * transform.forward;
            Vector3 leftAngle = Quaternion.AngleAxis(-sensorAngle, transform.up) * transform.forward;

            // Mid Forward Sensor
            RaycastHit hit;
            Vector3 end = sensorPos;
            end += transform.forward * sensorLength;
            Debug.DrawLine(sensorPos, end, Color.red);
            if (Physics.Raycast(sensorPos, transform.forward, out hit, sensorLength))
            {
                // Debug.DrawLine(sensorPos, hit.point, Color.red);
                avoidForward = AVOID_STATE.AVOIDING;
            }

            // Right Forward Sensor
            sensorPos += sideLength * transform.right;
            end = sensorPos;
            end += transform.forward * sensorLength;
            Debug.DrawLine(sensorPos, end, Color.red);
            if (Physics.Raycast(sensorPos, transform.forward, out hit, sensorLength))
            {
                // Debug.DrawLine(sensorPos, hit.point, Color.red);
                avoidRight = AVOID_STATE.AVOIDING;
                obstacle_steer -= 0.5f;
            }

            // Right Angle Sensor
            end = sensorPos;
            end += rightAngle * sensorLength;
            Debug.DrawLine(sensorPos, end, Color.red);
            if (Physics.Raycast(sensorPos, rightAngle, out hit, sensorLength))
            {
                // Debug.DrawLine(sensorPos, hit.point, Color.red);
                avoidRight = AVOID_STATE.AVOIDING;
                obstacle_steer -= 0.5f;
            }

            // Left Forward Sensor
            sensorPos -= 2 * sideLength * transform.right;
            end = sensorPos;
            end += transform.forward * sensorLength;
            Debug.DrawLine(sensorPos, end, Color.red);
            if (Physics.Raycast(sensorPos, transform.forward, out hit, sensorLength))
            {
                // Debug.DrawLine(sensorPos, hit.point, Color.red);
                avoidLeft = AVOID_STATE.AVOIDING;
                obstacle_steer += 0.5f;
            }

            // Left Angle Sensor
            end = sensorPos;
            end += leftAngle * sensorLength;
            Debug.DrawLine(sensorPos, end, Color.red);
            if (Physics.Raycast(sensorPos, leftAngle, out hit, sensorLength))
            {
                // Debug.DrawLine(sensorPos, hit.point, Color.red);
                avoidLeft = AVOID_STATE.AVOIDING;
                obstacle_steer += 0.5f;
            }

            // Right Side Sensor
            // Debug.DrawLine(transform.position, transform.position+transform.right*sidewaySensorLength, Color.red);

            // Left Side Sensor
            // Debug.DrawLine(transform.position, transform.position-transform.right*sidewaySensorLength, Color.red);
        }

        private void CalculateMove()
        {
            brake = false;
            if (playerInAttackRange)
            {
                direction = Vector3.zero;
                brake = true;
                return;
            }
            direction = transform.InverseTransformPoint(target);
            direction /= direction.magnitude;
            Sensors();
            if (avoidForward == AVOID_STATE.AVOIDING || avoidLeft == AVOID_STATE.AVOIDING && avoidRight == AVOID_STATE.AVOIDING)
            {
                direction.z *= -1.0f;
                direction.x = -1.0f * Mathf.Sign(direction.x);

            }
            else if (avoidLeft != avoidRight)
            {
                direction.x = obstacle_steer;
            }
        }
    }
}