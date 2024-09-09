using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sToolHandler : MonoBehaviour
{
    public Transform transformToolbelt;

    public SO_EventsUI eventsUI;

    sPlayerCharacter playerCharacter;

    SO_ItemData toolItemData;

    GameObject toolObj;
    sTool tool;
    // Start is called before the first frame update
    void Start()
    {
        playerCharacter = GetComponentInParent<sPlayerCharacter>();

        toolItemData = new SO_ItemData();
        toolItemData = null;

        toolObj = null;
        tool = null;
        //toolList = new List<SO_ItemData>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public sTool ReturnToolType()
    {
        return tool;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<sTool>(out sTool _tool))
        {
            // has no tool and grabs a tool
            if(toolItemData == null)
            {
                toolItemData = _tool.itemData;

                tool = _tool;

                toolObj = other.gameObject;

                eventsUI.TriggerToolHeldImage(_tool.itemData.itemSprite);

                tool.toolModel.SetActive(false);

                toolObj.transform.parent = transformToolbelt;
                //other.gameObject.SetActive(false);
                //Destroy(other.gameObject);
            }

            // when you already have a tool and touch another tool
            else

            {
                // spawn a canvas asking if you want to drop current tool
            }
            
        }
    }
}
