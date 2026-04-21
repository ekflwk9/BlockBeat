using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class ButtonBase : MonoBehaviour, IPointerDownHandler
{
    protected const float PunchEffectSpeed = 0.15f;
    protected float originButtonScale;

    protected virtual void Awake() => originButtonScale = this.transform.localScale.x;

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        SoundManager.OnEffect(SoundManager.SoundName.Touch);

        this.transform.DOKill();
        var tween = this.transform.DOScale(originButtonScale * 1.25f, PunchEffectSpeed);
        tween.OnComplete(Revers);
    }

    private void Revers() => this.transform.DOScale(originButtonScale * 1.25f, PunchEffectSpeed);
}

