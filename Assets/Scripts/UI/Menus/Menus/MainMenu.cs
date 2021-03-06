using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Guinea.Core;

namespace Guinea.UI
{
    public class MainMenu : MenuController
    {
        [SerializeField]
        private Slider slider;
        [SerializeField]
        private TextMeshProUGUI progressText;
        public void NewGame()
        {
            Commons.Log("New Game !!!!");
            StartCoroutine(LoadSceneAsync(1));
        }

        private IEnumerator LoadSceneAsync(int sceneIndex)
        {
            AsyncOperation op = SceneManager.LoadSceneAsync(sceneIndex);
            Open(MenuType.Loading, true);
            while (!op.isDone)
            {
                float progress = Mathf.Clamp01(op.progress / 0.9f);
                slider.value = progress;
                progressText.text = progress.ToString("P1");
                Commons.Log("Progress: " + progress);
                yield return null;
            }
        }
        public void Options()
        {
            Commons.Log("Options !!!!!");
            Open(MenuType.Options);
        }

        public void HowToPlay()
        {
            Commons.Log("How to Play !!!!!");
            Open(MenuType.HowToPlay);
        }

        public void Quit()
        {
            Commons.Log("Quit !!!!");
            Application.Quit();
        }

        public virtual void OnBackKeyEvent()
        {
            Commons.Log("MainMenu call OnBackKeyEvent!!");
            base.OnBackKeyEvent();
        }

    }
}