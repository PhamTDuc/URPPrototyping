using UnityEngine;
using Guinea.Core;

namespace Guinea
{
    public class Site : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Layer name of one of two teams")]
        private string team;


        [SerializeField]
        [Tooltip("Color for Timer")]
        private Color color;
        [Header("Debug")]
        private bool debug;
        private LayerMask layer;
        private int count = 0;
        void Awake()
        {
            layer = LayerMask.NameToLayer(team);
        }

        void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.layer == layer)
            {
                if (count == 0)
                {
                    if (debug) Commons.Log($"Team {team} ENTER SITE");
                    MasterManager.GetLevelManager().StartCountDown(team, color);
                }
                count++;
            }
        }

        void OnTriggerExit(Collider collider)
        {
            if (collider.gameObject.layer == layer)
            {
                count--;
                if (count == 0)
                {
                    if (debug) Commons.Log($"Team {team} LEAVE SITE");
                    MasterManager.GetLevelManager().ResetCountDown(team);
                }
            }
        }
    }
}