using UnityEngine;

public class PlayerBlock : MonoBehaviour
{
    private void Awake()
    {
        BlockController.Instance?.Add(this);
    }
}
