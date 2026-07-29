using UnityEngine;
using UnityEngine.UI;

public class sEQGraph : MaskableGraphic
{
    public sMixerMinigame mixer;

    public float lineThickness = 4f;

    public Color targetColor = Color.white;
    public Color playerColor = Color.green;

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        if (mixer == null)
            return;

        Rect rect = GetPixelAdjustedRect();

        float left = rect.xMin + 20;
        float right = rect.xMax - 20;

        float bottom = rect.yMin + 20;
        float top = rect.yMax - 20;

        float width = right - left;
        float height = top - bottom;

        Vector2[] points = new Vector2[4];

        points[0] = new Vector2(left, bottom + mixer.player.bass * height);
        points[1] = new Vector2(left + width / 3f, bottom + mixer.player.mid * height);
        points[2] = new Vector2(left + width * 2f / 3f, bottom + mixer.player.treble * height);
        points[3] = new Vector2(right, bottom + mixer.player.gain * height);

        for (int i = 0; i < points.Length - 1; i++)
        {
            DrawLine(vh, points[i], points[i + 1], Color.Lerp(Color.red, Color.green, mixer.GetMixQuality() / 100f));
        }

        Vector2[] targetPoints = new Vector2[4];

        targetPoints[0] = new Vector2(left, bottom + mixer.target.bass * height);
        targetPoints[1] = new Vector2(left + width / 3f, bottom + mixer.target.mid * height);
        targetPoints[2] = new Vector2(left + width * 2f / 3f, bottom + mixer.target.treble * height);
        targetPoints[3] = new Vector2(right, bottom + mixer.target.gain * height);

        for (int i = 0; i < targetPoints.Length - 1; i++)
        {
            DrawLine(vh, targetPoints[i], targetPoints[i + 1], targetColor);
        }

        foreach (Vector2 point in points)
            DrawPoint(vh, point, 5f, Color.Lerp(Color.red, Color.green, mixer.GetMixQuality() / 100f));

        foreach (Vector2 point in targetPoints)
            DrawPoint(vh, point, 5f, targetColor);

        //Rect rect = GetPixelAdjustedRect();

        DrawGrid(vh, rect);
    }

    void Update()
    {
        SetVerticesDirty();
    }

    void DrawLine(VertexHelper vh,
    Vector2 start,
    Vector2 end,
    Color color)
    {
        Vector2 direction = (end - start).normalized;

        Vector2 normal = new Vector2(
            -direction.y,
             direction.x);

        normal *= lineThickness * 0.5f;

        DrawQuad(
            vh,
            start - normal,
            start + normal,
            end + normal,
            end - normal,
            color);
    }

    void DrawGrid(VertexHelper vh, Rect rect)
    {
        float left = rect.xMin + 20;
        float right = rect.xMax - 20;

        float bottom = rect.yMin + 20;
        float top = rect.yMax - 20;

        float width = right - left;
        float height = top - bottom;

        Color gridColor = new Color(1f, 1f, 1f, 0.15f);

        // Horizontal lines
        for (int i = 0; i <= 4; i++)
        {
            float y = bottom + (height / 4f) * i;

            DrawLine(
                vh,
                new Vector2(left, y),
                new Vector2(right, y),
                gridColor);
        }

        // Vertical lines
        for (int i = 0; i <= 3; i++)
        {
            float x = left + (width / 3f) * i;

            DrawLine(
                vh,
                new Vector2(x, bottom),
                new Vector2(x, top),
                gridColor);
        }
    }

    void DrawQuad(VertexHelper vh,
    Vector2 p1,
    Vector2 p2,
    Vector2 p3,
    Vector2 p4,
    Color color)
    {
        int start = vh.currentVertCount;

        UIVertex vert = UIVertex.simpleVert;
        vert.color = color;

        vert.position = p1;
        vh.AddVert(vert);

        vert.position = p2;
        vh.AddVert(vert);

        vert.position = p3;
        vh.AddVert(vert);

        vert.position = p4;
        vh.AddVert(vert);

        vh.AddTriangle(start + 0, start + 1, start + 2);
        vh.AddTriangle(start + 2, start + 3, start + 0);
    }

    void DrawPoint(VertexHelper vh, Vector2 center, float radius, Color color)
    {
        DrawQuad(
            vh,
            center + new Vector2(-radius, -radius),
            center + new Vector2(-radius, radius),
            center + new Vector2(radius, radius),
            center + new Vector2(radius, -radius),
            color);
    }
}