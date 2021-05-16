using UnityEngine;
namespace Guinea.Core
{
    public abstract class IInteractable : MonoBehaviour
    {
        [SerializeField]
        private float radius;
        [Header("Debug")]
        [SerializeField]
        private bool debug;
        public abstract void Interact(ICanInteract canInteract);

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, radius);
        }

        void OnTriggerEnter(Collider collider)
        {
            if (debug) Commons.Log($"Interactable.cs, collide with {collider.name}");
            ICanInteract canInteract = collider.GetComponent<ICanInteract>();
            if (canInteract != null)
            {
                Interact(canInteract);
                Destroy(gameObject);
            }

        }
    }
}