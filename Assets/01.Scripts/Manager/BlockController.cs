using UnityEngine;

public class BlockController : MonoBehaviour
{
    public static BlockController Instance { get; private set; }

    [SerializeField] private int currentBlockIndex;
    [SerializeField] private int lastBlockIndex;
    [SerializeField] private Block[] blocks;

    private PlayerBlock player;
    private bool isDone = true;

#if UNITY_EDITOR
    private void Reset()
    {
        blocks = GetComponentsInChildren<Block>(true);
        var nextPos = Vector3.zero;

        for (int i = 0; i < blocks.Length; i++)
        {
            blocks[i].transform.position = nextPos;
            nextPos.y += blocks[i].transform.localScale.y;
        }

        currentBlockIndex = 0;
        lastBlockIndex = blocks.Length - 1;
    }
#endif

    private void Awake()
    {
        if (this.Singleton(Instance)) Instance = this;
    }

    /// <summary>
    /// 플레이어 블록 추가
    /// </summary>
    /// <param name="_player"></param>
    public void Add(PlayerBlock _player) => player = _player;

    /// <summary>
    /// 블록이 없어졌을 경우
    /// </summary>
    public void ExitBlock() => isDone = false;

    /// <summary>
    /// 블록이 땅에 닿아 준비를 완료했을 경우
    /// </summary>
    public void EnterBlock() => isDone = true;

    /// <summary>
    /// 플레이어 블록 1칸 이동
    /// </summary>
    /// <param name="_value"></param>
    public void MovePlayer(bool _isLeft)
    {
        if (!isDone) return;

        var trasform = player.transform;
        var nexPos = trasform.position.x;

        if ((nexPos < 0 && _isLeft) || (0 < nexPos && !_isLeft)) return;
        else nexPos = _isLeft ? -trasform.localScale.x : trasform.localScale.x;

        player.transform.position = new Vector3(nexPos + nexPos, 0, 0);
    }

    private void SetBlock()
    {

    }
}
