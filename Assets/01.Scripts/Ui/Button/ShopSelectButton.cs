using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopSelectButton : ButtonBase
{
    [SerializeField] private TMP_Text button;
    private bool isSelect;

#if UNITY_EDITOR
    protected override void Reset()
    {
        base.Reset();
        button = this.TryGetChildComponent<TMP_Text>();
    }
#endif

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (!isSelect)
        {
            base.OnPointerDown(eventData);
            UiManager.Get<ShopUi>().SetMainBlock();
        }
    }

    /// <summary>
    /// 현재 버튼 상태 설정
    /// </summary>
    /// <param name="_isSelect"></param>
    public void Select(bool _isSelect)
    {
        isSelect = _isSelect;

        if (_isSelect)
        {
            button.color = Color.green;
            button.text = "Selected";
        }

        else
        {
            button.color = Color.white;
            button.text = "Select";
        }
    }
}
