using UnityEngine;

public class Ground : MonoBehaviour
{
#if UNITY_EDITOR
    private void Reset()
    {
        this.RequireComponent<SpriteRenderer>();
        var box  = this.RequireComponent<BoxCollider2D>();
    }
#endif

    private void Start()
    {
        var viewportPos = new Vector3(Screen.width * 0.5f, 0f, 0f);
        var cam = CamController.Instatnce.cam;
        var worldPos = cam.ScreenToWorldPoint(viewportPos);

        worldPos.z = 0;
        worldPos.y += 0.5f;
        this.transform.position = worldPos;
        this.transform.localScale = new Vector3(100f, 1, 1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Tag.Block)) BlockController.Instance?.EnterBlock();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Tag.Block)) BlockController.Instance?.ExitBlock();
    }
}
