using UnityEngine;

public class PlayerBlock : MonoBehaviour
{
    private void Awake()
    {
        BlockManager.Instance?.Add(this);
    }
}
