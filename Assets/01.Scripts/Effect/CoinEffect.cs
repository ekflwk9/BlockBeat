using System.Collections;
using UnityEngine;

public class CoinEffect : Effect
{
    [SerializeField] private Rigidbody2D rigid;

    private const float speed = 3.5f;
    private Coroutine coroutine;

#if UNITY_EDITOR
    private void Reset()
    {
        rigid = this.RequireComponent<Rigidbody2D>();
        rigid.bodyType = RigidbodyType2D.Kinematic;
        rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
#endif

    /// <summary>
    /// 임펙트 활성화
    /// </summary>
    /// <param name="_startPos"></param>
    public void OnEffect(Vector3 _startPos)
    {
        transform.position = _startPos;
        this.gameObject.SetActive(true);

        if (coroutine != null) StopCoroutine(coroutine);
        coroutine = StartCoroutine(OnEffect());
    }

    private IEnumerator OnEffect()
    {
        var targetPos = CamController.Instatnce.rightTop;
        rigid.linearVelocity = (targetPos - this.transform.position) * speed;

        while (true)
        {
            var thisPos = this.transform.position;

            if (targetPos.x < thisPos.x || targetPos.y < thisPos.y)
            {
                //uiUpdate
                this.gameObject.SetActive(false);
                break;
            }

            yield return null;
        }
    }
}

/* 트레일 꼬리 초기화
         trail.enabled = false;
         yield return new WaitForFixedUpdate();
         trail.Clear();
         trail.enabled = true;
 */
