using UnityEngine;
using UnityEngine.EventSystems;
using Guinea.Core;

namespace UI

{
    public class ToolTipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private string header;
        [SerializeField]
        private string content;
        public void OnPointerEnter(PointerEventData eventData)=> MasterManager.GetToolTip().Show(content, header);
        public void OnPointerExit(PointerEventData eventData) => MasterManager.GetToolTip().Hide();
    }

}