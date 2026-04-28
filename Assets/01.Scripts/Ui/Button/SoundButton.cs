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
    [SerializeField] private RectTransform onIcon, offIcon;

#if UNITY_EDITOR
    private void Reset()
    {
        onIcon = this.TryGetChildComponent<RectTransform>("OnIcon");
        offIcon = this.TryGetChildComponent<RectTransform>("OffIcon");
    }
#endif

    private void Start()
    {
        SetIcon();
    }

    private void SetIcon()
    {
        var setting = Json.SettingData();
        var isOn = type == SoundType.Effect ? setting.effectSound : setting.musicSound;

        onIcon.gameObject.SetActive(isOn);
        offIcon.gameObject.SetActive(!isOn);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (type == SoundType.Music) SoundManager.SetMusicVolume();
        else SoundManager.SetEffectVolume();

        SetIcon();
        base.OnPointerDown(eventData);
    }
}
