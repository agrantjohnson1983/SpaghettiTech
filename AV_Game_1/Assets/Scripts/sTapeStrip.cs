using System.Collections.Generic;
using UnityEngine;

public class sTapeStrip : MonoBehaviour
{
    public LineRenderer line;

    public List<sTapeSegment> tapedSegments = new();

    bool initialized = false;


    void Awake()
    {
        if (line == null)
            line = GetComponent<LineRenderer>();

        line.positionCount = 0;
    }


    public void AddPoint(Vector3 point)
    {
        point += Vector3.up * 0.02f;


        // First point
        if (!initialized)
        {
            line.positionCount = 1;
            line.SetPosition(0, point);

            initialized = true;

            return;
        }


        line.positionCount++;

        line.SetPosition(
            line.positionCount - 1,
            point
        );
    }


    public void AddSegment(sTapeSegment segment)
    {
        if (tapedSegments.Contains(segment))
            return;

        tapedSegments.Add(segment);

        segment.SetTaped(true);
    }
}