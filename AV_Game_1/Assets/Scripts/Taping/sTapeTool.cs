using System.Collections.Generic;
using UnityEngine;

public class sTapeTool : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;
    public GameObject tapeStripPrefab;

    [Header("Layers")]
    public LayerMask floorLayer;

    public float rayDistance = 100f;

    bool isTaping;

    Vector3 startPoint;
    Vector3 currentPoint;

    sTapeStrip currentStrip;

    public float tapeCheckRadius = 0.75f;

    public LayerMask cableLayer;

    private HashSet<sTapeSegment> previewSegments = new();


    void Update()
    {
        HandleInput();

        if (isTaping)
        {
            Debug.Log("[TAPE] Updating preview");
            UpdatePreview();
        }
            
    }

    void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("[TAPE] Mouse DOWN");
            TryStartTape();
        }


        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("[TAPE] Mouse UP");
            FinishTape();
        }
    }

    void TryStartTape()
    {
        Debug.Log("[TAPE] Mouse down");

        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(ray.origin, ray.direction * 50f, Color.green, 1f);

        if (!Physics.Raycast(ray, out RaycastHit hit, 50f, floorLayer))
        {
            Debug.Log("[TAPE] No floor hit");
            return;
        }

        Debug.Log("[TAPE] Started at " + hit.point);

        startPoint = hit.point;
        currentPoint = hit.point;

        GameObject obj = Instantiate(tapeStripPrefab);

        Debug.Log("[TAPE] Spawned strip");

        currentStrip = obj.GetComponent<sTapeStrip>();

        currentStrip.UpdatePreview(startPoint, currentPoint);

        isTaping = true;
    }

    void UpdatePreview()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit hit, 50f, floorLayer))
        {
            Debug.Log("[TAPE] Preview ray missed");
            return;
        }

        Debug.Log("[TAPE] Preview point: " + hit.point);

        currentPoint = hit.point;

        currentStrip.UpdatePreview(startPoint, currentPoint);

        PreviewCableSegments();
    }

    void FinishTape()
    {
        Debug.Log("[TAPE] FinishTape called");

        if (currentStrip == null)
        {
            Debug.Log("[TAPE] No current strip!");
            return;
        }

        foreach (sTapeSegment segment in previewSegments)
        {
            segment.SetPreviewHighlight(false);
        }

        previewSegments.Clear();

        currentStrip.Commit();

        Debug.Log("[TAPE] Tape committed");

        DetectCableSegments();

        currentStrip = null;
        isTaping = false;
    }

    void PreviewCableSegments()
    {
        HashSet<sTapeSegment> detected = new();


        Vector3 direction = currentPoint - startPoint;

        float distance = direction.magnitude;

        int steps = Mathf.CeilToInt(distance / 0.25f);


        for (int i = 0; i <= steps; i++)
        {
            Vector3 point = Vector3.Lerp(
                startPoint,
                currentPoint,
                i / (float)steps
            );


            Collider[] hits =
                Physics.OverlapSphere(
                    point,
                    tapeCheckRadius
                );


            foreach (Collider hit in hits)
            {
                sTapeSegment segment =
                    hit.GetComponent<sTapeSegment>();


                if (segment != null)
                {
                    detected.Add(segment);
                }
            }
        }


        // Remove old highlights
        foreach (sTapeSegment oldSegment in previewSegments)
        {
            if (!detected.Contains(oldSegment))
                oldSegment.SetPreviewHighlight(false);
        }


        // Add new highlights
        foreach (sTapeSegment segment in detected)
        {
            segment.SetPreviewHighlight(true);
        }


        previewSegments = detected;
    }

    void DetectCableSegments()
    {
        Debug.Log("[TAPE] Starting cable detection");


        Vector3 direction = currentPoint - startPoint;

        float distance = direction.magnitude;

        int steps = Mathf.CeilToInt(distance / 0.25f);


        Debug.Log(
            "[TAPE] Distance: " + distance +
            " Steps: " + steps
        );


        HashSet<sTapeSegment> found = new();


        for (int i = 0; i <= steps; i++)
        {
            Vector3 point =
                Vector3.Lerp(
                    startPoint,
                    currentPoint,
                    i / (float)steps
                );

            Debug.DrawLine(
    point,
    point + Vector3.up * 0.5f,
    Color.red,
    3f
);

            Collider[] hits =
    Physics.OverlapSphere(
        point,
        tapeCheckRadius
    );


            Debug.Log(
                "[TAPE] Sample " + i +
                " hits: " + hits.Length
            );


            foreach (Collider hit in hits)
            {
                Debug.Log(
                    "[TAPE] Collider found: " +
                    hit.name
                );

                Debug.Log(
       "[TAPE] Collider: " + hit.name +
       " Layer: " + LayerMask.LayerToName(hit.gameObject.layer)
   );


                sTapeSegment segment =
                    hit.GetComponentInParent<sTapeSegment>();


                if (segment != null)
                {
                    found.Add(segment);
                }
            }
        }


        Debug.Log(
            "[TAPE] Segments found: " +
            found.Count
        );


        foreach (sTapeSegment segment in found)
        {
            Debug.Log(
                "[TAPE] Applying tape to: " +
                segment.name
            );

            segment.SetTaped(true);
        }
    }
}