using UnityEngine;
using Guinea.Core;

namespace Guinea
{
    [RequireComponent(typeof(Timer))]
    public class TimerDemo : MonoBehaviour
    {
        private Timer timer;

        void Awake()
        {
            timer = GetComponent<Timer>();
            timer.OnStart(() => Commons.Log("Start Timer")).OnChange((x) => Commons.Log($"Remaining Timer: {x}")).OnFinish(() => Commons.Log("Finish Timer"));
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                timer.StartTimer();
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                timer.SetPaused(!timer.IsPaused);
            }

            if (Input.GetAxis("Mouse ScrollWheel") > 0.0f)
            {
                // Time.timeScale += 0.1f;
                Commons.Log($"Current TimeScale: {Time.timeScale}");
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0.0f)
            {
                // Time.timeScale -= 0.1f;
                Commons.Log($"Current TimeScale: {Time.timeScale}");
            }

        }
    }
}
