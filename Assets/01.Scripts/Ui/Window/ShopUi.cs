using System;
using System.IO;
using TMPro;
using UnityEngine;

public class ShopUi : UiBase
{
    [SerializeField] private BuyButton buyButton;
    [SerializeField] private GameObject lockButton;
    [SerializeField] private ShopSelectButton selectButton;
    [SerializeField] private BlockInventory inventory;
    [SerializeField] private TMP_Text itemTitle;
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private TMP_Text conditionText;
    [SerializeField] private GameObject conditionWindow;
    [SerializeField] private Block.Info[] items;

    private Quest quest = new();
    private int currentIndex = (int)Json.GetMainBlock();

#if UNITY_EDITOR
    private void Reset()
    {
        SetName<ShopUi>();
        FindText();
        FinButton();
        FindInventory();
        LoadSprite();
    }

    private void FindText()
    {
        coinText = this.TryGetChildComponent<TMP_Text>("CoinText");
        itemTitle = this.TryGetChildComponent<TMP_Text>("ItemTitleText");
        conditionText = this.TryGetChildComponent<TMP_Text>("ConditionText");
        conditionWindow = this.TryFindChild("ConditionWindow");
    }

    private void FinButton()
    {
        lockButton = this.TryFindChild("LockButton");
        buyButton = this.TryGetChildComponent<BuyButton>();
        selectButton = this.TryGetChildComponent<ShopSelectButton>();
    }

    private void FindInventory()
    {
        inventory = FindAnyObjectByType<BlockInventory>();
        if (!inventory) Service.Log($"{typeof(BlockInventory).Name}이 씬에 존재하지 않음");
    }

    private void LoadSprite()
    {
        var blockNames = (Block.Name[])Enum.GetValues(typeof(Block.Name));
        items = new Block.Info[blockNames.Length];

        for (int i = 0; i < items.Length; i++)
        {
            var path = Path.Combine("Blocks", blockNames[i].ToString());
            var idle = Resources.Load<Sprite>($"{path}/Block");
            var left = Resources.Load<Sprite>($"{path}/LeftSpike");
            var right = Resources.Load<Sprite>($"{path}/RightSpike");

            items[i] = new Block.Info(idle, left, right);
        }
    }
#endif

    private void Start()
    {
        UiUpdate();
        ChangeBlock();
        CheckQuest();
        coinText.text = Json.GetCoin().ToString("N0");

        UiManager.Add<ShopUi>(this);
        UiManager.Get<FadeUi>().FadeOut(0.3f);
    }

    /// <summary>
    /// 현재 선택된 블록을 메인 블록으로
    /// </summary>
    public void SetMainBlock()
    {
        Json.SetMainBlock((Block.Name)currentIndex);
        selectButton.Select(true);
    }

    /// <summary>
    /// 플레이어 데이터에 현재 선택된 블록 아이템 추가
    /// </summary>
    public void AddBlock()
    {
        Json.AddBlockItem((Block.Name)currentIndex);
        coinText.text = Json.GetCoin().ToString("N0");
    }

    /// <summary>
    /// 페이지 이동
    /// </summary>
    public void Move(bool _isLeft)
    {
        currentIndex = _isLeft ? currentIndex - 1 : currentIndex + 1;

        if (currentIndex < 0) currentIndex = items.Length - 1;
        else if (items.Length <= currentIndex) currentIndex = 0;

        UiUpdate();
        ChangeBlock();
        CheckQuest();
    }

    private void UiUpdate()
    {
        var blockName = (Block.Name)currentIndex;

        itemTitle.text = blockName.ToString();
        selectButton.Select(blockName == Json.GetMainBlock());
    }

    private void ChangeBlock()
    {
        var info = items[currentIndex];

        for (int i = 0; i < inventory.blocks.Length; i++)
        {
            inventory.blocks[i].ChangeSprite(info);
        }
    }

    private void CheckQuest()
    {
        var currentBlock = (Block.Name)currentIndex;
        var complete = quest.Complete(currentBlock);
        var buy = Json.GetBlockItem(currentBlock);

        buyButton.gameObject.SetActive(!buy);
        lockButton.SetActive(!complete);

        var newText = quest.Text(currentBlock);
        conditionText.text = newText;

        conditionWindow.SetActive(!string.IsNullOrEmpty(newText));

        if (!buy) buyButton.SetPrice(quest.Price(currentBlock));
    }
}
