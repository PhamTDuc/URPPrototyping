using UnityEngine;
using UnityEngine.SceneManagement;
using Guinea.Core;
using System;

namespace Guinea.UI
{
    public class ToggleGamePaused : MonoBehaviour
    {
        [SerializeField]
        private GameObject toggleGamePauseMenu;
        private bool isGamePaused = false;
        void Awake()
        {
            Commons.Assert(toggleGamePauseMenu != null, "ToggleGamePauseMenu should not be null");
            toggleGamePauseMenu.SetActive(false);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && MasterManager.GetLevelManager().gameStatus == GameStatus.GAME_RUNNING)
            {
                Toggle();
            }
        }

        private void Toggle()
        {
            if (isGamePaused)
            {
                OnResume();
            }
            else
            {
                OnPause();
            }
            isGamePaused = !isGamePaused;
        }

        private void OnPause()
        {
            Commons.Log("Game Paused");
            Time.timeScale = 0.0f;
            toggleGamePauseMenu.SetActive(true);
        }

        private void OnResume()
        {
            Commons.Log("Game Resumed");
            Time.timeScale = 1.0f;
            toggleGamePauseMenu.SetActive(false);
        }

        public void Resume()
        {
            Commons.Log("Resume in GamePaused called");
            Toggle();
        }
        public void Restart()
        {
            Commons.Log("Restart in GamePaused");
            MasterManager.GetLevelManager().RestartCurrentLevel();
        }

        public void MainMenu()
        {
            Commons.Log("Call MainMenu from GamePaused^^");
            MasterManager.GetLevelManager().GoToMainMenu();
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}