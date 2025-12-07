using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SoundButton : ButtonBase
{
    private enum SoundType
    {
        Music,
        Effect,
    }

    [Header("설정할 사운드 타입"), SerializeField] private SoundType type;

    [Space(10f)]
    [SerializeField] private TMP_Text text;

#if UNITY_EDITOR
    protected override void Reset()
    {
        base.Reset();
        text = this.TryGetChildComponent<TMP_Text>();
    }
#endif

    private void Awake()
    {
        SetText();
    }

    private void SetText()
    {
        if (type == SoundType.Music) text.text = Json.GetMusicSound() ? "On" : "Off";
        else text.text = Json.GetEffectSound() ? "On" : "Off";
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (type == SoundType.Music)  Json.SetMusicSound();
        else Json.SetEffectSound();

        SetText();
        base.OnPointerDown(eventData);
    }
}
