using System.Collections;
using UnityEngine;

public class Effect : MonoBehaviour
{
    [Header("비활성화시킬 시간"), SerializeField] protected float timer = 0.1f;
    private Coroutine coroutine;

    protected virtual void OnEnable()
    {
        StartTimer();
    }

    protected void StartTimer()
    {
        if (coroutine != null) StopCoroutine(coroutine);
        coroutine = StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        yield return CoroutineManager.Wait(timer);
        this.gameObject.SetActive(false);
    }
}
