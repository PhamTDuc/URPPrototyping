using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Guinea.Core
{
    [CreateAssetMenu(fileName = "New Stats", menuName = "Inventory/Item")]
    public class Stats : ScriptableObject, IEnumerable<Stat>
    {
        [SerializeField]
        private List<Stat> _internal;

        public Stat this[ObjectType type]
        {
            get
            {
                return Find(type);
            }
            set
            {
                Add(value);
            }
        }

        public bool Exists(ObjectType type)
        {
            return _internal.Exists(stat => stat.type == type);
        }
        public void Add(Stat other)
        {
            if (Exists(other.type))
            {
                Upgrade(other);
            }
            else
            {
                _internal.Add(new Stat(other));
            }
        }
        public Stat Find(ObjectType type) => _internal.Find(stat => stat.type == type);


        public bool Upgrade(Stat other)
        {
            if (Exists(other.type))
            {
                this[other.type].quantity += other.quantity;
                return true;
            }
            return false;
        }

        public IEnumerator<Stat> GetEnumerator()
        {
            foreach (Stat stat in _internal)
            {
                yield return stat;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}