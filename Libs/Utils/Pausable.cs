using Nuwn;
using UnityEngine;

public abstract class PausableMonoBehaviour : MonoBehaviour
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
    }
}

public abstract class PausableScriptableObject : ScriptableObject
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