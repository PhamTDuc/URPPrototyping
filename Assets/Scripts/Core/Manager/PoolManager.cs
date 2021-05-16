using System;
using System.Collections.Generic;
using UnityEngine;

namespace Guinea.Core
{
    public class PoolManager : MonoBehaviour, IManager
    {
        [System.Serializable]
        private class Pool
        {
            public PoolObjectBase obj;
            public int size;
        }

        [SerializeField]
        private List<Pool> pools;
        [SerializeField]
        private bool debug;
        private Dictionary<ObjectType, Pool> poolsList = new Dictionary<ObjectType, Pool>();
        private Dictionary<ObjectType, Queue<PoolObjectBase>> poolsDict = new Dictionary<ObjectType, Queue<PoolObjectBase>>();

        public ManagerStatus status { get; private set; }

        void Awake()
        {
            foreach (Pool pool in pools)
            {
                Queue<PoolObjectBase> objectPool = new Queue<PoolObjectBase>();
                Commons.Assert(pool.obj != null, "PoolObjectBase can't not be null");
                poolsList.Add(pool.obj.Type, pool);
                poolsDict.Add(pool.obj.Type, objectPool);
                AddToPool(pool, pool.size);
            }
        }
        private void AddToPool(Pool pool, int count)
        {
            for (int i = 0; i < count; i++)
            {
                try
                {
                    Queue<PoolObjectBase> currentPool = poolsDict[pool.obj.Type];
                    PoolObjectBase obj = Instantiate(pool.obj.gameObject).GetComponent<PoolObjectBase>();
                    obj.gameObject.SetActive(false);
                    currentPool.Enqueue(obj);
                }
                catch (KeyNotFoundException)
                {
                    if (debug) Commons.LogWarning($"PoolManager.cs: Can't get object {pool.obj.Type} from pool");
                }
            }
        }

        public GameObject Spawn(ObjectType objectType, Vector3 pos = default(Vector3), Quaternion rot = default(Quaternion))
        {
            PoolObjectBase spawnObj;
            Commons.Assert(poolsDict.ContainsKey(objectType), $"PoolManager.cs: The key {objectType} not found");
            Queue<PoolObjectBase> pool = poolsDict[objectType];
            if (pool.Count == 0)
            {
                if (debug) Commons.LogWarning($"PoolManager.cs: The pool {objectType} is EMPTY, instantiate new object. Please increase SIZE of this pool");
                AddToPool(poolsList[objectType], 1);
            }
            spawnObj = pool.Dequeue();
            spawnObj.gameObject.SetActive(true);
            spawnObj.transform.position = pos;
            spawnObj.transform.rotation = rot;


            return spawnObj.gameObject;
        }

        public void DeActivate(PoolObjectBase obj)
        {
            if (debug) Commons.Log($"PoolManager.cs: The key {obj.Type} deactivate {obj}");
            obj.gameObject.SetActive(false);
            obj.transform.SetParent(null);
            Commons.Assert(poolsDict.ContainsKey(obj.Type), $"PoolManager.cs: The key {obj.Type} not found");
            poolsDict[obj.Type].Enqueue(obj);

        }

        public void DeactiveOrDestroy(GameObject obj)
        {
            PoolObjectBase poolObj = obj.GetComponent<PoolObjectBase>();
            if (poolObj == null)
            {
                Destroy(obj);
                return;
            }
            if (!poolsDict.ContainsKey(poolObj.Type))
            {
                if (debug) Commons.LogWarning($"PoolManager.cs: The {obj} has PoolObjectBase, however no {poolObj.Type} in pools. Please add {poolObj.Type} to pools");
                poolsDict.Add(poolObj.Type, new Queue<PoolObjectBase>());
            }
            DeActivate(poolObj);
        }

        public void Initialize()
        {
            status = ManagerStatus.Initialized;
        }
    }
}