using DG.Tweening;
using NaughtyAttributes;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CamController : MonoBehaviour
{
    public static CamController Instatnce { get; private set; }
    [field: SerializeField] public Camera cam { get; private set; }
    public Vector3 rightTop { get; private set; }
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
            InitCamSize();
        }
    }

    private void InitCamSize()
    {
        var camAngle = cam.fieldOfView * 0.5f;  //현재 각도
        var angleRad = camAngle * Mathf.Deg2Rad;

        var camSize = Vector3.zero;
        camSize.y = Mathf.Abs(this.transform.position.z - 1f) * Mathf.Tan(angleRad);
        camSize.x = this.transform.position.x + (camSize.y * cam.aspect);
        camSize.y += this.transform.position.y;

        rightTop = camSize;
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
