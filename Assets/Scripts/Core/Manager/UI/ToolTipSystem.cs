using UnityEngine;
using Guinea.Core;

namespace Guinea.UI
{
    public class ToolTipSystem : MonoBehaviour, IManager
    {
        [SerializeField]
        private ToolTip toolTip;
        public ManagerStatus status {get;private set;}

        void Awake()
        {
            toolTip.gameObject.SetActive(false);
        }
        public void Initialize()
        {
            status = ManagerStatus.Initialized;
        }

        public void Show(string content, string header)
        {
            toolTip.SetText(content, header);
            toolTip.gameObject.SetActive(true);
        }

        public void Hide()=>toolTip.gameObject.SetActive(false);
    }
}