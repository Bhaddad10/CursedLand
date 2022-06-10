using UnityEngine;

public abstract class IPersistentSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _uniqueInstance;
    public static string firstSceneName;

    public static T Instance
    {
        get { return _uniqueInstance; }
        private set
        {
            if (_uniqueInstance == null)
            {
                _uniqueInstance = value;
                firstSceneName = _uniqueInstance.gameObject.scene.name;
                DontDestroyOnLoad(_uniqueInstance.gameObject);
            }
            else if (_uniqueInstance != value)
            {
                DestroyImmediate(value.gameObject);
                Debug.LogWarning("Singleton duplicado destruido!!");
            }
        }
    }

    // Awake is called when the script instance is being loaded
    protected virtual void Awake()
    {
        if (Instance == null)
            Instance = this as T;
        else
        {
            DestroyImmediate(this.gameObject);
            Debug.LogWarning("Singleton duplicado destruido!!");
        }
    }
}
