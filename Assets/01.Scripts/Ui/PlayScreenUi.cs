using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayScreenUi : UiBase, IPointerClickHandler
{
    private bool isStart;

#if UNITY_EDITOR
    private void Reset()
    {
        SetName<PlayScreenUi>();

        var image = this.TryGetChildComponent<Image>();
        if (image) image.color = Color.clear;

        //RenderSettings.skybox = newMaterial;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            OnScoreUi();
            GameManager.Instance?.MovePlayer(true);
        }

        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            OnScoreUi();
            GameManager.Instance?.MovePlayer(false);
        }
    }
#endif

    private void Start()
    {
        UiManager.Off<ScoreUi>();
        UiManager.Off<TimerUi>();
        UiManager.On<ComboUi>();
        UiManager.On<TutorialUi>();
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

        if (eventData.position.x < this.transform.position.x) GameManager.Instance?.MovePlayer(true);
        else GameManager.Instance?.MovePlayer(false);
    }
}
