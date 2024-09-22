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
     
    private void Awake()
    {
        //connectionAvailablePanel = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        lr = sConnectionLineRenderer.lineRender.lr;
        lr.enabled = false;
        cam = Camera.main;
        
        //connectionsAvailableList = new List<GameObject>();

        lineRendererList = new List<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isClickingConnection)
        LineHandler();
    }

    public void SetConnectionSource(sConnectionSource _source)
    {
        connectionSource = _source;
    }

    public void SetConnectionPlate(uConnectionPlate _plate)
    {
        connectionPlate = _plate;
    }

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
    }

    public void OnConnectionClick(int _index)
    {
        Debug.Log("Button Click at index of: " + _index + ".  Connections available list has count of: " + connectionsAvailableList.Count);

        isClickingConnection = true;

        tempClickedIndex = _index;

        Debug.Log("Triggering " + connectionsAvailableList[_index].gameObject.name);

        connectionClickedObj = connectionsAvailableList[_index];
    }

    public int ReturnTempIndex()
    {
        return tempClickedIndex;
    }

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

    public bool ReturnIsClicking()
    {
        return isClickingConnection;
    }
}
