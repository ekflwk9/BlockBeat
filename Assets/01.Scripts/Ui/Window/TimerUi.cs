using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TimerUi : UiBase
{
    [SerializeField] private Image slide;

    private bool gameOver;
    private float score;

#if UNITY_EDITOR
    private void Reset()
    {
        SetName<TimerUi>();

        var canvas = this.TryGetComponent<Canvas>();
        if (canvas) canvas.sortingOrder = 11;

        slide = this.TryGetChildComponent<Image>("Slide");
    }
#endif

    /// <summary>
    /// 타이머 시간 점점 빨라지게
    /// </summary>
    public void UpTimer()
    {
        if (this.gameObject.activeSelf) StartCoroutine(Timer());
        if (score < 0.5f) score += 0.01f;
    }

    /// <summary>
    /// 타이머 정지
    /// </summary>
    public void StopTimer()
    {
        gameOver = true;
    }

    private void ResetSlide()
    {
        slide.fillAmount = 1f;
        slide.color = Color.white;
    }

    private IEnumerator Timer()
    {
        if (gameOver) yield return null;
        ResetSlide();

        while (!gameOver)
        {
            slide.fillAmount -= (score * 0.01f) * Time.smoothDeltaTime;
            if (slide.fillAmount < 0.4f) slide.color = Color.red;
            yield return null;
        }
    }
}
