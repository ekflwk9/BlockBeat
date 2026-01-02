using UnityEngine;

public class BlockEffect : Effect
{
    private const float power = 17f;

    [SerializeField] private SpriteRenderer render;
    [SerializeField] private Rigidbody2D rigid;

#if UNITY_EDITOR
    private void Reset()
    {
        render = this.RequireComponent<SpriteRenderer>();
        render.sortingOrder = 4;

        rigid = this.RequireComponent<Rigidbody2D>();
        rigid.mass = 3f;
        rigid.gravityScale = 5f;
    }
#endif

    public void OnEffect(Block _block, bool _isLeft)
    {
        render.sprite = _block.render.sprite;
        this.transform.position = _block.transform.position;

        if (!this.gameObject.activeSelf) this.gameObject.SetActive(true);
        else StartTimer();

        var direction = Vector2.zero;
        direction.x = _isLeft ? power : -power;

        var verticalPower = power * 0.5f;
        direction.y = Random.Range(0f, verticalPower);

        rigid.linearVelocity = direction;
    }
}
