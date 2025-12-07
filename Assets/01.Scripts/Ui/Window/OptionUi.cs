using UnityEngine;

public class OptionUi : UiBase
{
#if UNITY_EDITOR
    private void Reset()
    {
        SetName<OptionUi>();
    }
#endif
}
