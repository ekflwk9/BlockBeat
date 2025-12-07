using UnityEngine;
using UnityEngine.EventSystems;

public class OnWindowButton : ButtonBase
{
    private enum WindowType
    {
        Credit,
        Option,
    }

    [Header("활설화 시킬 창"), SerializeField] private WindowType type;

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        Click();
    }

    private void Click()
    {
        switch (type)
        {
            case WindowType.Credit:
                UiManager.On<CreditUi>();
                break;

            case WindowType.Option:
                UiManager.On<OptionUi>();
                break;
        }
    }
}
