using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour
{
    public static T Current { get; private set; }
    private void Awake()
    {
        Current = default;
        if (Current == null)
            Current = (T)(object)this;
        else
            Destroy(this);
    }
    private void OnDestroy()
    {
        if (Equals(Current, this))
            Current = default;
    }
}

