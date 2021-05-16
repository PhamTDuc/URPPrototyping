using UnityEngine;

namespace Guinea.Core
{
    public interface IBeThrown
    {
        void Execute(float force, Transform local);
    }
}