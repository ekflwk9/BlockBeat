using UnityEngine;

public class BackGround : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigid;
    [SerializeField] private bool isFront;
    [SerializeField] private float leftPos;
    [SerializeField] private float rightPos;

    private float moveSpeed = 0.05f;

#if UNITY_EDITOR
    private void Reset()
    {
        rigid = this.RequireComponent<Rigidbody2D>();
        rigid.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
        rigid.bodyType = RigidbodyType2D.Dynamic;
        rigid.gravityScale = 0f;

        isFront = this.name.Contains("Front");
        leftPos = this.transform.position.x + (Vector3.left.x * 0.5f);
        rightPos = this.transform.position.x + (Vector3.right.x * 0.5f);
    }
#endif

    private void Awake()
    {
        var direction = isFront ? Vector2.right : Vector2.left;
        rigid.linearVelocity = direction * moveSpeed;
    }

    private void FixedUpdate()
    {
        CheckPos();
    }

    private void CheckPos()
    {
        var thisPosX = this.transform.position.x;

        if (thisPosX < leftPos) rigid.linearVelocity = Vector2.right * moveSpeed;
        else if (rightPos < thisPosX) rigid.linearVelocity = Vector2.left * moveSpeed;
    }
}
