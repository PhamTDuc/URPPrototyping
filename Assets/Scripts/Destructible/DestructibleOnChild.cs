using System;
using UnityEngine;
using Guinea.Core;
namespace Guinea.Destructible
{
    public class DestructibleOnChild : MonoBehaviour, IDestructible
    {
        [SerializeField]
        private DestructibleProperties properties;

        private IDestructible parent;
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
        }

        public void GetDamage(int damage)
        {
            properties.currentHealth -= damage;
            if (properties.currentHealth <= 0.0f)
            {
                Destroy(gameObject);
            }
            // Commons.Log("Destroy Child: " + gameObject.name);
            // if(parent!)
            parent?.GetDamage(damage);
        }

        public void DestroyWhenDied()
        {
            throw new NotImplementedException();
        }
    }
}