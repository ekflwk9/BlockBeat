using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TimerUi : UiBase
{
    [SerializeField] private Image slide;
    [SerializeField] private CanvasGroup group;

    private float score;

#if UNITY_EDITOR
    private void Reset()
    {
        SetName<TimerUi>();

        var canvas = this.TryGetComponent<Canvas>();
        if (canvas) canvas.sortingOrder = 11;

        group = this.RequireComponent<CanvasGroup>();
        slide = this.TryGetChildComponent<Image>("Slide");
    }
#endif

    public override void On()
    {
        base.On();

        group.alpha = 0f;
        group.DOFade(1f, 0.5f);
    }

    /// <summary>
    /// 타이머 시간 점점 빨라지게
    /// </summary>
    public void UpTimer()
    {
        if (this.gameObject.activeSelf) StartCoroutine(Timer());
        if (score < 0.7f) score += 0.01f;
    }

    private void ResetSlide()
    {
        slide.fillAmount = 1f;
        slide.color = Color.white;
    }

    private IEnumerator Timer()
    {
        var manager = BlockManager.Instance;

        if (!manager || manager.gameOver) yield return null;
        ResetSlide();

        while (!manager.gameOver)
        {
            slide.fillAmount -= (score * 0.01f) * Time.smoothDeltaTime;

            if (slide.fillAmount <= 0f) manager.GameOver();
            else if (slide.fillAmount < 0.5f) slide.color = Color.red;

            yield return null;
        }
    }
}
