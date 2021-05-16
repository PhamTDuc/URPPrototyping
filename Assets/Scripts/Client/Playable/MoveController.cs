using UnityEngine;
using Guinea.Core;

namespace Guinea
{
    [RequireComponent(typeof(PlayableProperties))]
    [RequireComponent(typeof(Rigidbody))]
    public class MoveController : MonoBehaviour
    {
        [Header("Wheels System")]
        [SerializeField]
        private WheelInfo FL;
        [SerializeField]
        private WheelInfo FR;
        [SerializeField]
        private WheelInfo BL;
        [SerializeField]
        private WheelInfo BR;
        private PlayableProperties properties;
        private static Quaternion FixRot = Quaternion.AngleAxis(180, Vector3.up);
        void Awake()
        {
            properties = GetComponent<PlayableProperties>();
        }

        void Start()
        {
            SpawnVisualWheels();
        }
        private void UpdateVisualWheel()
        {
            Vector3 pos;
            Quaternion rot;

            if (FL.wheelCollider != null)
            {
                FL.wheelCollider.GetWorldPose(out pos, out rot);
                FL.visualWheel.position = pos;
                FL.visualWheel.rotation = rot;
            }

            if (FR.wheelCollider != null)
            {
                FR.wheelCollider.GetWorldPose(out pos, out rot);
                FR.visualWheel.position = pos;
                FR.visualWheel.rotation = rot * FixRot;
            }

            if (BL.wheelCollider != null)
            {
                BL.wheelCollider.GetWorldPose(out pos, out rot);
                BL.visualWheel.position = pos;
                BL.visualWheel.rotation = rot;
            }


            if (BR.wheelCollider != null)
            {
                BR.wheelCollider.GetWorldPose(out pos, out rot);
                BR.visualWheel.position = pos;
                BR.visualWheel.rotation = rot * FixRot;
            }
        }

        private void ChangeBrakeTorque(float torque)
        {
            if (FL.wheelCollider != null) FL.wheelCollider.brakeTorque = torque;
            if (FR.wheelCollider != null) FR.wheelCollider.brakeTorque = torque;
            if (BL.wheelCollider != null) BL.wheelCollider.brakeTorque = torque;
            if (BR.wheelCollider != null) BR.wheelCollider.brakeTorque = torque;
        }

        private void SpawnVisualWheels()
        {
            Commons.Assert(properties.VisualWheel != null, "PlayableProperties.VisualWheel can't not be null.");
            FL.visualWheel = Instantiate(properties.VisualWheel, FL.wheelCollider.transform, false).transform; // TODO: Using PoolManager               
            FR.visualWheel = Instantiate(properties.VisualWheel, FR.wheelCollider.transform, false).transform; // TODO: Using PoolManager
            BL.visualWheel = Instantiate(properties.VisualWheel, BL.wheelCollider.transform, false).transform; // TODO: Using PoolManager
            BR.visualWheel = Instantiate(properties.VisualWheel, BR.wheelCollider.transform, false).transform; // TODO: Using PoolManager
            FL.visualWheel.gameObject.layer = gameObject.layer;
            FR.visualWheel.gameObject.layer = gameObject.layer;
            BL.visualWheel.gameObject.layer = gameObject.layer;
            BR.visualWheel.gameObject.layer = gameObject.layer;
        }

        public void Move(float forward, float steer, bool brake)
        {
            float _forward = Mathf.Clamp(forward, -1f, 1f);
            float _steer = Mathf.Clamp(steer, -1f, 1f);
            if (FL.wheelCollider != null) FL.wheelCollider.steerAngle = _steer * properties.SteerAngle;
            if (FR.wheelCollider != null) FR.wheelCollider.steerAngle = _steer * properties.SteerAngle;
            if (BL.wheelCollider != null) BL.wheelCollider.motorTorque = _forward * properties.Motor;
            if (BR.wheelCollider != null) BR.wheelCollider.motorTorque = _forward * properties.Motor;

            if (brake)
            {
                ChangeBrakeTorque(properties.BrakeForce);
                if (BL.wheelCollider != null) BL.wheelCollider.motorTorque = 0.0f;
                if (BR.wheelCollider != null) BR.wheelCollider.motorTorque = 0.0f;
            }
            else
            {
                if (Mathf.Abs(_forward) <= 0.0005f)
                {
                    ChangeBrakeTorque(properties.Ratio * properties.BrakeForce);
                }
                else
                {
                    ChangeBrakeTorque(0.0f);
                }

            }
            UpdateVisualWheel();
        }
    }
}