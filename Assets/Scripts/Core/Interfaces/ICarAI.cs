using UnityEngine;

namespace Guinea.Core
{
    public interface ICarAI
    {
        bool SetTarget(Transform target);
        void SetRandomTarget();
        void SetTargetByVec3(Vector3 new_target);
        void MoveAI(); // *This will be Called in Fixed Update of Current State
        bool HasTarget { get; }
        Transform Target { get; }
    }
}