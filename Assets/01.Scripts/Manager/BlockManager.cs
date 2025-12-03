using UnityEngine;
using System.Collections.Generic;

public static class BlockManager
{
    private static List<Block> blocks = new();
    private static PlayerBlock player;

    private static int moveCount;
    private static int moveMaxCount = 2;

    /// <summary>
    /// 플레이어 블록 추가
    /// </summary>
    /// <param name="_player"></param>
    public static void Add(PlayerBlock _player) => player = _player;

    /// <summary>
    /// 장애물 블록 추가
    /// </summary>
    /// <param name="_block"></param>
    public static void Add(Block _block) => blocks.Add(_block);

    /// <summary>
    /// 플레이어 블록 1칸 이동
    /// </summary>
    /// <param name="_value"></param>
    public static void MovePlayer(float _value)
    {
        var trasform = player.transform;
        var nexPos = 0f;
        moveCount += _value < 0 ? -1 : 1;

        if (moveCount < -moveMaxCount) moveCount = -moveMaxCount;
        else if (moveMaxCount < moveCount) moveCount = moveMaxCount;
        else nexPos = _value < 0 ? -trasform.localScale.x : trasform.localScale.x;

        player.transform.position = new Vector3(trasform.position.x + nexPos, 0, 0);
    }
}
