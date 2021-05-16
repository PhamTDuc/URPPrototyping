using UnityEngine;
using Guinea.Core;
namespace Guinea
{
    [RequireComponent(typeof(PlayableProperties))]
    public class GunMovement : MonoBehaviour
    {
        [SerializeField]
        private Transform local;
        [SerializeField]
        private float sensitivity;
        [SerializeField]
        private float Ymin;
        [SerializeField]
        private float Ymax;
        [SerializeField]
        private float smooth;
        private float angle;
        public void UpDown(float y)
        {
            angle += y * sensitivity;
            angle = Mathf.Clamp(angle, Ymin, Ymax);
            Quaternion target = Quaternion.Euler(-angle, 0.0f, 0.0f);
            if (local != null)
            {
                local.transform.localRotation = target;
            }
        }
    }
}