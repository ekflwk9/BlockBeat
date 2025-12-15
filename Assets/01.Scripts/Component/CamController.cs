using DG.Tweening;
using NaughtyAttributes;
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
            InitBackGroundCam();
            InitBackGroundScreen();
        }
    }

    private void InitBackGroundCam()
    {
        var backGroundCam = this.TryGetChildComponent<Camera>("BackGroundCamera");
        if (!backGroundCam) return;

        var rotateSpeed = 100f;
        var newRotate = new Vector3(0, 360f, 0f);

        var tween = backGroundCam.transform.DORotate(newRotate, rotateSpeed, RotateMode.FastBeyond360);
        tween.SetEase(Ease.Linear);
        tween.SetLoops(-1, LoopType.Restart);
    }

    private void InitBackGroundScreen()
    {
        var backGroundScreen = this.TryFindChild("BackGroundScreen");
        if (!backGroundScreen) return;

        var camAngle = cam.fieldOfView * 0.5f;  //현재 각도
        var angleRad = camAngle * Mathf.Deg2Rad;

        var camSize = Vector3.zero;
        camSize.y = Mathf.Abs(this.transform.position.z - 1f) * Mathf.Tan(angleRad);
        camSize.x = camSize.y * cam.aspect;

        backGroundScreen.transform.localScale = camSize * 2f;
        backGroundScreen.transform.position = Vector3.forward;
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
