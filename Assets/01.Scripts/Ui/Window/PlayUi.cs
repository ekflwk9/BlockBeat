using System.Collections;
using UnityEngine;

public class PlayUi : UiBase
{
#if UNITY_EDITOR
    private void Reset()
    {
        SetName<PlayUi>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            GameManager.Instance?.Touch(true);
        }

        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            GameManager.Instance?.Touch(false);
        }
    }
#endif

    private void Start()
    {
        InitUi();
        StartCoroutine(StartGame());
    }

    private void InitUi()
    {
        UiManager.Off<PointUi>();
        UiManager.Off<TimerUi>();
        UiManager.On<ComboUi>();
        UiManager.On<TutorialUi>();

        UiManager.Get<FadeUi>().FadeOut(0.5f);
    }

    /// <summary>
    /// 게임을 플레이하는데 필요한 UI 활성화
    /// </summary>
    public IEnumerator StartGame()
    {
        var manager = GameManager.Instance;

        while (true)
        {
            if (!manager.canTouch)
            {
                UiManager.On<PointUi>();
                UiManager.On<TimerUi>();
                UiManager.Off<TutorialUi>();
                break;
            }

            yield return null;
        }
    }
}
