using System;
using System.Collections;
using UnityEngine;
using Guinea.Core;

namespace Guinea
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(SightOfView))]
    [RequireComponent(typeof(PlayableProperties))]
    public class Memory : MonoBehaviour, IMemory
    {
        [SerializeField]
        private bool debug;
        private MemoryInfo[] memories = new MemoryInfo[10]; // ? Used for better performance (memories.Count <= target.Length)
        private SightOfView sov;
        private PlayableProperties properties;
        public MemoryInfo[] Memories { get { return memories; } } // ! User MUST check if (memory.obj != null) before use

        void Awake()
        {
            properties = GetComponent<PlayableProperties>();
            sov = GetComponent<SightOfView>();
            for (int i = 0; i < memories.Length; i++)
            {
                memories[i] = new MemoryInfo();
            }
        }
        private void UpdateMemory()
        {
            for (int i = 0; i < sov.Targets.Count; i++)
            {
                AddOrUpdateMemory(sov.Targets[i]);
            }
        }
        private void AddOrUpdateMemory(GameObject target)
        {
            MemoryInfo memory = Array.Find(memories, mem => target != null && mem.obj == target);
            if (memory == null)
            {
                memory = Array.Find(memories, mem => mem.obj == null);
                if (memory == null)
                {
                    memory = memories[memories.Length - 1];
                }

            }
            memory.obj = target;
            memory.lastSeen = Time.time;
        }

        private void ForgetMemories(float olderThan)
        {
            foreach (MemoryInfo memory in memories)
            {
                if (memory != null && memory.Age > olderThan)
                {
                    memory.obj = null;
                }

            }
        }
        private void CalculateScore(Func<MemoryInfo, float> func)
        {
            foreach (var memory in memories)
            {
                memory.score = func(memory);
            }
        }

        void Start()
        {
            StopAllCoroutines();
            StartCoroutine(ImplementMemoryCoroutine());
        }

        private IEnumerator ImplementMemoryCoroutine()
        {
            while (true)
            {
                ImplementMemory();
                yield return new WaitForSeconds(properties.MemoryDelay);
            }
        }

        private void ImplementMemory()
        {
            sov.CheckFOV();
            UpdateMemory();
            ForgetMemories(properties.MemorySpan);
            CalculateScore(CalculateScore);
        }


        private float CalculateScore(MemoryInfo memory)
        {
            if (memory.obj == null)
            {
                return 0;
            }
            float distanceScore = 1.0f - Vector3.Distance(memory.obj.transform.position, 
            gameObject.transform.position) / (sov.ViewRadius * 2.0f);
            if (debug) Commons.Log($"Memory.cs Score of {memory.obj.name}: {distanceScore}");
            return distanceScore;
        }
        private void OnDrawGizmosSelected()
        {
            foreach (MemoryInfo memory in memories)
            {
                if (memory == null || memory.obj == null)
                {
                    continue;
                }
                Color color = Color.red;
                color.a = 0.4f;
                Gizmos.color = color;
                Gizmos.DrawSphere(memory.obj.transform.position, 1.0f);
            }
        }
        public bool NotInMemory(GameObject obj)
        {
            return Array.Find(memories, mem => mem.obj != null && mem.obj == obj) != null;
        }

        public bool InMemory(string layerName, string tagName)
        {
            return Array.Find(memories, mem => Utility.Filter(mem.obj, layerName, tagName)) != null;
        }

        public bool InMemory(string layerName, string tagName, out MemoryInfo info)
        {
            info = Array.Find(memories, mem => Utility.Filter(mem.obj, layerName, tagName));
            return info != null;
        }
    }

}