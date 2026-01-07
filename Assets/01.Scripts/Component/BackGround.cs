using DG.Tweening;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    [SerializeField] private bool isFront;

#if UNITY_EDITOR
    private void Reset()
    {
        isFront = this.name.Contains("Front");
    }
#endif

    private void Awake()
    {
        
    }

    private void NextMovePos()
    {

    }

    private void OnDestroy()
    {
        this.transform.DOKill();
    }
}
