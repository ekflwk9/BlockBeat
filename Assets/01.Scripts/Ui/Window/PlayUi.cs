using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayUi : UiBase, IPointerClickHandler
{
    private bool isStart;

#if UNITY_EDITOR
    private void Reset()
    {
        SetName<PlayUi>();

        var image = this.TryGetChildComponent<Image>();
        if (image) image.color = Color.clear;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            OnScoreUi();
            GameManager.Instance?.Touch(true);
        }

        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            OnScoreUi();
            GameManager.Instance?.Touch(false);
        }
    }
#endif

    private void Start()
    {
        UiManager.Off<ScoreUi>();
        UiManager.Off<TimerUi>();
        UiManager.On<ComboUi>();
        UiManager.On<TutorialUi>();

        UiManager.Get<FadeUi>().FadeOut(0.5f);
    }

    private void OnScoreUi()
    {
        if (!isStart)
        {
            isStart = true;

            UiManager.On<ScoreUi>();
            UiManager.On<TimerUi>();
            UiManager.Off<TutorialUi>();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnScoreUi();

        if (eventData.position.x < this.transform.position.x) GameManager.Instance?.Touch(true);
        else GameManager.Instance?.Touch(false);
    }
}
