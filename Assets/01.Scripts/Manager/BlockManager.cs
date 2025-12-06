using System.Collections;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public static BlockManager Instance { get; private set; }
    public int score { get; private set; }
    public bool gameOver { get; private set; }

    private int currentIndex;
    [SerializeField] private int lastIndex;
    [SerializeField] private Block[] blocks;

    private bool isDone = true;
    private PlayerBlock player;

#if UNITY_EDITOR
    private void Reset()
    {
        blocks = GetComponentsInChildren<Block>(true);
        lastIndex = blocks.Length - 1;

        var ground = FindAnyObjectByType<Ground>();
        var nextPos = ground ? ground.transform.position + Vector3.up * 2f : Vector3.zero;

        for (int i = 0; i < blocks.Length; i++)
        {
            blocks[i].transform.position = nextPos;
            nextPos.y += blocks[i].transform.localScale.y;
        }
    }
#endif

    private void Awake()
    {
        Instance = this;
        MoveBlock();
    }

    /// <summary>
    /// 플레이어 블록 추가
    /// </summary>
    /// <param name="_player"></param>
    public void Add(PlayerBlock _player) => player = _player;

    /// <summary>
    /// 플레이어 블록 1칸 이동
    /// </summary>
    /// <param name="_value"></param>
    public void MovePlayer(bool _isLeft)
    {
        if (!isDone || gameOver) return;
        isDone = false;

        var trasform = player.transform;
        var thisPosX = trasform.position.x;
        var nexPosX = 0f;

        if ((thisPosX < 0 && _isLeft) || (0 < thisPosX && !_isLeft)) nexPosX = thisPosX;
        else nexPosX = -thisPosX;

        player.transform.position = new Vector3(nexPosX, 0, 0);
        BreakBlock();
    }

    private void BreakBlock()
    {
        var isLeft = player.transform.position.x < 0f;
        var direction = isLeft ? Block.DirectionType.Left : Block.DirectionType.Right;
        var isBreak = blocks[currentIndex].CanBreak(direction);

        if (isBreak)
        {
            score++;
            Json.PlayerScore(score);
            CamController.Instatnce?.Shake(0.2f, 0.1f);

            UiManager.Get<ScoreUi>().UpScore();
            UiManager.Get<TimerUi>().UpTimer();
            SetNextBlock();
        }

        else
        {
            GameOver();
        }
    }

    /// <summary>
    /// 게임 오버
    /// </summary>
    public void GameOver()
    {
        if (gameOver) return;
        gameOver = true;
        StartCoroutine(GameOverTimer());
        //Json.Save();
    }

    private IEnumerator GameOverTimer()
    {
        yield return CoroutineManager.Wait(1.5f);

        SceneChangeManager.Add(EndLoad);
        UiManager.Get<FadeUi>().FadeIn(0.5f, ChangeResultScene);
    }

    private void ChangeResultScene()
    {
        SceneChangeManager.Change(SceneChangeManager.SceneName.Result);
    }

    private void EndLoad()
    {
        UiManager.Get<FadeUi>().FadeOut(0.5f);
    }

    private void SetNextBlock()
    {
        var lastBlockPos = blocks[lastIndex].transform.position;
        lastBlockPos.y += blocks[lastIndex].transform.localScale.y;
        blocks[currentIndex].SetBlock(lastBlockPos);

        lastIndex = SetIndex(lastIndex);
        currentIndex = SetIndex(currentIndex);

        MoveBlock();
    }

    private void MoveBlock()
    {
        var speed = 10f;

        for (int i = 0; i < blocks.Length; i++)
        {
            blocks[i].MoveBlock(speed);
        }
    }

    /// <summary>
    /// 블록이 땅에 닿았을 경우 모두 정지
    /// </summary>
    public void TouchGround()
    {
        for (int i = 0; i < blocks.Length; i++)
        {
            blocks[i].MoveBlock(0f);
        }

        isDone = true;
    }

    private int SetIndex(int _index)
    {
        _index++;

        if (blocks.Length <= _index) _index = 0;
        return _index;
    }
}
