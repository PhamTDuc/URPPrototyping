using System;
using System.Collections;
using UnityEngine;

namespace Guinea.Core
{
    [System.Serializable]
    public class Timer : MonoBehaviour
    {
        private int duration;
        public int Duration { get { return duration; } }
        // [SerializeField]
        // [Range(1, 5)]
        // private int tick;
        // public int Tick { get { return tick; } }
        private bool isPaused;
        public bool IsPaused { get { return isPaused; } }
        private int remainingDuration;

        private Action onTimerStart = delegate { };
        private Action<int> onTimerChange = delegate { };
        private Action<bool> onTimerSetPaused = delegate { };
        private Action onTimerFinish = delegate { };

        public void ResetTimer()
        {
            remainingDuration = duration;
            // onTimerStart = delegate { };
            // onTimerChange = delegate { };
            // onTimerSetPaused = delegate { };
            // onTimerFinish = delegate { };
            isPaused = false;
        }

        public Timer SetDuration(int duration)
        {
            this.duration = duration;
            return this;
        }

        // public Timer SetTick(int tick)
        // {
        //     this.tick = tick;
        //     return this;
        // }

        public void SetPaused(bool paused)
        {
            isPaused = paused;
            onTimerSetPaused(isPaused);
        }


        public Timer OnStart(Action action)
        {
            onTimerStart += action;
            return this;
        }

        public Timer OnChange(Action<int> action)
        {
            onTimerChange += action;
            return this;
        }

        public Timer OnFinish(Action action)
        {
            onTimerFinish += action;
            return this;
        }

        public Timer OnSetPaused(Action<bool> action)
        {
            onTimerSetPaused += action;
            return this;
        }

        public void StartTimer()
        {
            ResetTimer();
            onTimerStart();
            StopAllCoroutines();
            StartCoroutine(UpdateTimer());
        }
        IEnumerator UpdateTimer()
        {
            while (remainingDuration >= 0)
            {
                if (!isPaused)
                {
                    onTimerChange(remainingDuration);
                    remainingDuration -= 1;
                    yield return new WaitForSeconds(1);
                }
                yield return null;
            }
            Finish();
        }
        public void Finish()
        {
            onTimerFinish();
        }
    }
}