using UnityEngine;

public class Block : MonoBehaviour
{
#if UNITY_EDITOR
    private void Reset()
    {
        var box = this.RequireComponent<BoxCollider2D>();
        var rigid = this.RequireComponent<Rigidbody2D>();
        rigid.gravityScale = 2f;
    }
#endif
}
