using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public const int effectCount = 8;       //임펙트 스폰 갯수
    public const int blockSpawnCount = 20;   //블럭 스폰 갯수
    public const float maxBottom = -1.5f;    //블럭 시작 위치
    public const float downSpeed = 16f;      //내려가는 속도

    public static GameManager Instance { get; private set; }
    public bool gameOver { get; private set; }
    public bool canTouch { get; private set; } = true;

    [SerializeField] private CrossHair crossHair;
    [SerializeField] private Block[] blocks;
    [SerializeField] private BlockEffect[] blockEffects;
    [SerializeField] private CoinEffect[] coinEffects;
    [SerializeField] private LightEffect[] lightEffects;

    private int evadeCount;
    private Block.Type previousBlockType;

    private Dictionary<Type, int> effectIndex = new()
    {
        [typeof(BlockEffect)] = 0,
        [typeof(CoinEffect)] = 0,
    };

#if UNITY_EDITOR
    private void Reset()
    {
        ResetObject();
        ResetCrossHair();
        ResetBlocks();
        ResetLightEffects();
        ResetEffects();
        ResetCoinEffect();
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

    private void ResetLightEffects()
    {
        lightEffects = this.GetComponentsInChildren<LightEffect>(true);
        if (lightEffects.Length != 0) return;

        lightEffects = new LightEffect[effectCount];

        var path = Path.Combine("Prefabs", "LightEffect");
        var load = Resources.Load<LightEffect>(path);

        for (int i = 0; i < effectCount; i++)
        {
            lightEffects[i] = Instantiate(load);
            lightEffects[i].name = $"{load.gameObject.name} {i}";

            lightEffects[i].transform.SetParent(this.transform);
            lightEffects[i].gameObject.SetActive(false);
        }
    }

    private void ResetEffects()
    {
        blockEffects = this.GetComponentsInChildren<BlockEffect>(true);
        if (blockEffects.Length != 0) return;

        blockEffects = new BlockEffect[effectCount];

        var path = Path.Combine("Prefabs", $"{typeof(BlockEffect).Name}");
        var load = Resources.Load<BlockEffect>(path);

        for (int i = 0; i < effectCount; i++)
        {
            blockEffects[i] = Instantiate(load);
            blockEffects[i].name = $"{load.gameObject.name} {i}";

            blockEffects[i].transform.SetParent(this.transform);
            blockEffects[i].gameObject.SetActive(false);
        }
    }

    private void ResetCoinEffect()
    {
        coinEffects = this.GetComponentsInChildren<CoinEffect>(true);
        if (coinEffects.Length != 0) return;

        coinEffects = new CoinEffect[effectCount];

        var path = Path.Combine("Prefabs", $"{typeof(CoinEffect).Name}");
        var load = Resources.Load<CoinEffect>(path);

        for (int i = 0; i < effectCount; i++)
        {
            coinEffects[i] = Instantiate(load);
            coinEffects[i].name = $"{load.gameObject.name} {i}";

            coinEffects[i].transform.SetParent(this.transform);
            coinEffects[i].gameObject.SetActive(false);
        }
    }

    private void ResetCrossHair()
    {
        crossHair = this.TryGetChildComponent<CrossHair>();

        if (!crossHair)
        {
            var path = Path.Combine("Prefabs", $"{typeof(CrossHair).Name}");
            var load = Resources.Load<CrossHair>(path);

            crossHair = Instantiate(load);
            crossHair.name = load.name;
            crossHair.transform.SetParent(this.transform);
        }
    }
#endif

    private void Awake()
    {
        Instance = this;
        InitDate();
    }

    private void InitDate()
    {
        Json.SetEvade(0);
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

        if (blocks[0].CanBreak(direction))
        {
            GetCoin(direction);
            crossHair.OnAnimtion();

            CamController.Instatnce.UpColor();
            SoundManager.OnEffect(SoundManager.SoundName.Block);
            Json.SetPlayPoint(Json.GetPlayPoint() + 1);

            UiManager.Get<PointUi>().ShowPoint();
            UiManager.Get<TimerUi>().UpTimer();

            OnBlockEffect(_isLeft);
            SetNextBlock();
        }

        else
        {
            GameOver();
        }
    }

    private void GetCoin(Block.Type _direction)
    {
        if (blocks[0].CanEatCoin(_direction))
        {
            Json.SetCoin(Json.GetCoin() + 1);
            SoundManager.OnEffect(SoundManager.SoundName.Coin);
            OnCoinEffect(_direction);
        }
    }

    /// <summary>
    /// 게임 종료
    /// </summary>
    public void GameOver()
    {
        if (gameOver) return;
        gameOver = true;
        crossHair.gameObject.SetActive(false);

#if !UNITY_EDITOR
        if (FirebaseManager.connect) Json.SaveMaxPoint();
#else
        Json.SaveMaxPoint();
#endif

        SoundManager.OffMusic();
        SoundManager.OnEffect(SoundManager.SoundName.Dead);

        UiManager.Get<FadeUi>().FadeOut(FadeUi.Type.GameOver);
        UiManager.Get<TimerUi>().StopTimer();

        CamController.Instatnce.Shake(0.2f, 0.2f);
        StartCoroutine(GameOverTimer());
    }

    private IEnumerator GameOverTimer()
    {
        yield return CoroutineManager.Wait(1.5f);
        UiManager.Get<FadeUi>().FadeIn(ChangeResultScene);
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
            UiManager.Get<EvadeUi>().Show(evadeCount);
            UiManager.Get<FadeUi>().FadeOut(FadeUi.Type.Evade);

            SoundManager.OnEffect(SoundManager.SoundName.Evade);
            CamController.Instatnce.Shake(0.15f, 0.1f);
            evadeCount++;

            if (Json.GetEvade() < evadeCount) Json.SetEvade(evadeCount);
        }

        else
        {
            evadeCount = 0;
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

    private void OnBlockEffect(bool _isLeft)
    {
        var key = typeof(BlockEffect);
        effectIndex[key]++;
        if (blockEffects.Length <= effectIndex[key]) effectIndex[key] = 0;

        blockEffects[effectIndex[key]].OnEffect(blocks[0], _isLeft);
    }

    private void OnCoinEffect(Block.Type _type)
    {
        var key = typeof(CoinEffect);
        effectIndex[key]++;
        if (coinEffects.Length <= effectIndex[key]) effectIndex[key] = 0;

        var block = blocks[0].transform;
        var spawnPos = block.position;

        if (_type == Block.Type.Right) spawnPos.x += block.localScale.x;
        else spawnPos.x -= block.localScale.x;

        var index = effectIndex[key];
        coinEffects[index].OnEffect(spawnPos);
        lightEffects[index].OnEffect(spawnPos);
    }

    private void MoveBlock()
    {
        BlockSpeed(downSpeed);
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
