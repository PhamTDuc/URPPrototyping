using UnityEngine;
using UnityEngine.SceneManagement;

namespace Guinea.Core
{
    public class LevelManager : MonoBehaviour, IManager
    {
        [SerializeField]
        private int duration;
        [SerializeField]
        private TimerCountDown timer;
        private string team;
        private bool isRunning;
        public ManagerStatus status { get; private set; }

        void Start()
        {
            Commons.Assert(timer != null, "TimerCountDown could not be null!!");
            timer.SetDuration(duration).OnFinish(OnFinish);
            timer.ResetTimer();
        }

        public LevelManager()
        {
            status = ManagerStatus.Initialized;
        }
        public void Initialize() { }

        public void LoadNextLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void RestartCurrentLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void StartCountDown(string team, Color color)
        {
            if (isRunning) return;
            isRunning = true;
            this.team = team;
            timer.SetColor(color);
            timer.StartTimer();
        }

        public void OnFinish()
        {
            Commons.Log($"Game Over. Team {this.team} win!!!");
            // LoadNextLevel();
        }

        public void ResetCountDown(string team)
        {
            if (this.team == team)
            {
                isRunning = false;
                this.team = null;
                timer.ResetTimer();
            }

        }
    }
}