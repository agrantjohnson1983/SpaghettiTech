using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class uConnectionsAvailablePanel : MonoBehaviour
{
    //public static uConnectionsAvailablePanel connectionAvailablePanel;

    public Transform connectionsGridTransform;

    public GameObject pButtonConnectionAvailable;

    List<GameObject> connectionsAvailableList;

    sConnectionSource connectionSource;
    uConnectionPlate connectionPlate;

    GameObject connectionClickedObj;

    LineRenderer lr;

    List<LineRenderer> lineRendererList;

    Vector3 startPos;
    Vector3 endPos;
    Camera cam;
    [SerializeField] AnimationCurve animationCurve;

    Vector3 camOffset = new Vector3(0, 0, 10);
    bool isClickingConnection = false;

    int tempClickedIndex = -1;

    public GameObject connectionLine;


    // Start is called before the first frame update
    void Start()
    {
        // Line Renderer stuff
        lr = sConnectionLineRenderer.lineRender.lr;
        lr.enabled = false;
        cam = Camera.main;

        lineRendererList = new List<LineRenderer>();
    }

    // Line Rendere Gets toggled when "isClickingConnection" is toggled
    void Update()
    {
        if(isClickingConnection)
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
            if(connectionsAvailableList != null)
            {
                Debug.Log("More than 1 connection - adding to list with list count of " + connectionsAvailableList.Count);

                for (int i = 1; i < _pluggableObjects.Count; i++)
                {
                    uButtonConnectionAvailable connectionButton;

                    connectionsAvailableList.Add(Instantiate(pButtonConnectionAvailable, connectionsGridTransform));

                    connectionButton = connectionsAvailableList[i].gameObject.GetComponent<uButtonConnectionAvailable>();

                    connectionButton.SetConnection(_pluggableObjects[i].GetComponent<iPluggable>().connectionSprite, i + 1.ToString(), i);

                    //connectionButton.SetI
                }
            }

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
        connectionsAvailableList[_index].GetComponent<uButtonConnectionAvailable>().OnChannelClick();
        
        // This sets a line renderer
        SetConnectionLine(_index);
    }


    // This sets the isClickingConnection to true which turns on the line renderer
    public void OnConnectionClick(int _index)
    {

        Debug.Log("Button Click at index of: " + _index + ".  Connections available list has count of: " + connectionsAvailableList.Count);

        isClickingConnection = true;

        tempClickedIndex = _index;

        if (_index >= 0)
        {
            Debug.Log("Triggering " + connectionsAvailableList[_index].gameObject.name);

            connectionClickedObj = connectionsAvailableList[_index];
        }
            

        else
            Debug.Log("Index is less than 0");

        
    }

    // This returns the index that has been temporarily set
    public int ReturnTempIndex()
    {
        return tempClickedIndex;
    }

    // This method handles the line renderer when the player clicks a button for a "connection available"
    void LineHandler()
    {
           if(lr.enabled == false)
            {
            //Debug.Log("Starting Line");
            //lr = gameObject.AddComponent<LineRenderer>();

            lr.enabled = true;

            lr.useWorldSpace = true;

            lr.positionCount = 2;
            
            //startPos = connectionClickedObj.transform.position;

            startPos = cam.ScreenToWorldPoint(Input.mousePosition+camOffset);

            //startPos.z = sPlayerCharacter.playerGlobal.transform.position.z;

            lr.SetPosition(0, startPos);
            
            
            //lr.widthCurve = animationCurve;
            //lr.numCapVertices = 10;

            }

        if(Input.GetMouseButton(0))
        {
            //endPos = Input.mousePosition;

            Vector3 endMousePos = Input.mousePosition;

            //endMousePos = endMousePos * -1;

            //endPos = Camera.main.ScreenToWorldPoint(endPos);
            endPos = Camera.main.ScreenToWorldPoint(endMousePos+camOffset);

            //endPos.z = 0;

            //endPos.z = sPlayerCharacter.playerGlobal.transform.position.z;

            lr.SetPosition(1, endPos);

            //Debug.Log("Holding Line");
        }

        if(Input.GetMouseButtonUp(0))
        {
            lr.enabled = false;
            
            lr.positionCount = 0;

            
            //lr = null;
            isClickingConnection = false;

            Debug.Log("Line Release");
        }
    }



    // Use this for setting a connection line to continually appear once it's been connected
    public void SetConnectionLine(int _index)
    {
        // Caches a line renderer
        LineRenderer _lr;
        
        // Spanws the line renderer and then gets a reference to it
        _lr = Instantiate(connectionLine, this.transform).GetComponent<LineRenderer>();

        // Adds position count to set the two line points
        _lr.positionCount = 2;

        // Uses the startPos and endPos of the other line renderer system which sets the line to the last 2 points used
        _lr.SetPosition(0, startPos - camOffset);
        _lr.SetPosition(1, endPos - camOffset);

        // Adds the line to the line renderer list
        lineRendererList.Add(_lr);
    }

    // When a connection available button is clicked when connected
    public void DisconnectLine(int _index)
    {

        Debug.Log("Disabling Line Renderer at index of: " + _index);

        // Checks to make sure index is not set to negative
        if (_index < 0)
        {
            Debug.Log("Line Index Cannot Be Less Than 0");
            return;
        }

        // Checks to see if line renderer list is null
        if(lineRendererList == null)
        {
            Debug.Log("Line Render List Null");
            return;
        }

        // Sets line render from index
        LineRenderer lr = lineRendererList[_index];

        // Removes line from list
        lineRendererList.Remove(lr);

        // Destroys line
        Destroy(lr.gameObject);
    }
    

    // Returns a reference to the connection plate
    public uConnectionPlate ReturnConnectionPlate()
    {
        return connectionPlate;
    }
    
    // Returns if the player is clicking connection or not
    public bool ReturnIsClicking()
    {
        return isClickingConnection;
    }
}
