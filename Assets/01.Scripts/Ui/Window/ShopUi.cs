using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class ShopUi : UiBase
{
    [SerializeField] private TMP_Text infoText;
    [SerializeField] private BlockInventory inventory;

    public Block.Name currentBlockName { get; private set; }
    private Dictionary<Block.Name, string> info = new();
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
        SetInfoText();

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
        }
    }

    private void SetInfoText()
    {
        //var condition = "Condition";
        info.Add(Block.Name.NormalBlock, "NormalBlock"); //test
    }

    /// <summary>
    /// 현재 정보창 글 설정
    /// </summary>
    /// <param name="_text"></param>
    public void SetInfo(Block.Name _blockName)
    {
        if (info.ContainsKey(_blockName))
        {
            currentBlockName = _blockName;
            infoText.text = info[_blockName];
        }
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
