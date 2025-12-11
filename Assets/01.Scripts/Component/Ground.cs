using DG.Tweening;
using UnityEngine;

public class Ground : MonoBehaviour
{
    [SerializeField] private SpriteRenderer effect;

#if UNITY_EDITOR
    private void Reset()
    {
        var render = this.RequireComponent<SpriteRenderer>();
        render.sortingOrder = 100;

        effect = this.TryGetChildComponent<SpriteRenderer>("Effect");
        effect.sortingOrder = render.sortingOrder + 1;
        effect.color = Color.clear;

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

    /// <summary>
    /// 터치 Ground 임펙트
    /// </summary>
    public void OnEffect()
    {
        effect.DOKill();
        effect.transform.DOKill();

        var duration = 0.3f;

        var thisScale = this.transform.localScale;
        effect.transform.localScale = thisScale;
        effect.transform.DOScale(thisScale * 1.3f, duration);

        effect.color = Color.white;
        effect.DOFade(0f, duration);
    }
}
