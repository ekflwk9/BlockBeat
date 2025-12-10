using DG.Tweening;
using TMPro;
using UnityEngine;

public class ComboUi : UiBase
{
    [SerializeField] private TMP_Text combo;
    [SerializeField] private SpriteRenderer fade;

    private Color fadeOriginColor;
    private Vector3 startPos;

    private string[] text =
    {
       "Avoided it!",
       "Great job!",
       "That's crazy!",
       "You can't avoid it now.",
       "Are you a pro gamer?",
       "No way!",
    };

#if UNITY_EDITOR
    private void Reset()
    {
        SetName<ComboUi>();

        combo = this.TryGetChildComponent<TMP_Text>();
        fade = this.TryGetChildComponent<SpriteRenderer>();
    }
#endif

    private void Start()
    {
        startPos = combo.transform.position;
        combo.color = Color.clear;

        var screenPos = new Vector3(0.5f, 0.5f);
        var fadePos = CamController.Instatnce.cam.ViewportToWorldPoint(screenPos);
        fadePos.z = 0f;

        fade.transform.position = fadePos;
        fade.transform.localScale = new Vector3(Screen.width, Screen.height);

        fadeOriginColor = fade.color;
        fade.color = Color.clear;
    }

    public void Show(int _score)
    {
        if (text.Length <= _score) _score = text.Length - 1;
        combo.text = text[_score];

        combo.DOKill();
        combo.transform.DOKill();
        combo.color = Color.white;

        var newPos = startPos;
        newPos.x = Screen.width * 0.2f;
        combo.transform.position = newPos;

        var tween = combo.transform.DOMoveX(startPos.x, 0.3f);
        tween.OnComplete(EndMove);
    }

    private void EndMove()
    {
        combo.DOFade(0f, 0.5f);
    }

    public void Fade()
    {
        fade.DOKill();

        fade.color = fadeOriginColor;
        fade.DOFade(0f, 0.5f);
    }
}
