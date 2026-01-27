using DG.Tweening;
using UnityEngine;

public class TitleMoving : MonoBehaviour
{
    private const float moveSpeed = 1.3f;
    [SerializeField] private RectTransform rect;

#if UNITY_EDITOR
    private void Reset()
    {
        rect = this.TryGetComponent<RectTransform>();
    }
#endif

    private void Awake()
    {
        var movePos = rect.anchoredPosition.y - 7f;
        var tween = rect.DOAnchorPosY(movePos, moveSpeed);
        tween.SetLoops(-1, LoopType.Yoyo);
        tween.SetEase(Ease.Linear);
    }

    private void OnDestroy()
    {
        rect.DOKill();
    }
}
