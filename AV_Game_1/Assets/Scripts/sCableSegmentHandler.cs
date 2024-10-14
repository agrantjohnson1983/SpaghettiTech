using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sCableSegmentHandler : MonoBehaviour
{
    public GameObject pCableSegment;

    public GameObject pCableIn, pCableOut;

    public GameObject MovingUI;

    public float movingUItimePerSegment = 1f;

    public float movingUIspeed = 5f;

    public int numerOfSegments = 20;

    public Material startingMaterial, connectionCompleteMaterial, connectionHalfMaterial;

    List<GameObject> cableList;

    bool isConnected = false;

    public Vector3 spawnOffset = new Vector3(0,1,0);

    //public Sprite

    // Start is called before the first frame update
    void Start()
    {
        SpawnCable();
    }

    void SpawnCable()
    {
        cableList = new List<GameObject>();

        GameObject cableInTemp;

        sCablePlug cableIn;

        // Spawns the cable INPUT plug
        cableInTemp = Instantiate(pCableIn, this.transform);

        cableInTemp.transform.position += spawnOffset;

        // gets a reference to the plug
        cableIn = cableInTemp.GetComponent<sCablePlug>();

        // add plug reference to cable list
        cableList.Add(cableInTemp);

        // Spawns the segments of the cable
        for (int i = 0; i < numerOfSegments; i++)
        {
            GameObject tempObj;
            ConfigurableJoint tempJoint;
            Rigidbody tempRB;

            // gets a reference to the RB of the cable list - this will start with the plug in
            tempRB = cableList[i].GetComponent<Rigidbody>();

            // spawns a new cable segment
            tempObj = Instantiate(pCableSegment, cableList[i].transform);

            tempObj.transform.position += spawnOffset;

            // gets a reference to the joint on the newly spawned cable segment
            tempJoint = tempObj.GetComponent<ConfigurableJoint>();

            // sets the connected body of the joint as the temp RB
            tempJoint.connectedBody = tempRB;

            //tempRB.velocity = Vector3.zero;
            //tempObj.transform.position = Vector3.zero;
            //tempObj.transform.rotation = cableList[i].transform.rotation;

            cableList.Add(tempObj);

        }

        // adds last cable to end of list and last transform in list
        GameObject tempCableOut;

        sCablePlug cableOut;

        ConfigurableJoint _joint;

        Rigidbody _rb;

        // gets a reference to the last cable segments RB
        _rb = cableList[cableList.Count - 1].GetComponent<Rigidbody>();

        // spawns the output plug
        tempCableOut = Instantiate(pCableOut, cableList[cableList.Count - 1].transform);

        tempCableOut.transform.position += spawnOffset;

        // gets reference to the cable plug in the the newly spawn tempCableOut object
        cableOut = tempCableOut.GetComponent<sCablePlug>();

        // gets a reference to the joint in the cable out
        _joint = tempCableOut.GetComponent<ConfigurableJoint>();

        // sets cable out joint to the last sements RB
        _joint.connectedBody = _rb;

        //_rb.velocity = Vector3.zero;

        // adds cable out to the cable list
        cableList.Add(tempCableOut);

        // Gives the cable in a ref to the cable out
        cableIn.SetPlugOtherEnd(cableOut);

        // Gives the cable out a ref to the cable in
        cableOut.SetPlugOtherEnd(cableIn);

        // Turns off the moving UI
        MovingUI.SetActive(false);
    }

    public void HalfConnect()
    {
        //isConnected = true;

        MovingUI.SetActive(true);

        StartCoroutine(MoveConnectionUI());

        for (int i = 1; i < cableList.Count-1; i++)
        {
            cableList[i].GetComponent<MeshRenderer>().material = connectionHalfMaterial;
        }   
    }

    // This will disconnect the cable from the connection plate, changes the color of segments and turns of moving UI
    public void Disconnect()
    {
        //isConnected = false;

        StopAllCoroutines();

        MovingUI.SetActive(false);

        for (int i = 1; i < cableList.Count-1; i++)
        {
            cableList[i].GetComponent<MeshRenderer>().material = startingMaterial;
        }
    }

    public void ConnectionComplete()
    {
        //Debug.Log("Cable connection complete - cable is now connected at location " + _tranform.position.ToString());

        GameManager.gm.ReturnCurrentPlayer().ReturnGrabController().GrabReset();

        //this.gameObject.transform.position = _tranform.position;
        //this.gameObject.transform.rotation = _tranform.rotation;

        for (int i = 1; i < cableList.Count-1; i++)
        {
            cableList[i].GetComponent<MeshRenderer>().material = connectionCompleteMaterial;
        }
    }

    // This is for the movement of the overhead UI
    // This is recursive and keeps looping till it's told to stop
    IEnumerator MoveConnectionUI()
    {

        if(cableList == null)
        {
            Debug.Log("Cable List Null");
            yield return null;
        }

        //MovingUI.SetActive(true);

        //int currentIndex = 0;
        int nextIndex;


        for (int i = 0; i < cableList.Count-1; i++)
        {
            nextIndex = (int)i + 1;

            //Debug.Log("Cable Moving from cable list index " + i + " to " + nextIndex);

            int counter = 0;

            while (counter < movingUItimePerSegment)
            {
                MovingUI.transform.position = Vector3.Lerp(cableList[i].transform.position, cableList[nextIndex].transform.position, (counter/movingUItimePerSegment));

                counter++;

                yield return null;
            }

            //currentIndex++;
            //nextIndex++;

            // Needs to flip index backwards 
        }

        //currentIndex - 1;

        // current index should come out as cable list count

        // Reverse

        for (int i = cableList.Count-1; i > 0; i--)
        {
            nextIndex = (int)i - 1;

            int counter = 0;

            while (counter < movingUItimePerSegment)
            {
                MovingUI.transform.position = Vector3.Lerp(cableList[i].transform.position, cableList[nextIndex].transform.position, (counter / movingUItimePerSegment));

                counter++;

                yield return null;
            }

            //currentIndex--;
            //nextIndex--;
        }

        MovingUI.SetActive(true);

        StartCoroutine(MoveConnectionUI());
    }

    // Update is called once per frame
    void Update()
    {

    }
}
