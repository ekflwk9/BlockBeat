using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AddPopupUi : UiBase
{
    private const int maxTextCount = 10; //최대 입력 가능 글자 수

    [SerializeField] private TMP_Text warningText;
    [SerializeField] private TMP_InputField input;
    [SerializeField] private Image buttonLine;


#if UNITY_EDITOR
    private void Reset()
    {
        SetName<AddPopupUi>();

        warningText = this.TryGetChildComponent<TMP_Text>("WarningText");
        buttonLine = this.TryGetChildComponent<Image>("ButtonLine");
        input = this.TryGetChildComponent<TMP_InputField>();
    }
#endif

    private void OnDestroy()
    {
        buttonLine.DOKill();
        warningText.DOKill();
    }

    public override void On()
    {
        if (string.Equals(Json.defaultName, Json.GetName()))
        {
            base.On();
            InitStartLine();
        }

        else
        {
            base.Off();
            FirebaseManager.AddRank();
        }
    }

    private void InitStartLine()
    {
        var tween = buttonLine.DOFade(0f, 0.5f);
        tween.SetLoops(-1, LoopType.Yoyo);
        tween.SetEase(Ease.Linear);
    }

    private void OnWarning(string _text)
    {
        warningText.text = _text;
        warningText.color = Color.red;

        warningText.DOKill();
        warningText.DOFade(0f, 5f);
    }

    public bool CanChangeName()
    {
        var nickName = input.text;

        //공백 검사
        if (string.IsNullOrWhiteSpace(nickName) || nickName.Contains(" "))
        {
            OnWarning("No spaces are allowed, and at least one character must be entered.");
            return true;
        }

        //글자수 제한 검사
        else if (maxTextCount <= nickName.Length)
        {
            OnWarning($"You cannot exceed {maxTextCount} characters.");
            return true;
        }

        var enumer = FirebaseManager.value.GetEnumerator();

        //중복 검사
        while (enumer.MoveNext())
        {
            if (string.Equals(nickName, enumer.Current.Key))
            {
                OnWarning("An identical name already exists.");
                return true;
            }
        }

        return false;
    }

    public string InputName()
    {
        return input.text;
    }
}
