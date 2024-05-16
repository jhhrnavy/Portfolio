using UnityEngine;

public class StaticInstance<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; set; }
    protected virtual void Awake()
    {
        if(Instance == null)
            Instance = this as T;
    }
    protected virtual void OnApplicationQuit()
    {
        Instance = null;
        Destroy(gameObject);
    }
}

public abstract class Singleton<T> : StaticInstance<T> where T : MonoBehaviour
{
    protected override void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        base.Awake();
    }
}

// Persistent version of the singleton. audio sources, persistent data, stateful
public abstract class PersistentSingleton<T> : Singleton<T> where T : MonoBehaviour 
{
    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(gameObject);
    }
}
//public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
//{
//    private static T instance;

//    public static T Instance
//    {
//        get
//        {
//            if (null == instance)
//            {
//                GameObject obj;
//                obj = GameObject.Find(typeof(T).Name);
//                if (obj == null)
//                {
//                    obj = new GameObject(typeof(T).Name);
//                    instance = obj.AddComponent<T>();
//                }
//                else
//                {
//                    instance = obj.GetComponent<T>();
//                }
//            }
//            return instance;
//        }
//    }

//    private void Awake()
//    {
//        if (instance != null) 
//            Destroy(gameObject);
//        else 
//            instance = this as T;

//        DontDestroyOnLoad(gameObject);
//    }


//}
