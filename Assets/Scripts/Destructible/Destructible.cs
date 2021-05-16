using System;
using UnityEngine;
using Guinea.Core;
using Guinea.Event;
namespace Guinea.Destructible
{
    public class Destructible : MonoBehaviour, IDestructible, DestructibleEvent
    {
        [SerializeField]
        private DestructibleProperties properties;
        [SerializeField]
        private bool destroyParent;
        [SerializeField]
        private GameObject destroyedVersion;
        private IDestructible parent;
        // private Destructible[] children;
        public event Action<float> OnDestructible = delegate { };


        public float maxHealth
        {
            get { return properties.maxHealth; }
        }
        public float currentHealth
        {
            get { return properties.currentHealth; }
        }

        void Awake()
        {
            parent = transform.root.GetComponent<IDestructible>();
            // children = GetComponentsInChildren<Destructible>();
        }

        public void GetDamage(int damage)
        {
            properties.currentHealth -= damage;
            // Commons.Log("Destroy Parent: " + gameObject.name+"(health="+properties.currentHealth+")");

            if (parent != GetComponent<IDestructible>())
            {
                parent?.GetDamage(damage);
            }
            else if (properties.maxHealth != 0)
            {
                OnDestructible((float)properties.currentHealth / properties.maxHealth);
            }

            if (properties.currentHealth <= 0.0f)
            {

                DestroyWhenDied();
            }
        }

        protected virtual void DestroyWhenDied()
        {
            Commons.Log("Destroy when died called "+ gameObject.name);
            if (destroyedVersion != null)
            {
                Instantiate(destroyedVersion, transform.position, transform.rotation); // TODO: Using PoolManager instead of Instantiate
            }

            if (destroyParent) Destroy(transform.parent.gameObject);
            else Destroy(gameObject);
        }

    }
}