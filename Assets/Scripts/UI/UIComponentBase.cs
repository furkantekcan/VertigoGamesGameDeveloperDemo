using UnityEngine;

public abstract class UIComponentBase : MonoBehaviour
{
#if UNITY_EDITOR
    protected virtual void OnValidate()
    {
        AutoBindReferences();
    }
#endif

    protected abstract void AutoBindReferences();
}