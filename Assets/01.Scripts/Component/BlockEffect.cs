using DG.Tweening;
using System.IO;
using UnityEngine;

public class BlockEffect : Effect
{
    [SerializeField] private SpriteRenderer render;

#if UNITY_EDITOR
    private void Reset()
    {
        render = this.TryGetChildComponent<SpriteRenderer>();
        if (render) return;

        var path = Path.Combine("Prefabs", "BlockFade");
        var load = Resources.Load<SpriteRenderer>(path);

        render = Instantiate(load);
        render.sortingOrder = 5;
        render.transform.SetParent(this.transform);
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
