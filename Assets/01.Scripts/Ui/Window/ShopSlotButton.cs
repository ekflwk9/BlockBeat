using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopSlotButton : MonoBehaviour, IPointerClickHandler
{
    private Block.Info info;
    private Block.Name blockName;

    [SerializeField] private Image item;
    [SerializeField] private GameObject main;
    [SerializeField] private GameObject select;
    [SerializeField] private GameObject lockSlot;

#if UNITY_EDITOR
    private void Reset()
    {
        item = this.TryGetChildComponent<Image>("Item");
        main = this.TryFindChild("Main");
        select = this.TryFindChild("Select");
        lockSlot = this.TryFindChild("Lock");
    }
#endif

    /// <summary>
    /// 초기화
    /// </summary>
    /// <param name="_blockName"></param>
    public void InitButton(Block.Name _blockName)
    {
        var path = Path.Combine("Blocks", _blockName.ToString());
        var idle = Resources.Load<Sprite>($"{path}/Block");
        var left = Resources.Load<Sprite>($"{path}/LeftSpike");
        var right = Resources.Load<Sprite>($"{path}/RightSpike");

        item.sprite = Instantiate(idle);
        blockName = _blockName;

        main.SetActive(Json.GetMainBlock() == _blockName);
        select.SetActive(Json.GetMainBlock() == _blockName);
        lockSlot.SetActive(!Json.GetBlockItem(_blockName));

        this.transform.localScale = Vector3.one;
        info = new Block.Info(idle, left, right);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!lockSlot.activeSelf && !select.activeSelf)
        {
            select.SetActive(true);

            UiManager.Get<ShopUi>().SetInfo(blockName);
            UiManager.Get<ShopUi>().ChangeBlocks(info);
        }
    }
}
