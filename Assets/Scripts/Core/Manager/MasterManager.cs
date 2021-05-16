using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Guinea.UI;

namespace Guinea.Core
{
    // [RequireComponent(typeof(LoaderManager))]
    [RequireComponent(typeof(DialogUI))]
    [RequireComponent(typeof(ToolTipSystem))]
    [RequireComponent(typeof(PoolManager))]
    [RequireComponent(typeof(LoaderManager))]
    [RequireComponent(typeof(LevelManager))]
    public class MasterManager : MonoBehaviour
    {
        private static readonly Dictionary<Type, object> Services = new Dictionary<Type, object>();
        private static List<IManager> _startups = new List<IManager>();

        [SerializeField]
        private static bool debug;
        private static void Register<T>(object service) where T : IManager
        {
            Services[typeof(T)] = service;
            _startups.Add((IManager)service);
        }

        public static T Resolve<T>() where T : IManager
        {
            try
            {
                T service = (T)Services[typeof(T)];
                if (service.status == ManagerStatus.Initialized)
                    return service;
            }
            catch (KeyNotFoundException)
            {
                if (debug) Commons.LogWarning($"MasterManager.cs: Can't Resolve<{typeof(T)}>");
                throw;
            }
            return default;
        }

        public static DialogUI GetDialogUI() => Resolve<DialogUI>();
        public static ToolTipSystem GetToolTip() => Resolve<ToolTipSystem>();
        public static LoaderManager GetLoaderManager() => Resolve<LoaderManager>();
        public static PoolManager GetPoolManager() => Resolve<PoolManager>();
        public static LevelManager GetLevelManager() => Resolve<LevelManager>();
        void Awake()
        {
            #region Register Services
            SettingManager settingManager = new SettingManager();
            LoaderManager loaderManager = GetComponent<LoaderManager>();
            DialogUI dialogUI = GetComponent<DialogUI>();
            ToolTipSystem toolTipSystem = GetComponent<ToolTipSystem>();
            PoolManager poolManager = GetComponent<PoolManager>();
            LevelManager levelManager = GetComponent<LevelManager>();


            Register<SettingManager>(settingManager);
            Register<LoaderManager>(loaderManager);
            Register<DialogUI>(dialogUI);
            Register<ToolTipSystem>(toolTipSystem);
            Register<PoolManager>(poolManager);
            Register<LevelManager>(levelManager);
            #endregion
            StartCoroutine(StartupManagers());
            DontDestroyOnLoad(gameObject);
        }

        // [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        // public static void InitializeOnLoad()
        // {
        //     GetLoaderManager().InitializeOnLoad();
        // }

        void Start()
        {
            Resolve<SettingManager>();
        }

        private IEnumerator StartupManagers()
        {
            foreach (IManager manager in _startups) { manager.Initialize(); }

            int numModules = _startups.Count;
            int numReady = 0;

            while (numReady < numModules)
            {
                int lastReady = numReady;
                numReady = 0;
                foreach (IManager manager in _startups) { if (manager.status == ManagerStatus.Initialized) { numReady++; } }

                if (numReady > lastReady) { if (debug) Commons.Log("Progress: " + numReady + "/" + numModules); }
                yield return null;
            }
            if (debug) Commons.Log("Line 68 MasterManager.cs: All Managers Start Up!!");
        }
    }
}