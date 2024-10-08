using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Auto_Singleton<T> : MonoBehaviour
    where T : Component
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                var objs = FindObjectsOfType(typeof(T)) as T[];
                if (objs.Length > 0) { _instance = objs[0]; }

                if (objs.Length > 1) Debug.Log(typeof(T).Name + "more than one");

                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = string.Format("_{0}" + (typeof(T).Name));
                    _instance = obj.AddComponent<T>();
                }
            }
            return _instance;
        }
    }
}
