using UnityEngine;
using TMPro;
using Guinea.Core;
using Guinea.Event;

namespace Guinea.UI
{
    [RequireComponent(typeof(DestructibleEvent))]
    public class MiniUI : MonoBehaviour
    {

        [SerializeField]
        private Canvas canvas;
        [SerializeField]
        private bool debug;
        private TextMeshProUGUI health;

        private DestructibleEvent destructible;

        void Awake()
        {
            health = canvas.GetComponentInChildren<TextMeshProUGUI>();
            destructible = GetComponent<DestructibleEvent>();
            health.text = "Health: " + destructible.currentHealth;
        }

        void OnEnable()
        {
            destructible.OnDestructible += OnDestructible;
        }

        void OnDisable()
        {
            destructible.OnDestructible -= OnDestructible;
        }
        private void OnDestructible(float ratio)
        {
            health.text = "Health: " + destructible.currentHealth;
            if (debug) Commons.Log($"Current Health: {destructible.currentHealth}");
            // MasterManager.GetDialogUI().SetMessage("Dangerous dangerous: " + destructible.currentHealth).Show();
        }
    }
}