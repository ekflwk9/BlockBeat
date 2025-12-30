using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuyButton : ButtonBase
{
    [SerializeField] private TMP_Text priceText;
    private int price;

#if UNITY_EDITOR
    protected override void Reset()
    {
        base.Reset();
        priceText = this.TryGetChildComponent<TMP_Text>("PriceText");
    }
#endif

    /// <summary>
    /// 가격 설정 및 활성화
    /// </summary>
    /// <param name="_coin"></param>
    public void SetPrice(int _coin)
    {
        price = _coin;
        priceText.text = _coin.ToString();
        priceText.color = Json.GetCoin() < _coin ? Color.red : Color.white;

        this.gameObject.SetActive(true);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        var playerCoin = Json.GetCoin();

        if (price <= playerCoin)
        {
            Json.SetCoin(playerCoin - price);
            UiManager.Get<ShopUi>().AddBlock();
            UiManager.Get<FadeUi>().FadeOut(FadeUi.Type.Buy);

            base.OnPointerDown(eventData);
            this.gameObject.SetActive(false);
        }
    }
}
