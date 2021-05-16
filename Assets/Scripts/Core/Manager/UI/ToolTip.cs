using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace Guinea.UI
{
    [ExecuteInEditMode]
    public class ToolTip : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI headerField;
        [SerializeField]
        private TextMeshProUGUI contentField;
        [SerializeField]
        private LayoutElement layoutElement;
        [SerializeField]
        private int charWrapLimit;

        private RectTransform rectTransform;

        void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        void Update()
        {
            if (Application.isEditor)
            {
                CheckLength();
            }

            Vector2 pos = Input.mousePosition;

            float pivotX = pos.x / Screen.width;
            float pivotY = pos.y / Screen.height;

            rectTransform.pivot = new Vector2(pivotX, pivotY);
            transform.position = pos;
        }
        public void SetText(string content, string header = null)
        {
            contentField.text = content;
            if (string.IsNullOrEmpty(header))
            {
                headerField.gameObject.SetActive(false);
            }
            else
            {
                headerField.gameObject.SetActive(true);
                headerField.text = header;
            }
            CheckLength();
        }

        private void CheckLength()
        {
            int headerLength = headerField.text.Length;
            int contentLength = contentField.text.Length;
            layoutElement.enabled = (headerLength > charWrapLimit || contentLength > charWrapLimit) ? true : false;
        }
    }
}