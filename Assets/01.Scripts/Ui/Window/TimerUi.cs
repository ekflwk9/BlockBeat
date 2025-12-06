using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TimerUi : UiBase
{
    [SerializeField] private Image slide;

    private Coroutine coroutine;
    private float score;

#if UNITY_EDITOR
    private void Reset()
    {
        SetName<TimerUi>();

        var canvas = this.TryGetComponent<Canvas>();
        canvas.sortingOrder = 11;

        slide = this.TryGetChildComponent<Image>("Slide");
    }
#endif

    /// <summary>
    /// 타이머 시간 점점 빨라지게
    /// </summary>
    public void UpTimer()
    {
        if (this.gameObject.activeSelf) coroutine = StartCoroutine(Timer());
        if (score < 0.5f) score += 0.01f;
    }

    /// <summary>
    /// 타이머 정지
    /// </summary>
    public void StopTimer()
    {
        if (coroutine != null) StopCoroutine(coroutine);
    }

    private IEnumerator Timer()
    {
        slide.fillAmount = 1f;

        while (true)
        {
            slide.fillAmount -= (score * 0.01f) * Time.smoothDeltaTime;
            yield return null;
        }
    }
}
