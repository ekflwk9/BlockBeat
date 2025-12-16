using System.IO;
using UnityEngine;

public class Block : MonoBehaviour
{
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
    private Sprite[] image = new Sprite[3];

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
        var data = Json.GetBlock();

        image[0] = LoadSprite(data, "Block");
        image[1] = LoadSprite(data, "LeftSpike");
        image[2] = LoadSprite(data, "RightSpike");
    }

    private Sprite LoadSprite(string _fileName, string _typeName)
    {
        var path = Path.Combine("Blocks", _fileName, _typeName);
        var load = Resources.Load<Sprite>(path);
        if (!load) Service.Log($"{_fileName}에 {_typeName}이 로드되지 않음");

        return Instantiate(load);
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
        var minRange = maxLevel < Json.GetPlayScore() ? (int)Type.Left : (int)Type.None;
        var ranType = Random.Range(minRange, (int)Type.Right + 1);

        currentType = (Type)ranType;
        render.sprite = image[ranType];
    }
}
