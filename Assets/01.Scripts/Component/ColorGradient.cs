using UnityEngine;
using UnityEngine.UI;

public class ColorGradient : BaseMeshEffect
{
    public struct ColorInfo
    {
        public const float ChangeColorSpeed = 0.15f;

        public float maxDelta;
        public float minDelta;
        public float delta;
        public bool plus;
        public Color color;

        public ColorInfo(float _maxDelta = 0f, float _minDelta = 0f, bool _plus = true)
        {
            this.maxDelta = _maxDelta;
            this.minDelta = _minDelta;
            this.delta = 0f;
            this.plus = _plus;
            this.color = Color.white;
        }

        public void NextColor()
        {
            var changeDelta = Time.smoothDeltaTime * ChangeColorSpeed;

            if (plus) delta += changeDelta;
            else delta -= changeDelta;

            if (maxDelta <= delta) plus = false;
            else if (delta <= minDelta) plus = true;

            color = Color.HSVToRGB(delta, 1f, 1f);
        }

        public void SetDelta(float _delta)
        {
            if (maxDelta < _delta) this.delta = maxDelta;
            else if (_delta < minDelta) this.delta = minDelta;
            else this.delta = _delta;

            color = Color.HSVToRGB(delta, 1f, 1f);
        }
    }

    [SerializeField] private ColorInfo top = new(_maxDelta:0.9f, _minDelta:0.6f, _plus: false);
    [SerializeField] private ColorInfo bottom = new(_maxDelta: 0.4f, _minDelta: 0.1f, _plus: true);
    [SerializeField] private Image image;

#if UNITY_EDITOR
    protected override void Reset()
    {
        image = this.TryGetChildComponent<Image>();
    }
#endif

    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive()) return;
        var vertex = new UIVertex();

        for (int i = 0; i < vh.currentVertCount; i++)
        {
            vh.PopulateUIVertex(ref vertex, i);
            var y = vertex.position.y;

            // 위쪽일수록 topColor
            vertex.color = 0f < y ? top.color : bottom.color;
            vh.SetUIVertex(vertex, i);
        }
    }

    /// <summary>
    /// 그라데이션 색깔 지정
    /// </summary>
    /// <param name="_changeColor"></param>
    public void ChangeColor(float _topDelta, float _bottomDelta)
    {
        top.SetDelta(_topDelta);
        bottom.SetDelta(_bottomDelta);

        image?.SetVerticesDirty(); //이미지 다시 그림
    }

    /// <summary>
    /// 다음 그라데이션 색깔로 변경
    /// </summary>
    /// <param name="_changeColor"></param>
    public void ChangeColor()
    {
        top.NextColor();
        bottom.NextColor();

        image?.SetVerticesDirty(); //이미지 다시 그림
    }
}