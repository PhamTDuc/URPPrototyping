using UnityEngine;

namespace Guinea.Core
{

    #region --- helper ---
    [System.Serializable]
    public class WheelInfo
    {
        public WheelCollider wheelCollider;
        [HideInInspector]
        public Transform visualWheel;
    }
    #endregion
}

