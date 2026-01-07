using UnityEngine;

public class LightEffect : Effect
{
    [SerializeField] private Animator anim;

#if UNITY_EDITOR
    private void Reset()
    {
        anim = this.TryGetComponent<Animator>();
    }
#endif

    /// <summary>
    /// 임펙트 활성화
    /// </summary>
    /// <param name="_spawnPos"></param>
    public void OnEffect(Vector3 _spawnPos)
    {
        this.transform.position = _spawnPos;
        this.gameObject.SetActive(true);
        anim.Play("LightEffect", 0, 0);
    }
}
