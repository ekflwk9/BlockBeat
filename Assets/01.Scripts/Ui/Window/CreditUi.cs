using TMPro;
using UnityEngine;

public class CreditUi : UiBase
{
    [SerializeField] private TMP_Text info;

#if UNITY_EDITOR
    private void Reset()
    {
        SetName<CreditUi>();

        info = this.TryGetChildComponent<TMP_Text>();
        this.Awake();
    }
#endif

    private void Awake()
    {
        info.text = "";
    }
}
