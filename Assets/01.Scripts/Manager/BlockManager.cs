using NaughtyAttributes;
using System.Collections;
using System.IO;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public static BlockManager Instance { get; private set; }
    public int score { get; private set; }
    public bool gameOver { get; private set; }

    private int currentIndex;
    [SerializeField] private int lastIndex;
    [SerializeField] private Block[] blocks;
    [SerializeField] private Effect[] effects;

    private int effectIndex;
    private bool isDone = true;
    private PlayerBlock player;

#if UNITY_EDITOR
    private void Reset()
    {
        ResetBlocks();
        ResetEffects();
    }

    private void ResetBlocks()
    {
        blocks = GetComponentsInChildren<Block>(true);

        if (blocks.Length == 0)
        {
            var path = Path.Combine("Prefabs", "Block");
            var load = Resources.Load<Block>(path);

            var ground = FindAnyObjectByType<Ground>();
            var nextPos = ground ? ground.transform.position + Vector3.up * 2f : Vector3.zero;

            var spawnCount = 20;
            blocks = new Block[spawnCount];

            for (int i = 0; i < spawnCount; i++)
            {
                blocks[i] = Instantiate(load);
                blocks[i].transform.position = nextPos;

                blocks[i].transform.SetParent(this.transform);
                nextPos.y += blocks[i].transform.localScale.y;
            }
        }

        lastIndex = blocks.Length - 1;
    }

    private void ResetEffects()
    {
        effects = GetComponentsInChildren<Effect>(true);
        if (effects.Length != 0) return;

        var effectCount = 8;
        effects = new Effect[effectCount];

        var path = Path.Combine("Prefabs", "BreakBlockEffect");
        var load = Resources.Load<Effect>(path);

        for (int i = 0; i < effectCount; i++)
        {
            effects[i] = Instantiate(load);
            effects[i].transform.SetParent(this.transform);
            effects[i].gameObject.SetActive(false);
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
        var direction = isLeft ? Block.Type.Left : Block.Type.Right;
        var isBreak = blocks[currentIndex].CanBreak(direction);

        if (isBreak)
        {
            score++;
            Json.PlayerScore(score);

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

        var currentBlockType = blocks[currentIndex].BlockType();
        blocks[currentIndex].BreakBlock(lastBlockPos);

        lastIndex = SetIndex(lastIndex);
        currentIndex = SetIndex(currentIndex);

        var nextBlock = blocks[currentIndex].BlockType();

        if ((currentBlockType == Block.Type.Left && nextBlock == Block.Type.Right) ||
            (currentBlockType == Block.Type.Right && nextBlock == Block.Type.Left))
            CamController.Instatnce.Shake(0.2f, 0.1f);

        OnEffect();
        MoveBlock();
    }

    private void OnEffect()
    {
        effectIndex++;
        if (effects.Length <= effectIndex) effectIndex = 0;

        var block = blocks[currentIndex].transform;
        var effectPos = block.position;
        effectPos.y -= block.localScale.y;

        effects[effectIndex].transform.position = effectPos;
        effects[effectIndex].gameObject.SetActive(true);
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
