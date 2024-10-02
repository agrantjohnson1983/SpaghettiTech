using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sBox : sInteractive, iClickable
{
    GameManager gm;

    public GameObject pBoxInventoryPanel;

    static sInventory inventory = null;

    public int numberOfSlots;

    public List<SO_ItemData> itemData;

    //GameObject tempInventoryObj;

    public Material materialBoxClosed;
    public Material materialBoxOpen;
    public Material materialBoxEmpty;

    bool isOpen = false;

    bool isEmpty = false;

    int pickOffset = 0;

    public Vector3 inventoryPanelOffset;

    //public GameObject ui_Ring;
    public float UI_ToggleDistance = 5f;
    bool isWithinOpenRange = false;

    public GameObject ui_Img, ui_Text;

    public Vector3 ui_Img_Offset, ui_Text_Offset;

    public GameObject pModel;

    // Start is called before the first frame update
    private void Awake()
    {
        //pModel.GetComponent<MeshRenderer>().material = materialBoxClosed;

        gm = GameManager.gm;

        ui_Select.SetActive(false);

        ui_Img.SetActive(false);

        ui_Text.SetActive(false);
        //ui_Ring.SetActive(false);
    }

    private void Update()
    {
        if(!isEmpty && GameManager.gm.ReturnCurrentPlayer() != null)
        DetectPlayer();

        //if(ui_Ring)
        //{
            //Debug.Log("Offsetting Ring UI");
             
            ui_Img.gameObject.transform.position = this.gameObject.transform.position + ui_Img_Offset;
            ui_Text.gameObject.transform.position = this.gameObject.transform.position + ui_Text_Offset;
        //}
    }

    public void TriggerOpenBox()
    {
        //base.TriggerAction(_actionObj, _toolToUse);
        //Debug.Log("Box Open Triggered");

            if(numberOfSlots > 0)
            {
            //Debug.Log("Opening Box");

                inventory = Instantiate(pBoxInventoryPanel, this.transform.position + inventoryPanelOffset, Quaternion.identity).GetComponent<sInventory>();

                inventory.gameObject.transform.parent = this.transform;

                inventory.SetBox(this);

                inventory.SetInventory(itemData.ToArray());

                //pModel.GetComponent<MeshRenderer>().material = materialBoxOpen;

                textMPAbove.SetText("OPEN");

                //ui_Ring.GetComponentInChildren<MeshRenderer>().material.color = Color.yellow;

                ui_Text.SetActive(false);

                ui_Img.SetActive(false);
            }

            else
            {
                //EmptyBox();
                
            }
    }

    public void CloseBox()
    {
        //Debug.Log("Closing Box");

        isOpen = false;
        //base.StopAction(_actionObj);
        // switches back to closed color unless empty
        //if(!isEmpty)
        //pModel.GetComponent<MeshRenderer>().material = materialBoxClosed;

        if (inventory != null)
        {
            Destroy(inventory.gameObject);
            inventory = null;
        }
        

        textMPAbove.SetText("CLOSED");

        //ui_Ring.GetComponentInChildren<MeshRenderer>().material.color = Color.green;

        ui_Text.SetActive(true);

        ui_Img.SetActive(true);
    }

    public void InventoryItemPick(int _index)
    {
        numberOfSlots--;

        RemoveItemData(_index);
    }

    void RemoveItemData(int _index)
    {
        //Debug.Log("Removing Item at index: " + _index);
        itemData.RemoveAt(_index);

        
        //itemData.Sort();
    }

    public int ReturnNumberOfInventorySlots()
    {
        return numberOfSlots;
    }

    public void EmptyBox()
    {
        isEmpty = true;

        pModel.GetComponent<MeshRenderer>().material = materialBoxEmpty;

        textMPAbove.SetText("EMPTY");

        //ui_Ring.SetActive(false);
        //ui_Ring.GetComponentInChildren<MeshRenderer>().material.color = Color.red;
        ui_Text.SetActive(false);
        ui_Img.SetActive(false);


    }

    // This gets called when a player clicks the box
    public void OnClick()
    {
        if(isWithinOpenRange && !isEmpty && !isOpen)
        {
            
            //if (!isOpen)
            //{

            if (inventory == null)
            {
                isOpen = true;
                TriggerOpenBox();
            }

            // Destorys current inventory and opens new one
            else
            {
                inventory.OnClickX();
                //Destroy(inventory.gameObject);
                //inventory = null;
                isOpen = true;
                TriggerOpenBox();
            }
                
            //}

            //else
            //{
            //    isOpen = false;
            //    CloseBox();
           // }
        }   
    }
    
    // this checks distance between the box and player and toggles on/off UI ring
    void DetectPlayer()
    {
        // Checks if the player is less than the distance of the UI toggle distance and if so turns on the UI
        if(Vector3.Distance(this.transform.position, GameManager.gm.ReturnCurrentPlayer().transform.position) < UI_ToggleDistance)
        {
            isWithinOpenRange = true;

            ui_Img.SetActive(true);
            ui_Text.SetActive(true);
            //ui_Ring.SetActive(!iGrabbable.IsGrabbed);
        }

        else
        {
            //Debug.Log("Outside of range of " + this.gameObject.name);

            isWithinOpenRange = false;

            ui_Img.SetActive(false);
            ui_Text.SetActive(false);

            //ui_Ring.SetActive(false);

            if (isOpen)
                CloseBox();
        }
    }

    public override void OnGrab()
    {
        Debug.Log("Box On Grab Triggered");
        iGrabbable.IsGrabbed = true;
        //ui_Ring.SetActive(false);
        ui_Select.SetActive(false);
    }


    public override void OffGrab()
    {
        Debug.Log("Box Off Grab Triggered");
        iGrabbable.IsGrabbed = false;
        //ui_Select.SetActive(true);
    }

    public override void OnSelect()
    {
        //Debug.Log("On Select on Box");
        ui_Select.SetActive(true);

        //base.OnSelect();
    }

    public override void OffSelect()
    {
        //Debug.Log("Off Select on Box");
        ui_Select.SetActive(false);
        //base.OnSelect();
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player")  && !isOpen)
        {
            isOpen = true;
            TriggerOpenBox();
            //Debug.Log("Open Trigger From Box Enter");
        }
    }

    
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !isOpen)
        {
            isOpen = true;
            TriggerOpenBox();
            //Debug.Log("Open Trigger From Box Stay");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && isOpen)
        {
            
            isOpen = false;
            CloseBox();
            //Debug.Log("Closed Trigger From Box");
        }
    }
    */
}
