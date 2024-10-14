using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// This script handles all of the connections available within the connection source and creates/destroys the buttons associated with those connections
// as well as handling the line rendering systems
public class uConnectionsAvailablePanel : MonoBehaviour
{
    //public static uConnectionsAvailablePanel connectionAvailablePanel;

    public Transform connectionsGridTransform;

    public GameObject pButtonConnectionAvailable;

    // This is a list of game objects that have pluggable behaviors
    List<GameObject> connectionsAvailableList;

    // connection source what starts the connection of a plug
    sConnectionSource connectionSource;

    // connection plate has all of the input channels
    uConnectionPlate connectionPlate;

    GameObject connectionClickedObj;

    // Line Renderer Stuff
    LineRenderer lr;
    Vector3 startPos;
    Vector3 endPos;
    Camera cam;
    [SerializeField] AnimationCurve animationCurve;

    Vector3 camOffset = new Vector3(0, 0, 10);

    // this bool is used by the line renderer to know if the player is clicking the mouse which renders the line
    bool isClickingConnectionAvailable = false;

    int tempClickedIndex = -1;

    // this spawns a "connection" line renderer when the player connects a line
    public GameObject pConnectionLineRenderer;

    // these get set for the line renderer to determine start and end positions when making a line connection
    GameObject tempStartObj, tempEndObj;

    // connection lines gets spawned to this location
    public Transform lineRenderersTransform;

    // a list of the connection lines when they gets spawned upon making a connection
    List<sConnectionLineRenderer> lineRendererConnectionList;

    private void OnEnable()
    {
        if(lr)
        lr.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();

        // turns the line renderer off upon start
        lr.enabled = false;

        // sets a reference to the main camera
        cam = Camera.main;

        lineRendererConnectionList = new List<sConnectionLineRenderer>();
    }

    // Line Rendere Gets toggled when "isClickingConnection" is toggled
    void Update()
    {
        // The line renderer only turns on if "isClickingConnection" is toggled on
        if(isClickingConnectionAvailable)
        LineHandler();
    }

    // Sets the reference to connection source
    public void SetConnectionSource(sConnectionSource _source)
    {
        connectionSource = _source;
    }

    // Sets the reference to connection plate
    public void SetConnectionPlate(uConnectionPlate _plate)
    {
        connectionPlate = _plate;
    }

    // This sets all the connection available buttons
    public void SetConnectionsAvailable(List<GameObject> _pluggableObjects)
    {
            // checks to make sure there are connections available
            if(connectionsAvailableList != null)
            {
                if(connectionsAvailableList.Count >= _pluggableObjects.Count)
                {
                    Debug.Log("Connection available list count is greater than or equal to pluggable objects list count - returning");
                    return;
                }
                
                Debug.Log("More than 1 connection - adding to list with list count of " + connectionsAvailableList.Count);

                // iterates through plugable objects list - This was set to start at 1 - not sure why?  setting to 0
                for (int i = 0; i < _pluggableObjects.Count; i++)
                {
                    // temp connection button reference
                    uButtonConnectionAvailable connectionButton;
                    
                    // button is spawned and aded to connections available list
                    connectionsAvailableList.Add(Instantiate(pButtonConnectionAvailable, connectionsGridTransform));
                    
                    // gets a reference to the button component on the connection available object
                    connectionButton = connectionsAvailableList[i].gameObject.GetComponent<uButtonConnectionAvailable>();

                    // sets the button to the plugs information
                    connectionButton.SetConnection(_pluggableObjects[i].GetComponent<iPluggable>().connectionSprite, i + 1.ToString(), i);
                }
            }

            // This does same thing as about but creates a new list and adds the first object to 0 index
            else
            {
                Debug.Log("1st connection available spawnning");// - adding to list with list count of " + connectionsAvailableList.Count);

                uButtonConnectionAvailable connectionButton;

                connectionsAvailableList = new List<GameObject>();

                connectionsAvailableList.Add(Instantiate(pButtonConnectionAvailable, connectionsGridTransform));

                connectionButton = connectionsAvailableList[0].gameObject.GetComponent<uButtonConnectionAvailable>();

                connectionButton.SetConnection(_pluggableObjects[0].GetComponent<iPluggable>().connectionSprite, 0 + 1.ToString(), 0);
            }
    }

    // This Gets called when an available connection is clicked on an available channel.  Changes available connection color, etc.
    public void SetButtonConnected(int _index)
    {
        Debug.Log("Setting button connected and line connection with index of: " + _index);

        isClickingConnectionAvailable = false;

        GameObject tempObj;

        tempObj = connectionsAvailableList[_index];

        connectionsAvailableList.Remove(tempObj);

        Destroy(tempObj);

        if (connectionsAvailableList.Count == 0)
            this.gameObject.SetActive(false);
        //connectionsAvailableList[_index].GetComponent<uButtonConnectionAvailable>().OnChannelClick();
    }

    // Returns the button connected gameobject from an index
    public GameObject ReturnButtonConnectedFromIndex(int _index)
    {
        return connectionsAvailableList[_index];
    }
    
    // This gets called by a Connection Available Channel - This sets the isClickingConnection to true which turns on the line renderer.  This also sets the tempClickIndex, which lets an input channel get an index.
    public void OnConnectionClick(int _index)
    {
        Debug.Log("Button Click at index of: " + _index + ".  Connections available list has count of: " + connectionsAvailableList.Count);

        // toggles the bool
        isClickingConnectionAvailable = true;

        // Checks to make sure index is greater than 0
        if (_index >= 0)
        {
            //Debug.Log("Triggering " + connectionsAvailableList[_index].gameObject.name);

            // Sets the connection clicked object
            connectionClickedObj = connectionsAvailableList[_index];

            // Sets the temp clicked index
            tempClickedIndex = _index;
        }
            

        else
            Debug.Log("Index is less than 0");
    }

    // This gets called when a plug is getting disconnected - it removes the plug from connections available list and destroys the button
    public void DisconnectPlug(int _index)
    {
        if (connectionsAvailableList.Count > 0)
        {
            // iterates through the connection available list
            for (int i = 0; i < connectionsAvailableList.Count; i++)
            {
                // checks if the button available's index on the connection available list is the same as the parameter input
                if (connectionsAvailableList[i].GetComponent<uButtonConnectionAvailable>().ReturnIndex() == _index)
                {
                    Debug.Log("Connection Available is disconnecting " + _index);

                    // temp GO ref
                    GameObject tempObj;

                    // sets a temp GO ref to the button object
                    tempObj = connectionsAvailableList[i];

                    // Removes the connection at this array point
                    connectionsAvailableList.Remove(tempObj);

                    // Destroys the button object
                    Destroy(tempObj);

                    // Will this mess up index points of other connections????
                }
            }
        }
    }

    // This returns the index that has been temporarily set
    public int ReturnTempIndex()
    {
        return tempClickedIndex;
    }

    // This changes the color of the connection available line that gets drawn to whatever color is input
    public void ChangeLineColor(Color _color)
    {
        lr.startColor = _color;
        lr.endColor = _color;
    }

    // This method handles the line renderer when the player clicks a button for a "connection available"
    void LineHandler()
    {
        if(lr.enabled == false)
           {

            // Turns on line
            lr.enabled = true;

            // Sets line to world space
            lr.useWorldSpace = true;

            // Sets line to have 2 positions
            lr.positionCount = 2;
            
            // Sets first position to mouse position converted from screen to world
            startPos = cam.ScreenToWorldPoint(Input.mousePosition+camOffset);

            // Sets the line to the startPos
            lr.SetPosition(0, startPos);
        }

        if(Input.GetMouseButton(0))
        {
            // Gets a reference to mouse position
            Vector3 endMousePos = Input.mousePosition;

            // Sets the end pos to the mouse pos converted from screen space to world space
            endPos = Camera.main.ScreenToWorldPoint(endMousePos+camOffset);

            // Sets the line end point to the endPos
            lr.SetPosition(1, endPos);
        }

        if(Input.GetMouseButtonUp(0))
        {
            lr.enabled = false;
            
            lr.positionCount = 0;

            isClickingConnectionAvailable = false;

            //Debug.Log("Line Release");
        }
    }

    // Sets the temp connection objects for the line connection
    public void SetTempConnectionObjects(GameObject _tempStartObj, GameObject _tempEndObj)
    {
        tempStartObj = _tempStartObj;

        tempEndObj = _tempEndObj;
    }

    // Use this for setting a connection line to continually appear once it's been connected
    public void SetConnectionLine(int _index)
    {
        //Debug.Log("Setting Connection Line at index of " + _index);

        // temp ref for line renderer connection
        sConnectionLineRenderer lineRendererConnection;

        // Spawns the line renderer prefab and gets a reference to the Connection Line Renderer component inside
        lineRendererConnection = Instantiate(pConnectionLineRenderer, lineRenderersTransform).GetComponent<sConnectionLineRenderer>();

        // Sets the start and end connection object and index of the line renderer connection
        lineRendererConnection.SetConnectionObjects(tempStartObj, tempEndObj, _index);

        // Adds the line to the line renderer connection list
        lineRendererConnectionList.Add(lineRendererConnection);
    }

    // When a connection available button is clicked when connected
    public void DisconnectLine(int _index)
    {

        //Debug.Log("Disabling Line Renderer at index of: " + _index);

        // Checks to make sure index is not set to negative
        if (_index < 0)
        {
            Debug.Log("Line Index Cannot Be Less Than 0");
            return;
        }

        // Checks to see if line renderer list is null
        if(lineRendererConnectionList == null)
        {
            Debug.Log("Line Render List Null");
            return;
        }

        // Sets line render from index
        sConnectionLineRenderer lr = lineRendererConnectionList[_index];

        // Removes line from list
        lineRendererConnectionList.Remove(lr);

        // Destroys line
        Destroy(lr.gameObject);
    }

    private void OnDisable()
    {
        ResetLineRenderer();
    }

    void ResetLineRenderer()
    {
        isClickingConnectionAvailable = false;

        lr.enabled = false;
    }
    
    // Returns a reference to the connection plate
    public uConnectionPlate ReturnConnectionPlate()
    {
        return connectionPlate;
    }
    
    // Returns if the player is clicking connection or not
    public bool ReturnIsClickingConnectionAvailable()
    {
        return isClickingConnectionAvailable;
    }
}
