using System.Collections;
using UnityEngine;

namespace Guinea.Destructible
{
    public class AutoDestroy : MonoBehaviour
    {
        [SerializeField]
        private float delay;

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(DestroyInterval());
        }

        private IEnumerator DestroyInterval()
        {
            yield return new WaitForSeconds(delay);
            Destroy(gameObject);
        }
    }
}