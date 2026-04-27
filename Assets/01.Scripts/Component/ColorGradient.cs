using UnityEngine;
using UnityEngine.UI;

public class ColorGradient : BaseMeshEffect
{
    [SerializeField] private Color topColor = Color.white;
    [SerializeField] private Color bottomColor = Color.black;

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
}