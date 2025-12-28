using DG.Tweening;
using TMPro;
using UnityEngine;

public class ComboUi : UiBase
{
    [SerializeField] private TMP_Text combo;
    private Vector3 startPos;

    private string[] text =
    {
       "Avoided it!",
       "Great job!",
       "That's crazy!",
       "You can't avoid it now.",
       "Are you a pro gamer?",
       "No way!",
       "You dodged that, too?",
       "HaHaHaHa !!",
    };

#if UNITY_EDITOR
    private void Reset()
    {
        SetName<ComboUi>();
        combo = this.TryGetChildComponent<TMP_Text>();
    }
#endif

    private void Start()
    {
        startPos = combo.transform.position;
        combo.color = Color.clear;
    }

    public void Show(int _score)
    {
        if (text.Length <= _score) _score = text.Length - 1;
        combo.text = text[_score];

        combo.DOKill();
        combo.transform.DOKill();
        combo.color = Color.white;

        var newPos = startPos;
        newPos.x = Screen.width * 0.8f;
        combo.transform.position = newPos;

        var tween = combo.transform.DOMoveX(startPos.x, 0.3f);
        tween.OnComplete(EndMove);
    }

    private void EndMove()
    {
        combo.DOFade(0f, 0.5f);
    }
}
