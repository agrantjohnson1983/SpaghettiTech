using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class sLaunchArc : MonoBehaviour
{
    LineRenderer lr;

    public float velocity;
    public float angle;
    public int resolution = 10;

    float g;  // gravity force

    float radianAngle;

    public Transform throwPos;

    private void Awake()
    {

        lr = GetComponent<LineRenderer>();
        g = Mathf.Abs(Physics.gravity.y);

    }

    private void OnEnable()
    {
        //RenderArc();
    }

    private void Update()
    {
        RenderArc();
    }

    void RenderArc()
    {
        lr.positionCount = resolution + 1;
        Vector3[] arcArray = CalculateArcArray();
        lr.SetPositions(arcArray);
        //AdjustToGround(arcArray);

    }


    Vector3[] CalculateArcArray()
    {
        /*
        Vector3[] arcArray = new Vector3[resolution + 1];

        radianAngle = Mathf.Deg2Rad * angle;

        float maxDistance = (velocity * velocity * Mathf.Sin(2 * radianAngle)) / g;

        for (int i = 0; i <= resolution; i++)
        {
            float t = (float)i / (float)resolution;

            arcArray[i] = CalculateArcPoint(t, maxDistance);
        }

        return arcArray;
        */
        Vector3[] arcArray = new Vector3[resolution + 1];
        float radianAngle = Mathf.Deg2Rad * angle;
        //float maxDistance = (velocity * velocity * Mathf.Sin(2 * radianAngle)) / g;
        float maxTime = (2 * velocity * Mathf.Sin(radianAngle)) / g;

        for (int i = 0; i <= resolution; i++)
        {
            float t = (float)i / (float)resolution * maxTime;
            float x = velocity * t * Mathf.Cos(radianAngle);
            float y = velocity * t * Mathf.Sin(radianAngle) - (0.5f * g * t * t);
            arcArray[i] = new Vector3(0, y, x); // Set z-component to 0
        }

        AdjustToGround(arcArray);

        return arcArray;

    }

    Vector3 CalculateArcPoint(float _t, float _maxDistance)
    {
        float x = _t * _maxDistance;

        float y = x * Mathf.Tan(radianAngle) - ((g * x * x) / ( 2 * velocity * velocity * Mathf.Cos(radianAngle) * Mathf.Cos(radianAngle) ));

        Vector3 arcPoint = new Vector3(0, y, x);

        // Check for ground collision and adjust the position if necessary
        RaycastHit hit;
        Ray ray = new Ray(transform.position + arcPoint, Vector3.down);
        if (Physics.Raycast(ray, out hit))
        {
            arcPoint = hit.point - transform.position;
        }

        return arcPoint;

        //return new Vector3(0, y, x);

    }

    void AdjustToGround(Vector3[] arcArray)
    {
        for (int i = resolution; i >= 0; i--)
        {
            Ray ray = new Ray(transform.position + arcArray[i], Vector3.down);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                arcArray[i] = hit.point - transform.position;
                break;
            }
        }
    }

}
