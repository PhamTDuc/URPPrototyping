using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace Guinea.Core
{
    [RequireComponent(typeof(Timer))]
    public class TimerCountDown : MonoBehaviour
    {
        [Header("Timer UI references")]
        private Timer timer;
        [SerializeField]
        private Image fill;
        [SerializeField]
        private TextMeshProUGUI duration;
        private Image[] images;
        void Awake()
        {
            timer = GetComponent<Timer>();
            timer.OnChange(UpdateUI);
            images = GetComponentsInChildren<Image>();
        }

        void Update()
        {
            // if (Input.GetKeyDown(KeyCode.Q))
            // {
            //     timer.StartTimer();
            // }
        }

        private void UpdateUI(int remainingDuration)
        {
            fill.fillAmount = (float)remainingDuration / timer.Duration;
            duration.text = string.Format("{0:D2}:{1:D2}", remainingDuration / 60, remainingDuration % 60);
        }

        public TimerCountDown OnChange(Action<int> action)
        {
            timer.OnChange(action);
            return this;
        }

        public TimerCountDown SetDuration(int duration)
        {
            timer.SetDuration(duration);
            return this;
        }

        public TimerCountDown OnFinish(Action action)
        {
            timer.OnFinish(action);
            return this;
        }

        public void StartTimer()
        {
            gameObject.SetActive(true);
            timer.StartTimer();
        }

        public void ResetTimer()
        {
            gameObject.SetActive(false);
            timer.ResetTimer();
        }

        public void SetColor(Color color)
        {
            foreach (Image img in images)
            {
                img.color = color;
            }
            duration.color = color;
        }

    }
}
