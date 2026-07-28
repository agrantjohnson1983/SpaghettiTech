using UnityEngine;

public class sTapeSegment : MonoBehaviour
{
    public bool IsTaped => tapeJoint != null;

    Rigidbody rb;

    FixedJoint tapeJoint;
    GameObject tapeAnchor;


    Collider collider;
    MeshRenderer renderer;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        collider = GetComponent<Collider>();

        renderer = GetComponent<MeshRenderer>();
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

        collider.enabled = false;

        renderer.enabled = false;


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