using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Diagnostics.Tracing;

[RequireComponent(typeof(sObjectHighlighter))]
public class sInteractive : MonoBehaviour, iGrabbable
{
    protected Rigidbody rb;
    protected RigidbodyConstraints startingConstraints;

    public SO_EventsUI soUI;

    public SO_AudioEventChannel soAudio;

    public SO_VFXEventChannel soVFX;

    public SO_Text soText;

    // Every player currently holding this object. Public API is IsGrabbed /
    // HolderCount (below) - callers should not need the list itself.
    protected List<sPlayerCharacter> holders = new List<sPlayerCharacter>();

    //public float _taskTime;

    //public eToolType _toolTypeNeeded;
    //public eTypeRigSetup typeOfRig;

    bool hasCorrectTool;

    bool isActive;

    //public TextMeshPro textMPAbove;

    //public TextMeshPro[] itemTextMP;

    //public string textItem;

    public GameObject ui_Select;

    protected bool canBeSelected = true;

    // Minimum number of simultaneous holders before this object is actually
    // free to move - e.g. a heavy truss piece that no single player can
    // budge alone. 1 (the default) means any single player can carry it,
    // same as before this system existed.
    public int minGrabbersRequired = 1;

    /*public float TaskTime
    {
        get
        {
            return _taskTime;
        }

        set
        {
            _taskTime = value;
        }
    }*/

    /* public eToolType ToolTypeNeeded
     {
         get
         {
             return _toolTypeNeeded;
         }

         set
         {
             _toolTypeNeeded = value;
         }
     }*/

    public bool CanBeGrabbed
    {
        get;
        set;
    }

    //protected bool _canBeGrabbed;

    // True once at least one player currently has a grip on this object.
    public bool IsGrabbed
    {
        get
        {
            return holders.Count > 0;
        }
    }

    // How many players currently have a grip on this object.
    public int HolderCount
    {
        get
        {
            return holders.Count;
        }
    }

    public Transform transformGrab
    {
        get
        {
            return _transformGrab;
        }
        set
        {

        }
    }

    public Transform _transformGrab;

    public Vector3 _UI_offset;
    public Vector3 ui_offset
    {
        get
        {
            return _UI_offset;
        }

        set
        {
            _UI_offset = value;
        }
    }

    protected sPlayerCharacter playerRef = null;


    // Start is called before the first frame update
    public virtual void Start()
    {
        // Sets all the texts of item - for box labels, etc;
        /*if(itemTextMP.Length > 0)
        {
            //Debug.Log("Setting text on " + gameObject.name);

            for (int i = 0; i < itemTextMP.Length; i++)
            {
                itemTextMP[i].SetText(textItem);
            }
        }*/

        rb = GetComponent<Rigidbody>();

        startingConstraints = rb.constraints;

        CanBeGrabbed = true;
    }

    public virtual void OnGrab(sPlayerCharacter _player)
    {
        if (!holders.Contains(_player))
        {
            holders.Add(_player);
        }

        ApplyGrabberCountConstraints();
    }

    public virtual void OffGrab(sPlayerCharacter _player)
    {
        holders.Remove(_player);

        canBeSelected = false;

        StartCoroutine(GrabSelectCooldown());

        ApplyGrabberCountConstraints();
    }

    // Below minGrabbersRequired, the object is frozen solid - the springs
    // from however many players ARE holding it just can't move it, same as
    // a real object too heavy for one or two people. At or above the
    // threshold it's released back to its normal starting constraints.
    // No-ops entirely for ordinary objects (minGrabbersRequired of 1, the
    // default) so this only matters for pieces you explicitly want to gate.
    protected virtual void ApplyGrabberCountConstraints()
    {
        if (minGrabbersRequired <= 1 || rb == null)
            return;

        rb.constraints = (holders.Count >= minGrabbersRequired)
            ? startingConstraints
            : RigidbodyConstraints.FreezeAll;
    }

    IEnumerator GrabSelectCooldown()
    {
        float counter = 0f;

        while (counter < 1f)
        {
            counter += Time.deltaTime;
            yield return null;
        }

        canBeSelected = true;
    }

    public virtual void OnSelect(sPlayerCharacter _player)
    {
        if (_player.ReturnGrabController().ReturnIsGrabbing() && canBeSelected)
        {
            if (ui_Select != null)
                ui_Select.SetActive(true);

            if (soUI != null)
                soUI.TriggerControlsPopup("SPACE", "Grab");
        }
    }

    public virtual void OffSelect()
    {

        if (ui_Select != null)
            ui_Select.SetActive(false);

        if (soUI != null)
            soUI.TriggerControlsPopup("", "Grab");
    }
}