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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            OnScoreUi();
            BlockManager.Instance?.MovePlayer(true);
        }

        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            OnScoreUi();
            BlockManager.Instance?.MovePlayer(false);
        }
    }
#endif

    private void Start()
    {
        UiManager.Off<ScoreUi>();
        UiManager.Off<TimerUi>();
    }

    private void OnScoreUi()
    {
        if (!isStart)
        {
            isStart = true;

            UiManager.On<ScoreUi>();
            UiManager.On<TimerUi>();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnScoreUi();

        if (eventData.position.x < this.transform.position.x) BlockManager.Instance?.MovePlayer(true);
        else BlockManager.Instance?.MovePlayer(false);
    }
}
