using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Guinea.Core;

namespace Guinea
{
    [RequireComponent(typeof(IPlayable))]
    public class CarAI : MonoBehaviour, ICarAI
    {

        // [SerializeField]
        // private float patrolRange;
        [SerializeField]
        private Vector3 sensorOffset;
        [SerializeField]
        private LayerMask whatIsGround;
        [SerializeField]
        private LayerMask obstacles;
        [SerializeField]
        private float sensorLength;
        [SerializeField]
        private float sensorAngle;
        [SerializeField]
        private float sideOffset;
        [SerializeField]
        private bool debug;
        private IPlayable playable;
        [SerializeField]
        private Transform target;
        private Stack<Vector3> avoidance = new Stack<Vector3>();
        private Vector3? currentTarget;

        #region Move and Obstacle
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
        [SerializeField]
        private Transform targetVisual;
        private float obstacle_steer = 0f;
        private bool isBraking = false;
        private Collider prevHit;
        private Collider currentHit;
        private GameObject randomTarget;
        public bool HasTarget => target != null;
        public Transform Target => target;
        #endregion

        void Awake()
        {
            playable = GetComponent<IPlayable>();
            randomTarget = new GameObject($"Target Random of{gameObject.name}");
        }
        public bool SetTarget(Transform new_target)
        {
            if (target == new_target) return false;
            if (new_target != null)
            {
                brake = false;
                target = new_target;
                if (debug) Commons.Log($"Set Target {new_target.gameObject.name}");
                return true;
            }
            target = null;
            brake = true;
            if (debug) Commons.Log("Set target to null");
            return true;
        }

        private void Sensoring()
        {
            if (target == null) return;
            ResetSensor();
            Vector3 sensorPos = transform.position + transform.forward * sensorOffset.z +
            transform.up * sensorOffset.y + transform.right * sensorOffset.x;
            Vector3 rightAngle = Quaternion.AngleAxis(sensorAngle, transform.up) * transform.forward;
            Vector3 leftAngle = Quaternion.AngleAxis(-sensorAngle, transform.up) * transform.forward;

            // Mid Forward Sensor
            currentHit = null;
            RaycastHit hit;
            Vector3 end = sensorPos;
            end += transform.forward * sensorLength;
            Debug.DrawLine(sensorPos, end, Color.red);
            Vector3 hitMidSensor = sensorPos;
            if (Physics.Raycast(sensorPos, transform.forward, out hit, sensorLength, obstacles))
            {
                // Debug.DrawLine(sensorPos, hit.point, Color.red);
                avoidForward = AVOID_STATE.AVOIDING;
                currentHit = hit.collider;
            }

            // Right Forward Sensor
            sensorPos += sideOffset * transform.right;
            end = sensorPos;
            end += transform.forward * sensorLength;
            Debug.DrawLine(sensorPos, end, Color.red);
            if (Physics.Raycast(sensorPos, transform.forward, out hit, sensorLength, obstacles))
            {
                // Debug.DrawLine(sensorPos, hit.point, Color.red);
                avoidRight = AVOID_STATE.AVOIDING;
                currentHit = hit.collider;
                obstacle_steer -= 0.5f;
            }

            // Right Angle Sensor
            end = sensorPos;
            end += rightAngle * sensorLength;
            Debug.DrawLine(sensorPos, end, Color.red);
            if (Physics.Raycast(sensorPos, rightAngle, out hit, sensorLength, obstacles))
            {
                // Debug.DrawLine(sensorPos, hit.point, Color.red);
                avoidRight = AVOID_STATE.AVOIDING;
                currentHit = hit.collider;
                obstacle_steer -= 0.5f;
            }

            // Left Forward Sensor
            sensorPos -= 2 * sideOffset * transform.right;
            end = sensorPos;
            end += transform.forward * sensorLength;
            Debug.DrawLine(sensorPos, end, Color.red);
            if (Physics.Raycast(sensorPos, transform.forward, out hit, sensorLength, obstacles))
            {
                // Debug.DrawLine(sensorPos, hit.point, Color.red);
                avoidLeft = AVOID_STATE.AVOIDING;
                currentHit = hit.collider;
                obstacle_steer += 0.5f;
            }

            // Left Angle Sensor
            end = sensorPos;
            end += leftAngle * sensorLength;
            Debug.DrawLine(sensorPos, end, Color.red);
            if (Physics.Raycast(sensorPos, leftAngle, out hit, sensorLength, obstacles))
            {
                // Debug.DrawLine(sensorPos, hit.point, Color.red);
                avoidLeft = AVOID_STATE.AVOIDING;
                currentHit = hit.collider;

                obstacle_steer += 0.5f;
            }

            // Right Side Sensor
            // Debug.DrawLine(transform.position, transform.position+transform.right*sidewaySensorLength, Color.red);

            // Left Side Sensor
            // Debug.DrawLine(transform.position, transform.position-transform.right*sidewaySensorLength, Color.red);
            #region Avoiding Obstacle
            if (avoidForward == AVOID_STATE.AVOIDING || avoidLeft == AVOID_STATE.AVOIDING && avoidRight == AVOID_STATE.AVOIDING) // * Have to go backward to avoid obstacles
            {
                // if (debug) Commons.Log($"Avoiding in collide");
                if (currentHit != null && currentHit != prevHit || direction.z > 0.0f)
                {
                    prevHit = currentHit;
                    if (debug) Commons.Log($"Avoiding in stack {prevHit}");
                    isBraking = true;
                    StopAllCoroutines();
                    StartCoroutine(BrakeCoroutine());

                    #region Add Avoidances Object
                    Vector3 new_target = transform.position - Mathf.Sign(direction.x) * transform.right * 10.0f;
                    avoidance.Push(new_target);
                    AddVisualWayPoint(new_target);
                    // Vector3 new_target = transform.position - transform.forward * Mathf.Sign(direction.z) * 16.0f;
                    new_target = transform.position - transform.forward * 16.0f;
                    avoidance.Push(new_target);
                    AddVisualWayPoint(new_target);
                    #endregion
                }
            }
            else if (avoidLeft != avoidRight)
            {
                direction.x = obstacle_steer;
                if (debug) Commons.Log("Avoiding Obstacle only");
            }
            #endregion
        }
        public void MoveAI()
        {

            WhereToGo();
            CheckWaypointIsReach();
            Sensoring();
            ResetBrake();
            CalculateSpeed();

            // if (avoidance.Count == 0 && direction.z < 0 && direction.x != 0 && ) CalculateTurnBack();
            playable.Move(direction.z, direction.x, brake);
        }

        private void ResetBrake()
        {
            if (isBraking && playable.Speed < 1.0f)
            {
                if (debug) Commons.Log("Calling ResetBrake()");
                StopAllCoroutines();
                brake = false;
                isBraking = false;
            }
        }

        private void WhereToGo()
        {
            if (avoidance.Count != 0)
            {
                currentTarget = avoidance.Peek();
            }
            else if (target)
            {
                currentTarget = target.position;
            }
            else
            {
                currentTarget = null;
            }

            if (currentTarget == null)
            {
                direction = Vector3.zero;
                return;
            }
            direction = transform.InverseTransformPoint(currentTarget.Value);
            // if (debug) Commons.Log("Distance to target: " + direction.magnitude);
        }

        private void CheckWaypointIsReach()
        {
            if (target != null && direction.magnitude <= 4.0f)
            {
                if (avoidance.Count != 0) avoidance.Pop();
                else target = null;
                if (debug) Commons.Log("Stop Stop!!");
            }
        }

        private void ResetSensor()
        {
            #region Reset Sensor
            avoidLeft = AVOID_STATE.NORMAL;
            avoidRight = AVOID_STATE.NORMAL;
            avoidForward = AVOID_STATE.NORMAL;
            obstacle_steer = 0f;
            #endregion
        }

        private IEnumerator BrakeCoroutine()
        {
            if (debug) Commons.Log("Calling BrakeCoroutine()");
            while (true)
            {
                brake = true;
                yield return new WaitForSeconds(0.01f);
                brake = false;
            }
        }

        private void CalculateSpeed()
        {
            if (!isBraking && direction.magnitude < 12.0f && playable.Speed > 8.0f)
            {
                isBraking = true;
                StopCoroutine("BrakeCoroutine");
                StartCoroutine(BrakeCoroutine());
                if (debug) Commons.Log($"Speed Exceed. Gonna brake: {playable.Speed}");
            }

            // float speed = direction.magnitude;
            // if (direction != Vector3.zero)
            // {
            //     direction = direction.normalized;
            //     speed = Mathf.SmoothStep(0.01f, 0.5f, LinearInterpolate(12.0f, 0.0f, speed));
            //     speed = Mathf.Pow(speed, 2.0f);
            // }
            // direction.z *= speed;
            if (avoidLeft == AVOID_STATE.NORMAL
                && avoidRight == AVOID_STATE.NORMAL
                && avoidForward == AVOID_STATE.NORMAL
                && direction.z < 0
                && Mathf.Abs(direction.x) > 0.2f
                && direction.magnitude > 16.0f)
            {
                if (debug) Commons.Log("Calculate Turn back");
                direction.z = 0.25f; // * Try to fix go Backward problem
                direction.x = Mathf.Sign(direction.x);
            }
        }

        private float LinearInterpolate(float x1, float x2, float value)
        {
            Commons.Assert(x1 != x2, "x1 and x2 cant not be equal!!");
            float a = 1.0f / (x1 - x2);
            float b = -x2 / (x1 - x2);
            return a * value + b;
        }

        public void SetRandomTarget()
        {
            float range = 100.0f;
            Vector3 new_target = new Vector3(Random.Range(-range, range), 1.0f, Random.Range(-range, range));
            while (Vector3.Distance(new_target, gameObject.transform.position) < 60.0f)
            {
                new_target.x = Random.Range(-range, range);
                new_target.z = Random.Range(-range, range);
            }
            SetTargetByVec3(new_target);
        }

        public void SetTargetByVec3(Vector3 new_target)
        {
            randomTarget.transform.position = new_target;
            SetTarget(randomTarget.transform);
        }
        private void AddVisualWayPoint(Vector3 target)
        {
            if (debug && targetVisual) targetVisual.position = target;
        }
    }
}