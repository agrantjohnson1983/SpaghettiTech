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

    bool _isGrabbed = false;

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

    public bool IsGrabbed
    {
        get
        {
            return _isGrabbed;
        }
        set
        {
            _isGrabbed = value;
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

    public virtual void OnGrab()
    {
        IsGrabbed = true;

        
    }

    public virtual void OffGrab()
    {
        IsGrabbed = false;
        canBeSelected = false;
        StartCoroutine(GrabSelectCooldown());
    }

    IEnumerator GrabSelectCooldown()
    {
        float counter = 0f;

        while(counter < 1f)
        {
            counter += Time.deltaTime;
            yield return null;
        }

        canBeSelected = true;
    }

    public virtual void OnSelect()
    {
        if(!sPlayerCharacter.playerCharacterGlobal.ReturnGrabController().ReturnIsGrabbing() && canBeSelected)
        {
            if(ui_Select!=null)
                ui_Select.SetActive(true);

            if(soUI!=null)
                soUI.TriggerControlsPopup("SPACE", "Grab");
        }
    }

    public virtual void OffSelect()
    {

        if(ui_Select!=null)
            ui_Select.SetActive(false);

        if(soUI!=null)
            soUI.TriggerControlsPopup("", "Grab");
    }
}
