using DG.Tweening;
using System.Collections;
using UnityEngine;

public class CamController : MonoBehaviour
{
    public static CamController Instatnce { get; private set; }
    [field: SerializeField] public Camera cam { get; private set; }
    [SerializeField] private Color originColor;

    private Tween camColorTween;
    private Coroutine shakeCoroutine;

#if UNITY_EDITOR
    private void Reset()
    {
        cam = this.TryGetChildComponent<Camera>();
        if (cam) originColor = cam.backgroundColor;
        this.name = $"[Camera]";
    }
#endif

    private void Awake()
    {
        if (this.Singleton(Instatnce)) Instatnce = this;
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

    /// <summary>
    /// 특정 Color로 변경 후 다시 원상복귀
    /// </summary>
    /// <param name="_color"></param>
    public void CamBackGroundFade(Color _color, float _timer = 0.2f)
    {
        camColorTween?.Kill();
        cam.backgroundColor = _color;
        camColorTween = DOTween.To(GetCamColor, SetCamColor, originColor, 0.2f);
    }

    private Color GetCamColor() => cam.backgroundColor;
    private void SetCamColor(Color _color) => cam.backgroundColor = _color;
}
