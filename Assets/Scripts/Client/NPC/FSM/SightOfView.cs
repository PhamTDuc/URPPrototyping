using System;
using System.Collections.ObjectModel;
using UnityEngine;
using Guinea.Core;

namespace Guinea
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(PlayableProperties))]
    public class SightOfView : MonoBehaviour
    {
        [SerializeField]
        private LayerMask layers;
        [SerializeField]
        private LayerMask obstacles;

        [Header("Debug")]
        [SerializeField]
        private bool debug;
        private static readonly int maxSize = 5;
        private Collider[] colliders = new Collider[maxSize];
        private GameObject[] targets = new GameObject[maxSize]; // * Helper targets is used for access Collider.gameObject easier
        private int count;

        private PlayableProperties properties;
        public ReadOnlyCollection<GameObject> Targets
        {
            get { return Array.AsReadOnly(targets); }
        }
        public float ViewRadius => properties.ViewRadius;

        void Awake()
        {
            properties = GetComponent<PlayableProperties>();
        }
        public void CheckFOV()
        {
            count = Physics.OverlapSphereNonAlloc(transform.position, properties.ViewRadius, colliders, layers);
            for (int i = 0; i < count; i++)
            {
                Vector3 toward = colliders[i].transform.position - transform.position;
                if (Vector3.Angle(transform.forward, toward.normalized) < properties.ViewAngle / 2.0f && !Physics.Raycast(transform.position, toward.normalized, toward.magnitude, obstacles))
                {
                    if (debug) Commons.Log("SightOfView.cs See: " + colliders[i]);
                    targets[i] = colliders[i].gameObject;
                }
                else
                {
                    targets[i] = null;
                }
            }
            for (int i = count; i < targets.Length; i++)
            {
                targets[i] = null;
            }
        }

        void OnDrawGizmosSelected()
        {
            foreach (var obj in targets)
            {
                if (obj == null) continue;
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position, obj.transform.position);
            }
        }
    }
}