using UnityEngine;
using UnityEngine.SceneManagement;
using Guinea.Event;

using System;

namespace Guinea.Core
{
    public class LevelManager : MonoBehaviour, IManager, GameOverEvent
    {
        [SerializeField]
        private int duration;
        [SerializeField]
        private TimerCountDown timer;
        private string team;
        private bool isRunning;

        public event Action<string> OnGameOverEvent = delegate { };

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
            Time.timeScale = 1.0f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void RestartCurrentLevel()
        {
            Time.timeScale = 1.0f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void GoToMainMenu()
        {
            Time.timeScale = 1.0f;
            SceneManager.LoadScene(0);
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
            timer.gameObject.SetActive(false);
            Time.timeScale = 0.0f;
            OnGameOverEvent(this.team);
            Commons.Log($"Game Over. Team {this.team} win!!!");
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