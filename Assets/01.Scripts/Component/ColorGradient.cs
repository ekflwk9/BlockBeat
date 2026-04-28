using UnityEngine;
using UnityEngine.UI;

public class ColorGradient : BaseMeshEffect
{
    [SerializeField] private Color topColor = Color.white;
    [SerializeField] private Color bottomColor = Color.black;
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
            vertex.color = 0f < y ? topColor : bottomColor;
            vh.SetUIVertex(vertex, i);
        }
    }

    /// <summary>
    /// 그라데이션 색깔 지정
    /// </summary>
    /// <param name="_changeColor"></param>
    public void SetColor(Color _topColor, Color _bottomColor)
    {
        topColor = _topColor;
        bottomColor = _bottomColor;

        image?.SetVerticesDirty(); //이미지 다시 그림
    }
}