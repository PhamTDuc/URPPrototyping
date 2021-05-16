using UnityEngine;
using TMPro;
using Guinea.Core;

namespace Guinea.UI
{
    public class GameOver : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI teamName;
        void Start()
        {
            Commons.Assert(teamName != null, "TeamName could not be null");
            MasterManager.GetLevelManager().OnGameOverEvent += OnGameOver;
            gameObject.SetActive(false); // Hide GameOver when game started
        }

        public void NextLevel()
        {
            Commons.Log("Go to NextLevel^^");
        }

        public void Resume()
        {
            Commons.Log("Resume this level^^");
        }
        public void MainMenu()
        {
            Commons.Log("GW to MainMenu^^");
            MasterManager.GetLevelManager().GoToMainMenu();
        }

        public void OnGameOver(string name)
        {
            gameObject.SetActive(true);
            teamName.text = "The winner is " + name;
            Commons.Log("Activate GameOver menu");
        }
    }
}