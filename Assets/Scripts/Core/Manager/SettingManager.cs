using UnityEngine;

namespace Guinea.Core
{
    public class SettingManager : IManager
    {
        public ManagerStatus status { get; private set; }

        public SettingManager()
        {
            status = ManagerStatus.Initialized;
        }
        public void Initialize() { }

        public void Setting()
        {
            Cursor.visible = false;
        }
    }
}