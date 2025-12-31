using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoUi : UiBase
{
    [SerializeField] private TMP_InputField input;
    [SerializeField] private Image buttonLine;

#if UNITY_EDITOR
    private void Reset()
    {
        SetName<InfoUi>();
        FindUi();
    }

    private void FindUi()
    {
        input = this.TryGetChildComponent<TMP_InputField>();
        buttonLine = this.TryGetChildComponent<Image>("ButtonLine");
    }
#endif

    private void Start()
    {
        InitStartLine();
    }

    private void OnDestroy()
    {
        buttonLine.DOKill();
    }

    private void InitStartLine()
    {
        var tween = buttonLine.DOFade(0f, 0.5f);
        tween.SetLoops(-1, LoopType.Yoyo);
        tween.SetEase(Ease.Linear);
    }

    public bool SameName()
    {
        var enumer = FirebaseManager.value.GetEnumerator();
        var nickName = input.text;

        while (enumer.MoveNext())
        {
            var key = enumer.Current.Key;
            if (string.Equals(nickName, key)) return true;
        }

        return false;
    }

    public string InputName()
    {
        return input.text;
    }
}
