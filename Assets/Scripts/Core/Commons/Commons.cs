#if UNITY_EDITOR
#define DEBUG
#define ASSERT
#endif
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace Guinea.Core
{
    public class Commons
    {
        #region LogWarning
        //-----------------------------------
        //--------------------- Log , warning, 

        [Conditional("DEBUG")]
        public static void Log(object message)
        {
            Debug.Log(message);
        }

        [Conditional("DEBUG")]
        public static void Log(string format, params object[] args)
        {
            Debug.Log(string.Format(format, args));
        }
        [Conditional("DEBUG")]
        public static void LogWarning(object message)
        {
            Debug.LogWarning(message);
        }

        [Conditional("DEBUG")]
        public static void LogWarning(object message, UnityEngine.Object context)
        {
            Debug.LogWarning(message, context);
        }

        [Conditional("DEBUG")]
        public static void LogWarning(UnityEngine.Object context, string format, params object[] args)
        {
            Debug.LogWarning(string.Format(format, args), context);
        }
        #endregion


        [Conditional("DEBUG")]
        public static void WarningUnless(bool condition, object message)
        {
            if (!condition) Debug.LogWarning(message);
        }

        [Conditional("DEBUG")]
        public static void WarningUnless(bool condition, object message, UnityEngine.Object context)
        {
            if (!condition) Debug.LogWarning(message, context);
        }

        [Conditional("DEBUG")]
        public static void WarningUnless(bool condition, UnityEngine.Object context, string format, params object[] args)
        {
            if (!condition) Debug.LogWarning(string.Format(format, args), context);
        }

        #region Assert
        //---------------------------------------------
        //------------- Assert ------------------------

        /// Thown an exception if condition = false
        [Conditional("DEBUG")]
        public static void Assert(bool condition)
        {
            if (!condition) throw new UnityException();
        }

        /// Thown an exception if condition = false, show message on console's log
        [Conditional("DEBUG")]
        public static void Assert(bool condition, string message)
        {
            Debug.Assert(condition, message);
        }

        /// Thown an exception if condition = false, show message on console's log
        [Conditional("DEBUG")]
        public static void Assert(bool condition, string format, params object[] args)
        {
            Debug.Assert(condition, string.Format(format, args));
        }
        #endregion
    }

    public class Utility
    {
        public static ObjectType Validate(ObjectType value, ObjectType minimum, ObjectType limit)
        {
            if ((int)value < (int)minimum) value = minimum;
            if ((int)value >= (int)limit) value = (ObjectType)((int)limit - 1);
            return value;
        }

        public static GameObject CreatePrimitive(PrimitiveType type, Vector3 pos)
        {
            GameObject obj = GameObject.CreatePrimitive(type);
            GameObject.Destroy(obj.GetComponent<Collider>());
            obj.transform.position = pos;
            return obj;
        }

        public static int Filter(GameObject[] buffer, IEnumerable<GameObject> targets, Func<GameObject, bool> pred)
        {
            int count = 0;
            foreach (var obj in targets)
            {
                if (pred(obj)) buffer[count++] = obj;
                if (buffer.Length == count) break;
            }
            return count;
        }

        public static bool Filter(GameObject obj, string layerName = null, string tag = null)
        {
            if (obj == null) return false;
            if (layerName != null && obj.layer != LayerMask.NameToLayer(layerName)) return false;
            if (tag != null && !obj.CompareTag(tag)) return false;
            return true;
        }
    }
}