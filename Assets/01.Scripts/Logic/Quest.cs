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
        [Block.Name.Graphics] = Point(150, _price: 500),
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

    private static Quest.List Point(int _point, int _price = 0)
    {
        var questText = "a";
        var complete = _point < Json.GetPlayMaxPoint();

        return new Quest.List(questText, complete, _price);
    }

    private static Quest.List Survival(float _timer, int _price = 0)
    {
        var questText = "b";
        var complete = _timer < Json.GetMaxPlayTime();

        return new Quest.List(questText, complete, _price);
    }

    private static Quest.List Combo(int _combo, int _price = 0)
    {
        var questText = "c";
        var complete = _combo < Json.GetCombo();

        return new Quest.List(questText, complete, _price);
    }
}
