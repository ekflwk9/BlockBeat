using DG.Tweening;
using TMPro;
using UnityEngine;

public class EvadeUi : UiBase
{
    [SerializeField] private TMP_Text evade;
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
        SetName<EvadeUi>();
        evade = this.TryGetChildComponent<TMP_Text>();
    }
#endif

    private void Start()
    {
        startPos = evade.transform.position;
        evade.color = Color.clear;
    }

    public void Show(int _score)
    {
        if (text.Length <= _score) _score = text.Length - 1;
        evade.text = text[_score];

        evade.DOKill();
        evade.transform.DOKill();
        evade.color = Color.white;

        var newPos = startPos;
        newPos.x = Screen.width * 0.8f;
        evade.transform.position = newPos;

        var tween = evade.transform.DOMoveX(startPos.x, 0.3f);
        tween.OnComplete(EndMove);
    }

    private void EndMove()
    {
        evade.DOFade(0f, 0.5f);
    }
}
