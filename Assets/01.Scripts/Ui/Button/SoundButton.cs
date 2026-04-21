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
    [SerializeField] private GameObject prohibition;

#if UNITY_EDITOR
    private void Reset()
    {
        prohibition = this.TryFindChild("Prohibition");
        if (prohibition) prohibition.SetActive(false);
    }
#endif

    private void Start()
    {
        SetIcon();
    }

    private void SetIcon()
    {
        var isOn = type == SoundType.Effect ? Json.GetEffectSound() : Json.GetMusicSound();
        prohibition.SetActive(!isOn);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (type == SoundType.Music) SoundManager.SetMusicVolume();
        else SoundManager.SetEffectVolume();

        SetIcon();
        base.OnPointerDown(eventData);
    }
}
