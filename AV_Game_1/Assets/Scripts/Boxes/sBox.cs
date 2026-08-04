using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class sBox : sInteractive, iClickable, IPointerEnterHandler, IPointerExitHandler, iLoadable
{
    GameManager gm;

    public GameObject inventoryPanel;

    public Vector3 inventoryPanelOffset;

    static sInventory inventory = null;

    public int numberOfSlots;

    public List<SO_ItemData> boxedItemDataList;

    //GameObject tempInventoryObj;

    public Material materialBoxClosed;
    public Material materialBoxOpen;
    public Material materialBoxEmpty;

    bool isOpen = false;

    bool isEmpty = false;

    //int pickOffset = 0;

    //public GameObject ui_Ring;
    public float UI_ToggleDistance = 5f;
    bool isWithinOpenRange = false;

    public GameObject ui_Img;
    
    public Vector3 ui_Img_Offset, ui_Text_Offset;

    public GameObject pModel;

    public Texture2D boxSelectMouseImage;

    // Data asset this box was spawned/initialized from (null if placed manually in-scene)
    public SO_BoxData boxData;

    // TO DO - Box needs to generate it's own SO Data

    public SO_ItemData ItemData { get { return _itemData; } set { _itemData = value; } }

    SO_ItemData _itemData;

    public float boxTextFontSize = 0.05f;

    //bool isBeingThrown = false;

    

    // Start is called before the first frame update
    private void Awake()
    {
        gm = GameManager.gm;

        ui_Select.SetActive(false);

        ui_Img.SetActive(false);

        inventory = inventoryPanel.GetComponent<sInventory>();

        inventory.SetBox(this);

        inventory.SetInventory(boxedItemDataList.ToArray());
        //ui_Text.GetComponent<TextMeshProUGUI>().fontSize = boxTextFontSize;

        //ui_Text.SetActive(false);



    }

    // Called by sBoxSpawner right after Instantiate. Applies all data-driven
    // fields (materials, slots, offsets, scale) from the given SO_BoxData.
    public void Initialize(SO_BoxData data)
    {
        if (data == null)
        {
            Debug.LogWarning($"sBox.Initialize called with null data on {gameObject.name}");
            return;
        }

        boxData = data;

        numberOfSlots = data.numberOfSlots;

        boxedItemDataList = new List<SO_ItemData>(data.startingItemData);

        ScriptableObject _itemData = ScriptableObject.CreateInstance(typeof(SO_ItemData));

        ItemData = ((SO_ItemData)_itemData);

        ItemData.prefabItem = this.gameObject;

        ItemData.itemName = "Box";

        //ItemData.prefabItem = this.gameObject;

        //materialBoxClosed = data.materialBoxClosed;
        //materialBoxOpen = data.materialBoxOpen;
        //materialBoxEmpty = data.materialBoxEmpty;
        //boxSelectMouseImage = data.boxSelectMouseImage;

        //UI_ToggleDistance = data.UI_ToggleDistance;
        //ui_Img_Offset = data.ui_Img_Offset;
        //ui_Text_Offset = data.ui_Text_Offset;
        //inventoryPanelOffset = data.inventoryPanelOffset;

        //ApplyScale(data);

        // Make sure the model starts on the closed material to match the fresh state
        //if (pModel != null && materialBoxClosed != null)
        //    pModel.GetComponent<MeshRenderer>().material = materialBoxClosed;
    }

    void ApplyScale(SO_BoxData data)
    {
        if (pModel != null)
            pModel.transform.localScale = data.modelScale;

        float ringScale = data.autoScaleRingUI
            ? Mathf.Max(data.modelScale.x, data.modelScale.z)
            : data.ringUIScaleOverride;

        if (ui_Select != null)
            ui_Select.transform.localScale = Vector3.one * ringScale;
    }

    private void Update()
    {
        if (!isEmpty && sPlayerCharacter.playerCharacterGlobal != null)
            DetectPlayer();

        //if(ui_Ring)
        //{
        //Debug.Log("Offsetting Ring UI");

        ui_Img.gameObject.transform.position = this.gameObject.transform.position + ui_Img_Offset;
        //ui_Text.gameObject.transform.position = this.gameObject.transform.position + ui_Text_Offset;
        //}
    }

    // Recalculates the panel offset's z-sign each time the box opens, based on
    // where the player currently is relative to the box, so the panel always
    // opens on the far side from the player (top-down: greater player z -> negative
    // offset, lesser player z -> positive offset). x/y magnitudes come from the data asset.
    Vector3 GetInventoryPanelOffset()
    {
        Vector3 offset = inventoryPanelOffset;

        bool dynamicEnabled = boxData == null || boxData.useDynamicZOffset;

        if (dynamicEnabled)
        {
            var playerObj = sPlayerCharacter.playerCharacterGlobal;

            if (playerObj != null)
            {
                float zMagnitude = Mathf.Abs(offset.z);
                offset.z = (playerObj.transform.position.z > this.transform.position.z) ? -zMagnitude : zMagnitude;
            }
        }

        return offset;
    }

    public void TriggerOpenBox()
    {
        //base.TriggerAction(_actionObj, _toolToUse);
        //Debug.Log("Box Open Triggered");

        if (numberOfSlots > 0)
        {
            //Debug.Log("Opening Box");

            Vector3 panelOffset = GetInventoryPanelOffset();

            //inventory = Instantiate(pBoxInventoryPanel, this.transform.position + panelOffset, Quaternion.identity).GetComponent<sInventory>();

            sPlayerCharacter.playerCharacterGlobal.ToggleMovement(false);

            inventoryPanel.gameObject.SetActive(true);

            rb.constraints = RigidbodyConstraints.FreezeAll;

            //inventory.gameObject.transform.parent = this.transform;

            //inventory.SetBox(this);

            //inventory.SetInventory(boxedItemDataList.ToArray());

            //pModel.GetComponent<MeshRenderer>().material = materialBoxOpen;

            textMPAbove.SetText("OPEN");

            //ui_Ring.GetComponentInChildren<MeshRenderer>().material.color = Color.yellow;

            //ui_Text.SetActive(false);

            ui_Img.SetActive(false);

            ui_Select.SetActive(false);

            //GameManager.gm.ReturnCurrentPlayer().ReturnGrabController().grabPopupText.
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

        rb.constraints = startingConstraints;

        inventoryPanel.SetActive(false);

        sPlayerCharacter.playerCharacterGlobal.ToggleMovement(true);

        //if (inventory != null)
        //{
        //    inventory.ResetInventory();

        //    Destroy(inventory.gameObject);
        //    //inventory = null;
        //}


        textMPAbove.SetText("CLOSED");

        //ui_Ring.GetComponentInChildren<MeshRenderer>().material.color = Color.green;

        //ui_Text.SetActive(true);

        ui_Img.SetActive(true);
    }

    /*public void InventoryItemPick(int _index)
    {
        numberOfSlots--;

        //RemoveItemData(_index);
    }*/

    public void RemoveItemData(SO_ItemData _ItemData)
    {
        //Debug.Log("Removing Item at index: " + _index);
        boxedItemDataList.Remove(_ItemData);


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
        //ui_Text.SetActive(false);

        ui_Img.SetActive(false);


    }

    // This gets called when a player clicks the box
    public void OnClick()
    {
        Debug.Log(gameObject.name + " OnClick called. isWithinOpenRange=" + isWithinOpenRange + " isEmpty=" + isEmpty + " isOpen=" + isOpen);


        if (isWithinOpenRange && !isEmpty && !isOpen && !iGrabbable.IsGrabbed)
        {

            //Debug.Log(gameObject.name + " OnClick called. isWithinOpenRange=" + isWithinOpenRange + " isEmpty=" + isEmpty + " isOpen=" + isOpen);

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
        if (Vector3.Distance(this.transform.position, sPlayerCharacter.playerCharacterGlobal.transform.position) < UI_ToggleDistance)
        {
            isWithinOpenRange = true;

            //ui_Img.SetActive(true);
            //if (!isOpen)
                //ui_Text.SetActive(true);
            //ui_Ring.SetActive(!iGrabbable.IsGrabbed);
        }

        else
        {
            //Debug.Log("Outside of range of " + this.gameObject.name);

            isWithinOpenRange = false;

            //ui_Img.SetActive(false);
            //ui_Text.SetActive(false);

            //ui_Ring.SetActive(false);

            if (isOpen)
                CloseBox();
        }
    }

    public override void OnGrab()
    {
        //Debug.Log("Box On Grab Triggered");
        iGrabbable.IsGrabbed = true;
        //ui_Ring.SetActive(false);
        ui_Select.SetActive(false);

        ui_Img.SetActive(false);
    }


    public override void OffGrab()
    {
        //Debug.Log("Box Off Grab Triggered");
        Invoke("GrabReset", 0.5f);
        //ui_Select.SetActive(true);
    }

    void GrabReset()
    {
        iGrabbable.IsGrabbed = false;
    }

    public override void OnSelect()
    {
        if(sCharacterGrabController.isGrabbing)
        return;

        //Debug.Log("On Select on Box");
        if(!isOpen)
            ui_Select.SetActive(true);

        soUI.TriggerControlsPopup("F", "Open");

        //base.OnSelect();

        
    }

    public override void OffSelect()
    {
        //Debug.Log("Off Select on Box");
        ui_Select.SetActive(false);
        //base.OnSelect();

        soUI.TriggerControlsPopup("", "Open");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("Mouse entered the box object of " + this.gameObject.name);

        if (isWithinOpenRange && !sCharacterGrabController.isGrabbing)
            ui_Img.SetActive(true);
        //Cursor.SetCursor(boxSelectMouseImage, new Vector2(10, 10), CursorMode.Auto);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("Mouse has exited box " + this.gameObject.name);

        if (eventData.fullyExited)
            ui_Img.SetActive(false);
        //Cursor.SetCursor()
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