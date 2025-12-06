using UnityEngine;

public class MainUi : UiBase
{
#if UNITY_EDITOR
    private void Reset()
    {
        SetName<MainUi>();
    }
#endif
}
