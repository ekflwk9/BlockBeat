using System.Collections.Generic;

public class Quest
{
    public struct List
    {
        public readonly string text;
        public readonly bool isComplete;
        public readonly int price;

        public List(string _text, bool _complete, int _price)
        {
            text = _text;
            isComplete = _complete;
            price = _price;
        }
    }

    public Dictionary<Block.Name, Quest.List> list = new()
    {
        [Block.Name.Normal] = NoConditions(_price: 500),
        [Block.Name.Graphics] = Point(150, _price: 500),
        [Block.Name.Ice] = NoConditions(_price: 500),
        [Block.Name.Dotory] = NoConditions(_price: 500),
        [Block.Name.Bamboo] = Survival(_timer: 45f, _price: 500),
        [Block.Name.Bbiyak] = Combo(_combo: 6, _price: 500),
        [Block.Name.BobSang] = NoConditions(_price: 500),
        [Block.Name.Warrior] = NoConditions(_price: 500),
    };

    /// <summary>
    /// 현재 구매 조건을 충족 했는지 반환
    /// </summary>
    /// <param name="_blockName"></param>
    /// <returns></returns>
    public bool Complete(Block.Name _blockName)
    {
        if (!list.ContainsKey(_blockName)) return true;
        return list[_blockName].isComplete;
    }

    /// <summary>
    /// 해당 아이템 조건 텍스트 반환
    /// </summary>
    /// <param name="_blockName"></param>
    /// <returns></returns>
    public string Text(Block.Name _blockName)
    {
        if (!list.ContainsKey(_blockName)) return string.Empty;
        return list[_blockName].text;
    }

    /// <summary>
    /// 해당 아이템 가격 반환
    /// </summary>
    /// <param name="_blockName"></param>
    /// <returns></returns>
    public int Price(Block.Name _blockName)
    {
        if (!list.ContainsKey(_blockName)) return 0;
        return list[_blockName].price;
    }

    private static Quest.List NoConditions(int _price = 0)
    {
        return new Quest.List(string.Empty, true, _price);
    }

    private static Quest.List Point(int _point, int _price = 0)
    {
        var complete = _point < Json.GetPlayMaxPoint();
        var color = complete ? "<color=#00FF00>" : "<color=#FF0000>";

        var newText = $"{color}{_point}</color>";
        var questText = $"Over {newText} Points";

        return new Quest.List(questText, complete, _price);
    }

    private static Quest.List Survival(float _timer, int _price = 0)
    {
        var complete = _timer < Json.GetMaxPlayTime();
        var color = complete ? "<color=#00FF00>" : "<color=#FF0000>";

        var newText = $"{color}{_timer.ToString("F2")}</color>";
        var questText = $"Survival time over {newText}s";

        return new Quest.List(questText, complete, _price);
    }

    private static Quest.List Combo(int _combo, int _price = 0)
    {
        var complete = _combo < Json.GetMaxCombo();
        var color = complete ? "<color=#00FF00>" : "<color=#FF0000>";

        var newText = $"{color}{_combo}</color>";
        var questText = $"Over {newText} Combo";

        return new Quest.List(questText, complete, _price);
    }
}
