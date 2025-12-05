using System.Linq;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    public static BlockController Instance { get; private set; }
    [SerializeField] private Block[] blocks;

    private PlayerBlock player;
    private bool isDone = true;

#if UNITY_EDITOR
    private void Reset()
    {
        blocks = GetComponentsInChildren<Block>(true);

        var viewportPos = new Vector3(Screen.width * 0.5f, 0f, 0f);
        var worldPos = Camera.main.ScreenToWorldPoint(viewportPos);
        worldPos.z = 0f;
        blocks[0].transform.position = worldPos;
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
}
