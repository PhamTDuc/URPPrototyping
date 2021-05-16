using System;
using Guinea.Core;
using UnityEngine;

namespace Guinea
{
    [RequireComponent(typeof(PlayableProperties))]
    public class Throwable : MonoBehaviour
    {
        [SerializeField]
        private float throwForce;
        [SerializeField]
        private Transform hand;
        [SerializeField]
        [Range(0.1f, 2.0f)]
        private float elapsed;
        [SerializeField]
        Vector3 offset;
        private float timer = 0.0f;
        private ObjectType throwable;

        public void Throw(bool isPressed, Action action = null)
        {
            if (!isPressed)
            {
                if (timer > 0.0f)
                {
                    // if (throwable == null)
                    // {
                    //     timer = 0.0f;
                    //     return;
                    // }

                    // IBeThrown beThrown = Instantiate(throwable, hand.position + offset, hand.rotation).GetComponent<IBeThrown>(); // TODO: Using PoolManager instead of Instantiate

                    IBeThrown beThrown = MasterManager.GetPoolManager().Spawn(throwable, hand.position + offset, hand.rotation).GetComponent<IBeThrown>();
                    // IBeThrown beThrown = throwable.GetComponent<IBeThrown>();
                    beThrown?.Execute(throwForce * Mathf.Min(timer / elapsed, 1.0f), hand);
                    if (action != null) action();
                    timer = 0.0f;
                }
                return;
            }
            timer += Time.deltaTime;
        }

        public void SetThrowObject(ObjectType new_throwable)
        {
            throwable = new_throwable;
        }
    }
}