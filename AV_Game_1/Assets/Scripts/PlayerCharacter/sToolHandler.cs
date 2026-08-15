using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(sTapeTool))]
public class sToolHandler : MonoBehaviour
{
    public Transform transformToolbelt;

    public SO_EventsUI soUI;

    public SO_AudioEventChannel soAudio;

    public SO_VFXEventChannel soVFX;

    sPlayerCharacter playerCharacter;

    //SO_ItemData toolItemData;

    //GameObject toolObj;

    //List<SO_ToolData> toolHeldItemDataList;

    SO_ToolData currentToolData = null;

    //SO_ToolData currentToolItem = null;

    // Hands stuff
    //public int _numberOfHandsNeeded;
    //public int NumberOfHandsNeeded
    //{
    //    get
    //    {
    //        return _numberOfHandsNeeded;
    //    }

    //    set
    //    {
    //        _numberOfHandsNeeded = value;
    //    }
    //}

    //List<int> _handIndexList;

    //public List<int> HandIndexList
    //{
    //    get
    //    {
    //        return _handIndexList;
    //    }

    //    set
    //    {
    //        _handIndexList = value;
    //    }
    //}

    //public Sprite _handUseSprite;

    //public Sprite HandUseSprite
    //{
    //    get
    //    {
    //        return _handUseSprite;
    //    }

    //    set { }
    //}

    sTapeTool tapeTool;
    

    // Start is called before the first frame update
    void Start()
    {
        playerCharacter = GetComponentInParent<sPlayerCharacter>();

        //toolItemData = new SO_ItemData();
        //toolItemData = null;

        //toolObj = null;
        //toolHeldItemDataList = new List<SO_ToolData>();

        tapeTool = GetComponent<sTapeTool>();

        tapeTool.enabled = false;
        //toolList = new List<SO_ItemData>();
    }

    public SO_ToolData CheckIfHasTool(eToolType _toolType)
    {
        if (currentToolData == null)
            return null;

        if(currentToolData.typeOfTool == _toolType)
        {
            return currentToolData;
        }

        else

        {
            //Debug.Log("Tool held is incorrect");

            return null;
        }

        //Debug.Log("End of tool check - returning at end");

        //return _tempTool;
    }

    /*public List<SO_ToolData> ReturnToolHeldList()
    {
        return toolHeldItemDataList;
    }*/

    public void DropTool()
    {
        if (currentToolData == null)
            return;

        // Spawns tool model
        //Instantiate(toolHeldItemDataList[_index].prefabItem);

        GameObject tempObj;

        tempObj = Instantiate(currentToolData.prefabItem);

        Vector3 randomCircle = Random.onUnitSphere;

        Vector3 offset = new Vector3(randomCircle.x, 1f, randomCircle.z) * 2f;

        tempObj.transform.position = sPlayerCharacter.playerCharacterGlobal.transform.position + Vector3.up * 2f;

        Rigidbody rb = tempObj.GetComponent<Rigidbody>();

        rb.WakeUp();

        rb.constraints = RigidbodyConstraints.None;

        rb.AddForce(offset, ForceMode.Impulse);

        rb.AddTorque(Random.onUnitSphere * 100f, ForceMode.Impulse);

        // Triggers UI change
        //soUI.TriggerToolChange();

        // Removes tool from list
        //toolHeldItemDataList.RemoveAt(_index);

        //Invoke("ResetToolRB", 0.5f);
    }

    void ResetToolRB()
    {

    }

    public void GoTool(SO_ToolData _toolData)
    {
        // triggers UI change
        soUI.TriggerToolChange(_toolData);

        // turns off all current tools, then turns back on one tool
        //ToolsOff();

        // turns tool on, etc.
        switch(_toolData.typeOfTool)
        {
            case eToolType.NONE:

                break;

            case eToolType.ratchet:

                break;

            case eToolType.tape:

                tapeTool.enabled = true;

                break;
        }
    }

    void ToolsOff()
    {
        tapeTool.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<sTool>(out sTool _tool))
        {
            //Debug.Log("Collided with a tool: " + _tool);

            // this is for Non toolbelt tools - like nuts and bolts
            switch (_tool.toolData.typeOfTool)
            {
                case eToolType.NONE:

                    break;

                case eToolType.nut:

                    soUI.TriggerNutPickup();

                    if (soAudio != null)
                        soAudio.TriggerSFX("NutPickup");

                    Destroy(other.gameObject);

                    //return;
                    break;

                case eToolType.bolt:

                    soUI.TriggerBoltPickup();

                    if (soAudio != null)
                        soAudio.TriggerSFX("BoltPickup");

                    Destroy(other.gameObject);

                        //return;

                        break;

                case eToolType.tape:

                case eToolType.ratchet:
                
                case eToolType.crescent:

                    if (soAudio != null)
                        soAudio.TriggerSFX("ToolPickup");

                    DropTool();

                    currentToolData = _tool.toolData;

                    soUI.TriggerToolChange(_tool.toolData);

                    Destroy(other.gameObject);

                break;
                //break;
            }

            /*// has no tool and grabs a tool - and has hand free
            if (toolHeldItemDataList != null)//  && handFree)
            {
                //Debug.Log("Tool Acquired to belt");

                // adds to list
                toolHeldItemDataList.Add(_tool.toolData);

                if (_tool.toolData != null)
                    GoTool(_tool.toolData);
                else
                    Debug.LogWarning("No tool data for: " + _tool);

                Destroy(other.gameObject);
            }


            // when you already have a tool and touch another tool
            else

            {
                Debug.Log("Tool item data list is null!");
                // spawn a canvas asking if you want to drop current tool
            }*/
            
        }
    }
}
