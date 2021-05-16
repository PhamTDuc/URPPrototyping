using UnityEngine;
using Guinea.Core;

namespace Guinea.UI
{
    public class TestMenu : MonoBehaviour
    {
        [SerializeField]
        MenuController menuController;
#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.F))
            {
                menuController.Open(MenuType.Loading);
            }
            if (Input.GetKeyUp(KeyCode.G))
            {
                menuController.Close(MenuType.Loading);
            }
            if (Input.GetKeyUp(KeyCode.H))
            {
                menuController.Close(MenuType.Loading, MenuType.MainMenu);
            }
            if (Input.GetKeyUp(KeyCode.J))
            {
                menuController.Close(MenuType.Loading, MenuType.MainMenu, true);
            }
        }
#endif
    }
}