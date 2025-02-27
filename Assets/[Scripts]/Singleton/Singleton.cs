using UnityEngine;

/// <summary>
/// Base class for the singleton design pattern.
/// </summary>
public abstract class Singleton<T> : MonoBehaviour where T: MonoBehaviour
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
            }
            return _instance;
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
        }
        else if (_instance != this)
        {
            Debug.LogWarning($"Duplicate instance of {typeof(T)} detected, destroying the new one.");
            Destroy(this);
        }
    }
}
