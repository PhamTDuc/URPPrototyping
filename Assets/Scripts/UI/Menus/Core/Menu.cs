using System.Collections;
using UnityEngine;
using Guinea.Core;

namespace Guinea.UI
{
    public class Menu : MonoBehaviour
    {
        public static readonly string FLAG_ON = "ON";
        public static readonly string FLAG_OFF = "OFF";
        public static readonly string FLAG_NONE = "NONE";
        [SerializeField]
        protected MenuType menuType;
        [SerializeField]
        protected bool useAnimation;
        protected Animator animator;
        public string State { get; private set; }
        public MenuType Type { get { return menuType; } }

        #region Unity Methods
        protected void OnEnable()
        {
            if (useAnimation)
            {
                animator = GetComponent<Animator>();
                Commons.Assert(animator != null, "Use choose to use Animation, however no Animator for this component exists");
            }
        }
        #endregion
        #region Public Methods
        public void Animate(bool isOn)
        {
            if (useAnimation)
            {
                animator.SetBool("isOn", isOn);
                StopCoroutine("AwaitAnimation");
                StartCoroutine(AwaitAnimation(isOn));
            }
            else if (!isOn)
            {
                gameObject.SetActive(false);
            }
        }
        #endregion
        #region Private Methods
        private IEnumerator AwaitAnimation(bool isOn)
        {
            State = isOn ? FLAG_ON : FLAG_OFF;
            while (!animator.GetCurrentAnimatorStateInfo(0).IsName(State))
            {
                yield return null;
            }
            while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            {
                yield return null;
            }
            Commons.Log($"The menu {Type} transition to state {(isOn ? "ON" : "OFF")}");
            State = FLAG_NONE;
            if (!isOn) gameObject.SetActive(false);
        }
        #endregion
    }
}