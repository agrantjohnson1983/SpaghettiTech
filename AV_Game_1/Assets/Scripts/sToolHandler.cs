using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sToolHandler : MonoBehaviour, iRequireHands
{
    public Transform transformToolbelt;

    public SO_EventsUI soUI;

    sPlayerCharacter playerCharacter;

    SO_ItemData toolItemData;

    GameObject toolObj;

    List<sTool> toolHeldList;


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

    public bool CheckIfHasTool(eToolType _toolType)
    {

        bool hasCorrectTool = false;

        if (toolHeldList != null)
        {
            for (int i = 0; i < toolHeldList.Count; i++)
            {
                if (toolHeldList[i].typeOfTool == _toolType)
                {
                    Debug.Log("Tool check - has correct type of tool!");
                    hasCorrectTool = true;
                }
            }

        }

        else

        {
            Debug.Log("Tool held list is null");
        }

        return hasCorrectTool;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerCharacter = GetComponentInParent<sPlayerCharacter>();

        toolItemData = new SO_ItemData();
        toolItemData = null;

        toolObj = null;
        toolHeldList = new List<sTool>();
        //toolList = new List<SO_ItemData>();
    }

    public List<sTool> ReturnToolHeldList()
    {
        return toolHeldList;
    }

    public void DropTool(int _index)
    {
        // Spawns tool model
        Instantiate(toolHeldList[_index].itemData.prefabItem);

        // Triggers UI change
        soUI.TriggerToolChange(toolHeldList[_index].itemData);

        // Removes tool from list
        toolHeldList.RemoveAt(_index);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<sTool>(out sTool _tool))
        {
            bool handFree = true;
            int[] _tempIndexArray;// = new int[NumberOfHandsNeeded];

            NumberOfHandsNeeded = _tool.itemData.numberOfHandsNeeded;
            HandUseSprite = _tool.itemData.itemSprite;
            toolItemData = _tool.itemData;

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

            // has no tool and grabs a tool - and has hand free
            if (toolItemData != null  && handFree)
            {

                //toolItemData = _tool.itemData;
                // Sets grab UI text
                //soUI.ToggleControlsPopup(grabPopupText, _collisionObj.transform.position + grabbable.ui_offset);

                //GameManager.gm.ReturnCurrentPlayer().SetHand(HandUseSprite, _tempIndexArray);
                //HandIndexList = new List<int>(_tempIndexArray);

                Debug.Log("Tool Acquired to belt");

                toolHeldList.Add(_tool);

                soUI.TriggerToolChange(toolItemData);

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
                Debug.Log("You alreayd have a tool and just got another - need to implement this!");
                // spawn a canvas asking if you want to drop current tool
            }
            
        }
    }
}
