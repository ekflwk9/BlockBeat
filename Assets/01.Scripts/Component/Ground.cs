using UnityEngine;

public class Ground : MonoBehaviour
{
#if UNITY_EDITOR
    private void Reset()
    {
        var render = this.RequireComponent<SpriteRenderer>();
        render.sortingOrder = 100;

        var box  = this.RequireComponent<BoxCollider2D>();
        box.isTrigger = true;

        this.tag = Tag.Ground;
        this.gameObject.layer = Layer.Name.Ground;
    }
#endif

    private void Start()
    {
        var viewportPos = new Vector3(Screen.width * 0.5f, 0f, 0f); //Screen.height은 Top 포지션
        var cam = CamController.Instatnce.cam;
        var worldPos = cam.ScreenToWorldPoint(viewportPos);

        worldPos.z = 0;
        worldPos.y += 0.5f;

        this.transform.position = worldPos;
        this.transform.localScale = new Vector3(Screen.width, 1, 1f);
    }
}
