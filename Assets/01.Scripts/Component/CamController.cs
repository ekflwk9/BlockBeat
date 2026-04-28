using System.Collections;
using UnityEngine;

public class CamController : MonoBehaviour
{
    private const float ChangeColorSpeed = 0.15f;

    public static CamController Instatnce { get; private set; }
    [field: SerializeField] public Camera cam { get; private set; }

    [SerializeField] private ColorGradient gradient;

    public Vector3 top { get; private set; }
    private Coroutine shakeCoroutine;

    private float topColorDelta = 0.5f;
    private float bottomColorDelta = 0.5f;

#if UNITY_EDITOR
    private void Reset()
    {
        gradient = this.TryGetChildComponent<ColorGradient>();

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
            InitBackGroundColor();
        }
    }

    private void InitBackGroundColor()
    {
        topColorDelta = Random.Range(0f, 1f);
        var ranColor = Color.HSVToRGB(topColorDelta, 1f, 1f);

        gradient.SetColor(ranColor, Color.purple);
    }

    private void InitCamSize()
    {
        var camAngle = cam.fieldOfView * 0.5f;  //현재 각도
        var angleRad = camAngle * Mathf.Deg2Rad;

        var camSize = Vector3.zero;
        camSize.y = Mathf.Abs(this.transform.position.z) * Mathf.Tan(angleRad);
        //camSize.x = this.transform.position.x - (camSize.y * cam.aspect);
        camSize.x = this.transform.position.x;

        top = camSize;
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
    /// 배경색 변경
    /// </summary>
    /// <param name="_changeColor"></param>
    public void UpColor()
    {
        var frameTime = Time.smoothDeltaTime * ChangeColorSpeed;

        topColorDelta += frameTime;
        topColorDelta = 0.5f <= topColorDelta ? 0.5f : topColorDelta;

        bottomColorDelta += frameTime;
        bottomColorDelta = bottomColorDelta <= 0f ? 0.5f : bottomColorDelta;

        var topColor = Color.HSVToRGB(topColorDelta, 1f, 1f);
        var bottomColor = Color.HSVToRGB(bottomColorDelta, 1f, 1f);

        gradient.SetColor(topColor, bottomColor);
    }
}
