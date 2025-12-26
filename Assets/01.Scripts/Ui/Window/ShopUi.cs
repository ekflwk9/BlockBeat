using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class ShopUi : UiBase
{
    [SerializeField] private TMP_Text infoText;
    [SerializeField] private BlockInventory inventory;

    private Block.Name currentSelect = Json.GetMainBlock();
    private Dictionary<Block.Name, ShopSlotButton> buttons = new();
    private static Block.Name[] blocks = (Block.Name[])Enum.GetValues(typeof(Block.Name));

#if UNITY_EDITOR
    private void Reset()
    {
        SetName<ShopUi>();

        infoText = this.TryGetChildComponent<TMP_Text>("InfoText");
        inventory = FindAnyObjectByType<BlockInventory>();
        if (!inventory) Service.Log($"{typeof(BlockInventory).Name}이 씬에 존재하지 않음");
    }
#endif

    private void Start()
    {
        SetContent();

        UiManager.Add<ShopUi>(this);
        UiManager.Get<FadeUi>().FadeOut(0.5f);
    }

    private void SetContent()
    {
        var path = Path.Combine("Ui", "ShopSlotButton");
        var load = Resources.Load<ShopSlotButton>(path);
        var spawnPos = this.TryFindChild("Content").transform;

        for (int i = 0; i < blocks.Length; i++)
        {
            var button = Instantiate(load);
            button.transform.SetParent(spawnPos);

            button.InitButton(blocks[i]);
            buttons.Add(blocks[i], button);
        }
    }

    /// <summary>
    /// 현재 메인 블록 설정
    /// </summary>
    public void SetMainBlock()
    {
        buttons[Json.GetMainBlock()].UnMain();
        buttons[currentSelect].Main();
        Json.SetMainBlock(currentSelect);
    }

    /// <summary>
    /// 현재 선택한 블록 설정
    /// </summary>
    /// <param name="_text"></param>
    public void Select(Block.Name _blockName)
    {
        buttons[currentSelect].UnSelect();
        currentSelect = _blockName;
    }

    /// <summary>
    /// 블록 이미지 변경
    /// </summary>
    public void ChangeBlocks(Block.Info _info)
    {
        for (int i = 0; i < inventory.blocks.Length; i++)
        {
            inventory.blocks[i].ChangeSprite(_info);
        }
    }
}
