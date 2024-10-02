using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sToolHandler : MonoBehaviour, iRequireHands
{
    public Transform transformToolbelt;

    public SO_EventsUI soUI;

    sPlayerCharacter playerCharacter;

    //SO_ItemData toolItemData;

    //GameObject toolObj;

    List<SO_ItemData> toolHeldItemDataList;


    // Hands stuff
    public int _numberOfHandsNeeded;
    public int NumberOfHandsNeeded
    {
        get
        {
            return _numberOfHandsNeeded;
        }

        set
        {
            _numberOfHandsNeeded = value;
        }
    }

    List<int> _handIndexList;

    public List<int> HandIndexList
    {
        get
        {
            return _handIndexList;
        }

        set
        {
            _handIndexList = value;
        }
    }

    public Sprite _handUseSprite;

    public Sprite HandUseSprite
    {
        get
        {
            return _handUseSprite;
        }

        set { }
    }

    

    // Start is called before the first frame update
    void Start()
    {
        playerCharacter = GetComponentInParent<sPlayerCharacter>();

        //toolItemData = new SO_ItemData();
        //toolItemData = null;

        //toolObj = null;
        toolHeldItemDataList = new List<SO_ItemData>();
        //toolList = new List<SO_ItemData>();
    }

    public SO_ItemData CheckIfHasTool(eToolType _toolType)
    {
        SO_ItemData _tempTool = toolHeldItemDataList[0];

        //bool hasCorrectTool = false;

        if (toolHeldItemDataList != null)
        {
            for (int i = 0; i < toolHeldItemDataList.Count; i++)
            {
                if (toolHeldItemDataList[i].typeOfTool == _toolType)
                {
                    _tempTool = toolHeldItemDataList[i];

                    Debug.Log("Tool check - has correct type of tool!  Returning tool");// + toolHeldList[i].name);
                    //hasCorrectTool = true;

                    return _tempTool;
                }
            }

        }

        else

        {
            Debug.Log("Tool held list is null");

            _tempTool = null;
        }

        Debug.Log("End of tool check - returning at end");

        return _tempTool;
    }

    public List<SO_ItemData> ReturnToolHeldList()
    {
        return toolHeldItemDataList;
    }

    public void DropTool(int _index)
    {
        // Spawns tool model
        Instantiate(toolHeldItemDataList[_index].prefabItem);

        // Triggers UI change
        soUI.TriggerToolChange(toolHeldItemDataList[_index]);

        // Removes tool from list
        toolHeldItemDataList.RemoveAt(_index);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<sTool>(out sTool _tool))
        {
            Debug.Log("Collided with a tool: " + _tool);

            //bool handFree = true;
            //int[] _tempIndexArray;// = new int[NumberOfHandsNeeded];

            //umberOfHandsNeeded = _tool.itemData.numberOfHandsNeeded;
            HandUseSprite = _tool.itemData.itemSprite;
            //toolItemData = _tool.itemData;
            
            /*
            _tempIndexArray = GameManager.gm.ReturnCurrentPlayer().CheckHands(NumberOfHandsNeeded);
            int handCounter = 0;

            for (int i = 0; i < _tempIndexArray.Length; i++)
            {
                if (_tempIndexArray[i] < 0)
                {
                    Debug.Log("Temp Index is too small at position " + i.ToString() + " with value of " + _tempIndexArray[i].ToString());
                    handCounter++;
                    if (handCounter == _tempIndexArray.Length)
                    {
                        Debug.Log("Hand counter says no hands are free!");
                        handFree = false;
                    }
                    //handFree = false;
                }

                else
                {
                    //HandIndexList.Add(_tempIndexArray[i]);
                }
            }

            if (!handFree)
            {
                Debug.Log("No hand free");
            }

            */

            // has no tool and grabs a tool - and has hand free
            if (toolHeldItemDataList != null)//  && handFree)
            {

                //toolItemData = _tool.itemData;
                // Sets grab UI text
                //soUI.ToggleControlsPopup(grabPopupText, _collisionObj.transform.position + grabbable.ui_offset);

                //GameManager.gm.ReturnCurrentPlayer().SetHand(HandUseSprite, _tempIndexArray);
                //HandIndexList = new List<int>(_tempIndexArray);

                Debug.Log("Tool Acquired to belt");

                toolHeldItemDataList.Add(_tool.itemData);

                soUI.TriggerToolChange(_tool.itemData);

                Destroy(other.gameObject);

                //toolObj = other.gameObject;

                //eventsUI.TriggerItemHeldImage(_tool.itemData.itemSprite);
                //GameManager.gm.ReturnCurrentPlayer().CheckHands(toolItemData.itemSprite, toolItemData.numberOfHandsNeeded);

                //_tool.itemData.prefabItem.SetActive(false);


                //toolObj.transform.parent = transformToolbelt;
                //other.gameObject.SetActive(false);
                //Destroy(other.gameObject);
            }


            // when you already have a tool and touch another tool
            else

            {
                Debug.Log("Tool item data list is null!");
                // spawn a canvas asking if you want to drop current tool
            }
            
        }
    }
}
