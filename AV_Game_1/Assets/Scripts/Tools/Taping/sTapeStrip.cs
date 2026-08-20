using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class sTapeStrip : MonoBehaviour
{
    public LineRenderer line;

    public List<sTapeSegment> tapedSegments = new();

    [Header("Colors")]
    public Color previewColor = Color.yellow;
    public Color tapedColor = Color.black;

    public Vector3 StartPoint { get; private set; }
    public Vector3 EndPoint { get; private set; }

    void Awake()
    {
        line = GetComponent<LineRenderer>();

        line.useWorldSpace = true;
        line.positionCount = 2;
    }

    public void UpdatePreview(Vector3 start, Vector3 end)
    {
        //Debug.Log("Updating preview");

        StartPoint = start;
        EndPoint = end;

        start += Vector3.up * 0.02f;
        end += Vector3.up * 0.02f;

        line.startColor = previewColor;
        line.endColor = previewColor;

        line.material.color = previewColor;

        line.SetPosition(0, start);
        line.SetPosition(1, end);
    }

    public void Commit()
    {
        line.startColor = tapedColor;
        line.endColor = tapedColor;

        line.material.color = tapedColor;
    }
}