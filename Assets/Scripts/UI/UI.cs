using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Guinea.Core;
using Guinea.Event;

namespace Guinea.UI
{
    public class UI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI Ammo;
        [SerializeField]
        private Image weaponIcon;
        [SerializeField]
        private Slider slider;
        [SerializeField]
        private GameObject player;
        private IDestructible destructible;
        private ShotEvent shotEvent;
        private DestructibleEvent destructibleEvent;

        void Awake()
        {
            shotEvent = player.GetComponent<ShotEvent>();
            destructibleEvent = player.GetComponent<DestructibleEvent>();
            slider.value = 1.0f;
        }

        void OnEnable()
        {
            shotEvent.OnWeapon += OnWeapon;
            destructibleEvent.OnDestructible += OnDestructible;
        }

        void OnDisable()
        {
            shotEvent.OnWeapon -= OnWeapon;
            destructibleEvent.OnDestructible -= OnDestructible;
        }

        private void OnWeapon(int currentAmmo, int maxAmmo, ItemInfo info = null)
        {
            if (info == null)
            {
                weaponIcon.gameObject.SetActive(false);
                Ammo.gameObject.SetActive(false);
                return;
            }
            weaponIcon.gameObject.SetActive(true);
            Ammo.gameObject.SetActive(true);
            weaponIcon.sprite = info.Sprite;
            Ammo.text = string.Format("{00}", currentAmmo) + '/' + string.Format("{00}", maxAmmo);
        }

        private void OnDestructible(float percent)
        {
            slider.value = percent;
        }

    }
}