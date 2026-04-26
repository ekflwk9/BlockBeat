using DG.Tweening;
using UnityEngine;

public class CrossHair : MonoBehaviour
{
    private const float startPos = 0.9f;
    private const float moveSpeed = 0.2f;

    [SerializeField] private Transform left, right;

#if UNITY_EDITOR
    private void Reset()
    {
        this.name = typeof(CrossHair).Name;

        left = this.TryFindChild("LeftArrow").transform;
        if (left) left.transform.position = new Vector3(-startPos, GameManager.maxBottom, 0f);

        right = this.TryFindChild("RightArrow").transform;
        if (right) right.transform.position = new Vector3(startPos, GameManager.maxBottom, 0f);
    }
#endif

    private void OnDestroy()
    {
        left?.DOKill();
        right?.DOKill();
    }

    /// <summary>
    /// 크로스헤어 줌 애니메이션 재생
    /// </summary>
    public void OnAnimtion()
    {
        left.DOKill();
        right.DOKill();

        var newPos = new Vector3(startPos + startPos, GameManager.maxBottom, 0f);
        right.transform.position = newPos;

        newPos.x *= -1f;
        left.transform.position = newPos;

        left.DOMoveX(-startPos, moveSpeed);
        right.DOMoveX(startPos, moveSpeed);
    }
}
