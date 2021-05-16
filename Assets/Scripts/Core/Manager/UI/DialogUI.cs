using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Guinea.Core;

namespace Guinea.UI
{
   public class DialogUI: MonoBehaviour, IManager
    {
        [SerializeField]
        private TextMeshProUGUI messageDisplay;
        [SerializeField]
        private GameObject canvas;
        [SerializeField]
        private float elapsed;

        private Queue<string> messageQueue = new Queue<string>();

        private string message;
        private bool isRunning = false;

        public ManagerStatus status {get; private set;}

        public void Initialize()=> status = ManagerStatus.Initialized;
      
        void Awake()
        {
            canvas.SetActive(false);
        }

        public DialogUI SetMessage(string message)
        {
            this.message = message;
            return this;
        }

        public void Show()
        {
            messageQueue.Enqueue(message);
            if(!isRunning) StartCoroutine(ShowElapsed());
        }

        private IEnumerator ShowElapsed()
        {
            isRunning = true;
            messageDisplay.text = messageQueue.Dequeue();
            canvas.SetActive(true);
            yield return new WaitForSeconds(1.0f); // Display dialog in seconds
            canvas.SetActive(false);				
            yield return new WaitForSeconds(0.5f); // Display next dialog in seconds
            if (messageQueue.Count>0) yield return StartCoroutine(ShowElapsed());
            isRunning = false;
            yield return null;
        }
    }   
}
