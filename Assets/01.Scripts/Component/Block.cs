using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public enum DirectionType
    {
        None,
        Left,
        Right,
    }

    private Dictionary<DirectionType, GameObject> type = new();
    private DirectionType currentType;

    [SerializeField] private GameObject left;
    [SerializeField] private GameObject right;
    [SerializeField] private Rigidbody2D rigid;

#if UNITY_EDITOR
    private void Reset()
    {
        var box = this.RequireComponent<BoxCollider2D>();
        box.isTrigger = true;

        rigid = this.RequireComponent<Rigidbody2D>();
        rigid.bodyType = RigidbodyType2D.Kinematic;
        rigid.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;

        left = this.TryFindChild("Left");
        right = this.TryFindChild("Right");

        left?.SetActive(false);
        right?.SetActive(false);
    }
#endif

    private void Awake()
    {
        type.Add(DirectionType.Left, left);
        type.Add(DirectionType.Right, right);

        SetBlock(this.transform.position);
    }

    /// <summary>
    /// 현재 부수는게 가능한 상태인지 검사
    /// </summary>
    /// <param name="_type"></param>
    /// <returns></returns>
    public bool CanBreak(DirectionType _type)
    {
        if (!type.ContainsKey(_type)) return true;
        return !type[_type].activeSelf;
    }

    /// <summary>
    /// 해당 블록 상태 업데이트
    /// </summary>
    /// <param name="_newPosition"></param>
    public void SetBlock(Vector3 _newPosition)
    {
        this.transform.position = _newPosition;
        SetBlokcDirection();
    }

    /// <summary>
    /// 블록 아래로 이동
    /// </summary>
    /// <param name="_moveSpeed"></param>
    public void MoveBlock(float _moveSpeed)
    {
        rigid.linearVelocity = Vector2.down * _moveSpeed;
    }

    private void SetBlokcDirection()
    {
        if (type.ContainsKey(currentType)) type[currentType].SetActive(false);

        currentType = (DirectionType)Random.Range((int)DirectionType.None, (int)DirectionType.Right + 1);
        if (type.ContainsKey(currentType)) type[currentType].SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Tag.Ground)) BlockController.Instance.TouchGround();
    }
}
