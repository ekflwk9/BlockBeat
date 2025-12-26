using System.IO;
using UnityEngine;

public class BlockInventory : MonoBehaviour
{
    [field: SerializeField] public Block[] blocks { get; private set; }

#if UNITY_EDITOR

    private void Reset()
    {
        SetName();
        ResetBlocks();
    }

    private void SetName()
    {
        this.name = typeof(BlockInventory).Name;
        this.transform.position = Vector3.zero;
    }

    private void ResetBlocks()
    {
        blocks = this.gameObject.GetComponentsInChildren<Block>();

        if (blocks.Length == 0)
        {
            blocks = new Block[GameManager.blockSpawnCount];

            var path = Path.Combine("Prefabs", "Block");
            var load = Resources.Load<Block>(path);

            var nextPos = Vector3.zero;
            nextPos.y = GameManager.maxBottom;

            for (int i = 0; i < GameManager.blockSpawnCount; i++)
            {
                var block = Instantiate(load);
                block.name = $"{load.gameObject.name} {i}";

                block.Remove<BoxCollider2D>();
                block.Remove<Rigidbody2D>();

                block.transform.position = nextPos;
                block.transform.SetParent(this.transform);
                nextPos.y += block.transform.localScale.y;

                blocks[i] = block;
            }
        }
    }
#endif
}
