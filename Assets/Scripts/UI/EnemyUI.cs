using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Guinea.Core;
using Guinea.Event;

namespace Guinea.UI
{
    [RequireComponent(typeof(DestructibleEvent))]
    public class EnemyUI : MonoBehaviour
    {
        [SerializeField]
        private Slider sliderPrefab;
        [SerializeField]
        private Canvas canvas;
        [SerializeField]
        private Vector3 offset;
        [SerializeField]
        private Vector3 localScale;
        [SerializeField]
        private float elapsed;
        private float scaleModifier;
        [Header("Debug")]
        [SerializeField]
        private bool debug;
        private Slider slider;
        private DestructibleEvent destructible;
        private static Camera mainCamera;

        void Awake()
        {
            mainCamera = Camera.main;
            destructible = GetComponent<DestructibleEvent>();
            slider = Instantiate(sliderPrefab, parent: canvas.transform);
            slider.transform.localScale = localScale;
            slider.gameObject.SetActive(false);
        }

        void OnEnable()
        {
            destructible.OnDestructible += OnDestructible;
        }

        void OnDestructible(float percent)
        {
            slider.gameObject.SetActive(true);
            slider.value = percent;
            if (percent <= 0) // * When enemy died, hide healthBar slider
            {
                slider.gameObject.SetActive(false);
                return;
            }
            StopAllCoroutines();
            StartCoroutine(AutoHide());
            if (debug) Commons.Log("Call OnDestructible on EnemyUI ^-^");

        }
        void FixedUpdate()
        {
            slider.gameObject.transform.position = mainCamera.WorldToScreenPoint(transform.position + offset);
            scaleModifier = Vector3.Distance(mainCamera.transform.position, transform.position);
            if (debug) Commons.Log("Distance from object to camera: " + scaleModifier);
        }

        IEnumerator AutoHide()
        {
            yield return new WaitForSeconds(elapsed);
            if (slider) slider.gameObject.SetActive(false);
        }
    }
}