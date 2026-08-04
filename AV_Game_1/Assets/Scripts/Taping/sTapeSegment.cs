using UnityEngine;

public class sTapeSegment : MonoBehaviour
{
    public bool IsTaped => tapeJoint != null;

    Rigidbody rb;

    FixedJoint tapeJoint;
    GameObject tapeAnchor;


    Collider collision;
    MeshRenderer rend;

    public Renderer segmentRenderer;

    Material originalMaterial;

    [Header("Tape Preview")]
    public Material highlightedMaterial;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        collision = GetComponent<Collider>();

        rend = GetComponent<MeshRenderer>();

        if (segmentRenderer == null)
            segmentRenderer = GetComponent<Renderer>();

        if (segmentRenderer != null)
            originalMaterial = segmentRenderer.material;
    }

    public void SetPreviewHighlight(bool active)
    {
        if (segmentRenderer == null)
            return;

        if (active)
        {
            segmentRenderer.material = highlightedMaterial;
        }
        else
        {
            segmentRenderer.material = originalMaterial;
        }
    }


    public void SetTaped(bool taped)
    {
        if (IsTaped == taped)
            return;


        //IsTaped = taped;


        if (taped)
        {
            ApplyTape();
        }
        else
        {
            RemoveTape();
        }
    }


    void ApplyTape()
    {
        if (tapeJoint != null)
            return;

        if (rb == null)
            return;


        tapeAnchor = new GameObject(
            "Tape Anchor"
        );


        tapeAnchor.transform.position =
            transform.position;


        Rigidbody anchorRB =
            tapeAnchor.AddComponent<Rigidbody>();


        anchorRB.isKinematic = true;


        tapeJoint =
            gameObject.AddComponent<FixedJoint>();


        tapeJoint.connectedBody = anchorRB;

        rb.constraints = RigidbodyConstraints.FreezeAll;

        collision.enabled = false;

        rend.enabled = false;


        Debug.Log(
            name + " pinned by tape"
        );
    }


    void RemoveTape()
    {
        if (tapeJoint != null)
        {
            Destroy(tapeJoint);
        }


        if (tapeAnchor != null)
        {
            Destroy(tapeAnchor);
        }


        Debug.Log(
            name + " tape removed"
        );
    }
}