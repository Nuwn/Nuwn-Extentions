using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour
{
    public static T Instance { get; private set; }
    private void Awake()
    {
        Instance = default;
        if (Instance == null)
            Instance = (T)(object)this;
        else
            Destroy(this);
    }
    private void OnDestroy()
    {
        if (Equals(Instance, this))
            Instance = default;
    }
}

