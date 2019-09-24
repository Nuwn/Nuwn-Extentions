using Nuwn;
using UnityEngine;

public abstract class PausableMonoBehaviour : MonoBehaviour, IPausable
{
    protected virtual void Awake()
    {
        GameManager.RegisterPausable(this);
    }
    protected virtual void OnDestroy()
    {
        GameManager.UnRegisterPausable(this);
    }
    public virtual void OnPause(bool paused)
    {
        this.enabled = !paused;
        Debug.Log("paused");
    }
}

public abstract class PausableScriptableObject : ScriptableObject, IPausable
{
    protected virtual void Awake()
    {
        GameManager.RegisterPausable(this);
    }
    protected virtual void OnDestroy()
    {
        GameManager.UnRegisterPausable(this);
    }
    public virtual void OnPause(bool paused) { }
}