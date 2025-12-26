using System.Collections;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public const int effectCount = 15;     //엠펙트 스폰 갯수
    public const int blockSpawnCount = 20; //블럭 스폰 갯수
    public const float maxBottom = -1.5f;    //블럭 시작 위치

    public static GameManager Instance { get; private set; }
    public bool gameOver { get; private set; }
    public bool canTouch { get; private set; } = true;

    [SerializeField] private Block[] blocks;
    [SerializeField] private BlockEffect[] effects;
    [SerializeField] private SpriteRenderer crosshair;

    private int comboCount;
    private int effectIndex;
    private Block.Type previousBlockType;

#if UNITY_EDITOR
    private void Reset()
    {
        ResetObject();
        ResetBlocks();
        ResetEffects();
        ResetCrossHair();
    }

    private void ResetObject()
    {
        this.name = typeof(GameManager).Name;
        this.transform.position = Vector3.zero;
    }

    private void ResetBlocks()
    {
        blocks = GetComponentsInChildren<Block>(true);

        if (blocks.Length == 0)
        {
            var path = Path.Combine("Prefabs", "Block");
            var load = Resources.Load<Block>(path);

            blocks = new Block[blockSpawnCount];

            var nextPos = Vector3.zero;
            nextPos.y = maxBottom;

            for (int i = 0; i < blockSpawnCount; i++)
            {
                var block = Instantiate(load);
                block.name = $"{load.gameObject.name} {i}";

                block.transform.position = nextPos;
                block.transform.SetParent(this.transform);
                nextPos.y += block.transform.localScale.y;

                blocks[i] = block;
            }
        }
    }

    private void ResetEffects()
    {
        effects = this.GetComponentsInChildren<BlockEffect>(true);
        if (effects.Length != 0) return;

        effects = new BlockEffect[effectCount];

        var path = Path.Combine("Prefabs", "BreakBlockEffect");
        var load = Resources.Load<BlockEffect>(path);

        for (int i = 0; i < effectCount; i++)
        {
            effects[i] = Instantiate(load);
            effects[i].name = $"{load.gameObject.name} {i}";

            effects[i].transform.SetParent(this.transform);
            effects[i].gameObject.SetActive(false);
        }
    }

    private void ResetCrossHair()
    {
        crosshair = this.TryGetChildComponent<SpriteRenderer>("CrossHairLine");
        if (crosshair) return;

        var path = Path.Combine("Prefabs", "CrossHairLine");
        var load = Resources.Load<SpriteRenderer>(path);

        crosshair = Instantiate(load);

        crosshair.transform.position = Vector3.up * maxBottom;
        crosshair.name = load.gameObject.name;
        crosshair.transform.SetParent(this.transform);
    }
#endif

    private void Awake()
    {
        Instance = this;
        InitDate();
    }

    private void InitDate()
    {
        Json.SetCombo(0);
        Json.SetPlayPoint(0);
    }

    /// <summary>
    /// 플레이어 블록 1칸 이동
    /// </summary>
    /// <param name="_value"></param>
    public void Touch(bool _isLeft)
    {
        if (!canTouch || gameOver) return;
        canTouch = false;

        var direction = _isLeft ? Block.Type.Left : Block.Type.Right;
        var isBreak = blocks[0].CanBreak(direction);

        if (isBreak)
        {
            Json.SetPlayPoint(Json.GetPlayPoint() + 1);

            UiManager.Get<PointUi>().UpPoint();
            UiManager.Get<TimerUi>().UpTimer();

            OnEffect(_isLeft);
            SetNextBlock();
        }

        else
        {
            GameOver();
        }
    }

    /// <summary>
    /// 게임 종료
    /// </summary>
    public void GameOver()
    {
        if (gameOver) return;
        if (FirebaseManager.connect) Json.SaveMaxPoint();

        gameOver = true;
        crosshair.color = Color.red;

        UiManager.Get<TimerUi>().StopTimer();
        StartCoroutine(GameOverTimer());
    }

    private IEnumerator GameOverTimer()
    {
        yield return CoroutineManager.Wait(1.5f);
        UiManager.Get<FadeUi>().FadeIn(0.5f, ChangeResultScene);
    }

    private void ChangeResultScene()
    {
        SceneChangeManager.Change(SceneChangeManager.SceneName.Result);
    }

    private void SetNextBlock()
    {
        var lastIndex = blocks.Length - 1;
        var lastBlockPos = blocks[lastIndex].transform.position;
        lastBlockPos.y += blocks[lastIndex].transform.localScale.y;

        var currentBlockType = blocks[0].BlockType();

        //회피 콤보
        if ((previousBlockType == Block.Type.Left && currentBlockType == Block.Type.Right) ||
            (previousBlockType == Block.Type.Right && currentBlockType == Block.Type.Left))
        {
            UiManager.Get<ComboUi>().Show(comboCount);
            UiManager.Get<ComboUi>().Fade();

            CamController.Instatnce.Shake(0.15f, 0.1f);
            comboCount++;

            if (Json.GetCombo() < comboCount) Json.SetCombo(comboCount);
        }

        else
        {
            comboCount = 0;
        }

        blocks[0].BreakBlock(lastBlockPos);
        previousBlockType = currentBlockType;

        BlockSum();
        MoveBlock();
    }

    private void BlockSum()
    {
        var tempBlock = blocks[0];
        var length = blocks.Length - 1;

        for (int i = 0; i < length; i++)
        {
            blocks[i] = blocks[i + 1];
        }

        blocks[length] = tempBlock;
    }

    private void OnEffect(bool _isLeft)
    {
        effectIndex++;
        if (effects.Length <= effectIndex) effectIndex = 0;

        effects[effectIndex].OnEffect(blocks[0], _isLeft);
    }

    private void MoveBlock()
    {
        var speed = 14f;

        BlockSpeed(speed);
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        var currentBlock = blocks[0].transform;

        while (!canTouch && !gameOver)
        {
            var blockPos = currentBlock.position;

            if (blockPos.y < maxBottom)
            {
                BlockStop();
                canTouch = true;
            }

            yield return null;
        }
    }

    private void BlockSpeed(float _speed)
    {
        for (int i = 0; i < blocks.Length; i++)
        {
            blocks[i].MoveBlock(_speed);
        }
    }

    private void BlockStop()
    {
        var nextPos = this.transform.position;
        nextPos.y = maxBottom;

        for (int i = 0; i < blocks.Length; i++)
        {
            blocks[i].MoveBlock(0f);
            blocks[i].transform.position = nextPos;
            nextPos.y += blocks[i].transform.localScale.y;
        }
    }
}
