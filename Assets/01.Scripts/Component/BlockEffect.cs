using DG.Tweening;
using UnityEngine;

public class BlockEffect : Effect
{
    [SerializeField] private SpriteRenderer render;

    private const float moveSpeed = 12f; //이동 속도
    private float direction;

#if UNITY_EDITOR
    private void Reset()
    {
        render = this.TryGetComponent<SpriteRenderer>();
        if (render) render.sortingOrder = 5;
    }
#endif

    private void OnDestroy()
    {
        render.DOKill();
    }

    private void FixedUpdate()
    {
        var movePos = this.transform.position;
        movePos.x += (direction * Time.smoothDeltaTime) * moveSpeed;
        movePos.z = 0f;

        this.transform.position = movePos;
    }

    public void OnEffect(Block _block, bool _isLeft)
    {
        this.gameObject.SetActive(true);
        this.transform.position = _block.transform.position;
        direction = _isLeft ? 1f : -1f;

        render.sprite = _block.render.sprite;
        Fade();
    }

    private void Fade()
    {
        render.DOKill();
        render.color = Color.white;

        render.DOFade(0f, timer * 0.7f);
    }
}
