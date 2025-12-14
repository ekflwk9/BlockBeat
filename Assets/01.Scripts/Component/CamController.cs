using DG.Tweening;
using System.Collections;
using UnityEngine;

public class CamController : MonoBehaviour
{
    public static CamController Instatnce { get; private set; }
    [field: SerializeField] public Camera cam { get; private set; }
    private Coroutine shakeCoroutine;

#if UNITY_EDITOR
    private void Reset()
    {
        cam = this.TryGetChildComponent<Camera>();
        this.name = $"[Camera]";
    }
#endif

    private void Awake()
    {
        if (this.Singleton(Instatnce))
        {
            Instatnce = this;
            LoopRotate();
        }
    }

    private void LoopRotate()
    {
        var rotateSpeed = 100f;
        var newRotate = new Vector3(0, 360f, 0f);

        var tween = this.transform.DORotate(newRotate, rotateSpeed, RotateMode.FastBeyond360);
        tween.SetEase(Ease.Linear);
        tween.SetLoops(-1, LoopType.Restart);
    }

    /// <summary>
    /// 카메라 쉐이크
    /// </summary>
    /// <param name="_power"></param>
    /// <param name="_timer"></param>
    public void Shake(float _power = 0.1f, float _timer = 0.15f)
    {
        if (shakeCoroutine != null) StopCoroutine(shakeCoroutine);
        shakeCoroutine = StartCoroutine(ShakeTimer(_power, _timer));
    }

    private IEnumerator ShakeTimer(float _power, float _timer)
    {
        var time = 0f;
        var camTransform = cam.transform;

        while (time < _timer)
        {
            time += Time.smoothDeltaTime;

            var ranPos = this.transform.position;
            ranPos.y += Random.Range(-_power, _power);
            camTransform.position = ranPos;

            yield return null;
        }

        cam.transform.position = this.transform.position;
    }
}
