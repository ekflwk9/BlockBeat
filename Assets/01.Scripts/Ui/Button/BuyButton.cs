using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuyButton : ButtonBase
{
    [SerializeField] private TMP_Text priceText;
    private int price;

#if UNITY_EDITOR
    private void Reset()
    {
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
        priceText.color = Json.PlayerData().coin < _coin ? Color.red : Color.white;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        var playerData = Json.PlayerData();
        var coin = playerData.coin;

        if (price <= coin)
        {
            var newCoin = coin - price;

            if (PlayerData.MaxCoin < newCoin) playerData.coin = PlayerData.MaxCoin;
            else if (newCoin < 0) playerData.coin = 0;
            else playerData.coin  = newCoin;

            UiManager.Get<ShopUi>().AddBlock();
            UiManager.Get<FadeUi>().FadeOut(FadeUi.Type.Buy);
            SoundManager.OnEffect(SoundManager.SoundName.Buy);

            base.OnPointerDown(eventData);
            this.gameObject.SetActive(false);
        }
    }
}
