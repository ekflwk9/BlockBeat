using UnityEngine;

public class PlayerBlock : MonoBehaviour
{
    private void Awake()
    {
        BlockManager.Add(this);
    }
}
