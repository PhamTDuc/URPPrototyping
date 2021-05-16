using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Guinea.Core;

namespace Guinea.UI
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField]
        protected MenuType entryMenu;
        [SerializeField]
        protected Menu[] menus;
        private Dictionary<MenuType, Menu> m_menus = new Dictionary<MenuType, Menu>();
        private Menu currentOpenMenu;
        protected MenuType CurrentMenu => currentOpenMenu.Type;
        #region Unity Methods
        protected void Awake()
        {
            RegisterAllMenus();
            Open(entryMenu);
        }
        #endregion
        #region Public Methods
        public void Open(MenuType menuType, bool closePrev = false)
        {
            if (menuType == MenuType.None) return;
            if (!MenuExists(menuType))
            {
                Commons.LogWarning($"You are trying to turn on a menu {menuType} that has not been registered");
                return;
            }
            if (closePrev && currentOpenMenu != null)
            {
                Close(currentOpenMenu.Type);
            }
            currentOpenMenu = GetMenu(menuType);
            currentOpenMenu.gameObject.SetActive(true);
            currentOpenMenu.Animate(true);
        }

        public void Close(MenuType off, MenuType on = MenuType.None, bool waitForExit = false)
        {
            if (off == MenuType.None) return;
            if (!MenuExists(off))
            {
                Commons.LogWarning($"You are trying to turn off menu has not been registered {off} OFF.");
                return;
            }
            Menu offMenu = GetMenu(off);

            if (offMenu.gameObject.activeSelf)
            {
                offMenu.Animate(false);
            }

            if (on != MenuType.None)
            {
                Menu onMenu = GetMenu(on);
                if (waitForExit)
                {
                    StopCoroutine("WaitOnMenuExit");
                    StartCoroutine(WaitOnMenuExit(offMenu, onMenu));
                }
                else
                {
                    Open(on);
                }
            }
        }
        #endregion
        #region Protected Methods

        protected void RegisterAllMenus()
        {
            foreach (Menu menu in menus)
            {
                RegisterMenu(menu);
            }
        }
        protected void RegisterMenu(Menu menu)
        {
            if (MenuExists(menu.Type))
            {
                Commons.LogWarning($"You are trying to register a menu {menu.Type} that has already been registered. {menu.gameObject.name}");
                return;
            }
            m_menus.Add(menu.Type, menu);
            menu.gameObject.SetActive(false);
            Commons.Log($"Register new menu {menu.Type}");
        }
        protected virtual void OnBackKeyEvent()
        {
            Close(CurrentMenu);
        }
        #endregion
        #region Private Methods
        private IEnumerator WaitOnMenuExit(Menu offMenu, Menu onMenu)
        {
            Commons.Log("WaitOnMenuExits");
            while (offMenu.State != Menu.FLAG_NONE)
            {
                yield return null;
            }
            Open(onMenu.Type);
        }
        private Menu GetMenu(MenuType menuType)
        {
            if (!MenuExists(menuType))
            {
                Commons.LogWarning($"You are trying to get a menu {menuType} that has not been registered.");
                return null;
            }
            return m_menus[menuType];
        }
        private bool MenuExists(MenuType menuType) => m_menus.ContainsKey(menuType);

        #endregion
    }

}