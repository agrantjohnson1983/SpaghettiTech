using System.Collections.Generic;
using UnityEngine;

public class sTapeTool : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;

    [Header("Layers")]
    public LayerMask floorLayer;
    public LayerMask cableLayer;

    [Header("Tape Settings")]
    float tapeRadius = 0.75f;
    public float rayDistance = 10f;


    private bool isTaping;

    private HashSet<sTapeSegment> tapedSegments = new();

    private List<sTapeStrip> tapeStrips = new();

    public GameObject tapeStripPrefab;

    private sTapeStrip currentStrip;


    void Update()
    {
        HandleInput();

        if (isTaping)
        {
            TapeAtCursor();
        }
    }


    void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse Down");

            StartTape();

            isTaping = true;
        }


        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("Mouse Up");

            isTaping = false;

            EndTape();
        }
    }

    void StartTape()
    {
        Debug.Log("START TAPE");

        GameObject obj =
            Instantiate(tapeStripPrefab);


        currentStrip =
            obj.GetComponent<sTapeStrip>();

        currentStrip.transform.position = Vector3.zero;


        Debug.Log("Tape Created " + currentStrip);
    }

    void TapeAtCursor()
    {
        Debug.Log("TapeAtCursor running");

        Ray ray =
            playerCamera.ScreenPointToRay(
                Input.mousePosition
            );


        if (Physics.Raycast(ray, out RaycastHit hit, 50f))
        {
            Debug.Log(
                "Ray hit: " +
                hit.collider.name
            );


            if (currentStrip != null)
            {
                currentStrip.AddPoint(hit.point);
            }


            sTapeSegment segment =
                hit.collider.GetComponentInParent<sTapeSegment>();


            if (segment != null)
            {
                currentStrip.AddSegment(segment);
            }
        }
    }

    void EndTape()
    {
        currentStrip = null;

        Debug.Log("Tape finished");
    }
}