using UnityEngine;

namespace Guinea
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private Transform target;
        [SerializeField]
        private Vector3 offset;

        [SerializeField]
        private float minZoom;
        [SerializeField]
        private float maxZoom;
        [SerializeField]
        private float zoomSpeed;
        [SerializeField]
        private float pitch;
        [SerializeField]
        private float yawSpeed;
        [SerializeField]
        private float initialZoom;
        private float currentZoom;
        private float currentYaw;

        void Awake()
        {
            currentZoom = initialZoom;
        }

        void OnValidate()
        {
            initialZoom = initialZoom<minZoom?minZoom:initialZoom;
            initialZoom = initialZoom>maxZoom?maxZoom:initialZoom;
        }
        void Update()
        {
            currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
            currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
            currentYaw += Input.GetAxis("Mouse X") * yawSpeed;
        }

        void LateUpdate()
        {
            if (target == null) return;
            transform.localPosition = target.position + offset * currentZoom;
            transform.LookAt(target.position + Vector3.up * pitch);
            transform.RotateAround(target.position, Vector3.up, currentYaw + target.localEulerAngles.y);
        }

    }
}