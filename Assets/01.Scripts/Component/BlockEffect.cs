using DG.Tweening;
using UnityEngine;

public class BlockEffect : Effect
{
    [SerializeField] private SpriteRenderer render;

#if UNITY_EDITOR
    private void Reset()
    {
        render = this.TryGetChildComponent<SpriteRenderer>();
        if (!render) return;

        render.sortingOrder = 5;
        render.color = Color.clear;
    }
#endif

    protected override void OnEnable()
    {
        render.color = Color.white;
        render.DOFade(0f, 0.15f);

        base.OnEnable();
    }
}
