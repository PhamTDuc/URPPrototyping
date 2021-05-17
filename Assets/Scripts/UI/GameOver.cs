using UnityEngine;
using TMPro;
using Guinea.Core;

namespace Guinea.UI
{
    public class GameOver : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI teamName;
        [SerializeField]
        private GameObject nextLevelBtn;
        void Start()
        {
            Commons.Assert(teamName != null, "TeamName could not be null");
            Commons.Assert(nextLevelBtn != null, "nextLevelBtn could not be null");
            MasterManager.GetLevelManager().OnGameOverEvent += OnGameOver;
            MasterManager.GetLevelManager().OnGameOverLose += OnGameOverLose;
            gameObject.SetActive(false); // Hide GameOver when game started
        }

        public void NextLevel()
        {
            Commons.Log("Go to NextLevel^^");
            MasterManager.GetLevelManager().LoadNextLevel();
        }

        public void Resume()
        {
            Commons.Log("Resume this level^^");
            MasterManager.GetLevelManager().RestartCurrentLevel();

        }
        public void MainMenu()
        {
            Commons.Log("GW to MainMenu^^");
            MasterManager.GetLevelManager().GoToMainMenu();
        }

        public void OnGameOver(string name)
        {
            gameObject.SetActive(true);
            nextLevelBtn.SetActive(true);
            teamName.text = "The winner is " + name;
            Commons.Log("Activate GameOver menu");
        }

        public void OnGameOverLose(string name)
        {
            OnGameOver(name);
            nextLevelBtn.SetActive(false);
        }
    }
}