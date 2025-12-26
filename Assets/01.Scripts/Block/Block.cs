using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Block : MonoBehaviour
{
    public struct Info
    {
        public readonly Sprite idle, left, right;

        public Info(Sprite _idle, Sprite _left, Sprite _right)
        {
            idle = _idle;
            left = _left;
            right = _right;
        }
    }

    public enum Name
    {
        NormalBlock,
    }

    public enum Type
    {
        None,
        Left,
        Right,
    }

    private const int maxLevel = 150; //해당 레벨 이후부터 None블록은 나오지 않음

    [field: SerializeField] public SpriteRenderer render { get; private set; }
    [SerializeField] private Rigidbody2D rigid;

    private Type currentType;
    private Dictionary<Type, Sprite> image = new(capacity: Enum.GetValues(typeof(Type)).Length);

#if UNITY_EDITOR
    private void Reset()
    {
        render = this.RequireComponent<SpriteRenderer>();
        render.sortingOrder = 5;

        rigid = this.RequireComponent<Rigidbody2D>();
        rigid.bodyType = RigidbodyType2D.Kinematic;
        rigid.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
    }
#endif

    private void Awake()
    {
        InitBlock();
        BreakBlock(this.transform.position);
    }

    private void InitBlock()
    {
        image.Add(Type.None, LoadSprite("Block"));
        image.Add(Type.Left, LoadSprite("LeftSpike"));
        image.Add(Type.Right, LoadSprite("RightSpike"));
    }

    private Sprite LoadSprite(string _typeName)
    {
        var data = Json.GetMainBlock().ToString();
        var path = Path.Combine("Blocks", data, _typeName);
        var load = Resources.Load<Sprite>(path);

        if (!load) Service.Log($"{data}에 {_typeName}이 로드되지 않음");
        return Instantiate(load);
    }

    /// <summary>
    /// 현재 이미지 변경
    /// </summary>
    /// <param name="_sprite"></param>
    public void ChangeSprite(Info _info)
    {
        image[Type.None] = _info.idle;
        image[Type.Left] = _info.left;
        image[Type.Right] = _info.right;

        render.sprite = image[currentType];
    }

    /// <summary>
    /// 현재 부수는게 가능한 상태인지 검사
    /// </summary>
    /// <param name="_type"></param>
    /// <returns></returns>
    public bool CanBreak(Type _type)
    {
        return currentType != _type;
    }

    /// <summary>
    /// 해당 블록 상태 업데이트
    /// </summary>
    /// <param name="_newPosition"></param>
    public void BreakBlock(Vector3 _newPosition)
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

    /// <summary>
    /// 현재 블록 타입
    /// </summary>
    /// <returns></returns>
    public Type BlockType()
    {
        return currentType;
    }

    private void SetBlokcDirection()
    {
        var minRange = maxLevel < Json.GetPlayPoint() ? (int)Type.Left : (int)Type.None;

        currentType = (Type)UnityEngine.Random.Range(minRange, image.Values.Count);
        render.sprite = image[currentType];
    }
}
