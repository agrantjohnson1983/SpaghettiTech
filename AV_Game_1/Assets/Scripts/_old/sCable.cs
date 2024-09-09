using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sCable : sInteractive
{
    public GameObject pCablePoint;

    List<GameObject> cablePointsList;

    sCableConnectionHandler connectionHandler;

    Collider collider;

    //Rigidbody rb;

    LineRenderer lineRenderer;

    int activeLineIndex = 0;

    sPlayerCharacter player;

    Transform playerTransform;

    Vector3 startingPos;

    public float vertexCount = 2;

    Transform[] curvePoints;

    bool isConnected = false;

    public Material materialConnected;

    // Start is called before the first frame update
    void Start()
    {
        player = sPlayerCharacter.playerGlobal;

        playerTransform = player.transform;

        //rb = GetComponent<Rigidbody>();

        collider = GetComponent<Collider>();

        startingPos = this.gameObject.transform.position;

        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.positionCount = 2;

        cablePointsList = new List<GameObject>();

        // Spawns 2 cable points for beginning and end

        SetStartPoints();
        

        connectionHandler = GetComponentInParent<sCableConnectionHandler>();
    }

    void Update()
    {
        //if(!isConnected)
        //{
        //    lineRenderer.SetPosition(lineRenderer.positionCount - 1, this.gameObject.transform.position);
        //}

        for (int i = 0; i < lineRenderer.positionCount-1; i++)
        {
            lineRenderer.SetPosition(i, cablePointsList[i].transform.position);
        }

        lineRenderer.SetPosition(lineRenderer.positionCount - 1, this.gameObject.transform.position);

    }

    void SetStartPoints()
    {
        SpawnCablePoint(0, this.transform, true);

        SpawnCablePoint(1, this.transform, true);

        lineRenderer.SetPosition(activeLineIndex, startingPos);

        lineRenderer.SetPosition(lineRenderer.positionCount - 1, this.transform.position);
    }

    void SetMiddleLinePoint()
    {
        lineRenderer.positionCount++;

        activeLineIndex++;

        // spanws a new point for cable to connect to
        SpawnCablePoint(activeLineIndex, this.transform, false);

        // Sets active position point to new cable point - this should be next to last point in index
        lineRenderer.SetPosition(activeLineIndex, this.transform.position);

        // Sets last position count to cable
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, this.gameObject.transform.position);
    }

    public void TriggerAction(GameObject _actionObj, eToolType _toolToUse)
    {
        if(!isConnected)
        {
            //base.TriggerAction(_actionObj, _toolToUse);

            SetMiddleLinePoint();
        }
    }

    public void StopAction(GameObject _actionObj)
    {
        //base.StopAction(_actionObj);

        //lineRenderer.SetPosition(lineRenderer.positionCount - 1, this.gameObject.transform.position);
    }

    public void ConnectionComplete(sCablePlug _cablePlug, Transform _tranform)
    {
        Debug.Log("Cable connection complete - cable is now connected at location " + _tranform.position.ToString());

        sPlayerCharacter.playerGlobal.ReturnGrabController().GrabReset();

        collider.enabled = false;

        //rb.velocity = Vector3.zero;

        //rb.constraints = RigidbodyConstraints.FreezeAll;

        isConnected = true;

        lineRenderer.material = materialConnected;

        this.gameObject.transform.position = _tranform.position;
        this.gameObject.transform.rotation = _tranform.rotation;

        lineRenderer.SetPosition(lineRenderer.positionCount - 1, _tranform.position);

        if(_cablePlug.IsInput)
        {
            
        }

        else
        {
            
        }
    }

    void SpawnCablePoint(int _index, Transform _transform, bool _rbKinematic)
    {
        GameObject tempObj;

        tempObj = Instantiate(pCablePoint, _transform.position, Quaternion.identity);

        tempObj.GetComponent<Rigidbody>().isKinematic = _rbKinematic;

        //cablePointsList.Add(tempObj);

        cablePointsList.Insert(_index, tempObj);
    }

    void SetJointBetweenCablePoints(GameObject _cablePoint, GameObject _parentConnection)
    {
        HingeJoint joint;

        joint = _cablePoint.AddComponent<HingeJoint>();

        joint.connectedBody = _parentConnection.GetComponent<Rigidbody>();
    }

    /*
    public void ConnectSourceInput(sPowerSource _source)
    {
        connectionHandler.ConnectInput(_source);
    }

    public void ConnectSupplyOutput(sPowerSupply _supply)
    {
        connectionHandler.ConnectOutput(_supply);
    }
    */
}
