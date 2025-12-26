using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopSlotButton : MonoBehaviour, IPointerClickHandler
{
    private Block.Name blockName;
    private Block.Info info;

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
        //lockSlot.SetActive(!Json.GetBlockItem(_blockName)); //test

        this.transform.localScale = Vector3.one;
        info = new Block.Info(idle, left, right);
    }

    /// <summary>
    /// 선택 취소
    /// </summary>
    public void UnSelect()
    {
        select.SetActive(false);
    }

    /// <summary>
    /// 현재 메인 블록으로 설정
    /// </summary>
    public void Main()
    {
        main.SetActive(true);
    }

    /// <summary>
    /// 현재 메인 블록 취소
    /// </summary>
    public void UnMain()
    {
        main.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!lockSlot.activeSelf && !select.activeSelf)
        {
            select.SetActive(true);

            UiManager.Get<ShopUi>().Select(blockName);
            UiManager.Get<ShopUi>().ChangeBlocks(info);
        }
    }
}
