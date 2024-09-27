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

    //public Sprite

    // Start is called before the first frame update
    void Start()
    {
        cableList = new List<GameObject>();

        GameObject cableInTemp;

        sCablePlug cableIn;

        cableInTemp = Instantiate(pCableIn, this.transform);

        cableIn = cableInTemp.GetComponent<sCablePlug>();

        cableList.Add(cableInTemp);

        for (int i = 0; i < numerOfSegments; i++)
        {
            GameObject tempObj;
            ConfigurableJoint tempJoint;
            Rigidbody tempRB;

            tempRB = cableList[i].GetComponent<Rigidbody>();

            tempObj = Instantiate(pCableSegment, cableList[i].transform);

            tempJoint = tempObj.GetComponent<ConfigurableJoint>();

            tempJoint.connectedBody = tempRB;

            //tempObj.transform.position = Vector3.zero;
            //tempObj.transform.rotation = cableList[i].transform.rotation;

            cableList.Add(tempObj);

        }

        // adds last cable to end of list and last transform in list
        GameObject tempCableOut;

        sCablePlug cableOut;

        ConfigurableJoint _joint;
        
        Rigidbody _rb;

        _rb = cableList[cableList.Count - 1].GetComponent<Rigidbody>();

        tempCableOut = Instantiate(pCableOut, cableList[cableList.Count - 1].transform);

        cableOut = tempCableOut.GetComponent<sCablePlug>();

        _joint = tempCableOut.GetComponent<ConfigurableJoint>();

        _joint.connectedBody = _rb;

        cableList.Add(tempCableOut);

        cableIn.SetPlugOtherEnd(cableOut);
        cableOut.SetPlugOtherEnd(cableIn);

        MovingUI.SetActive(false);
    }

    public void HalfConnect()
    {
        //isConnected = true;

        //MovingUI.SetActive(true);
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

        StopCoroutine(MoveConnectionUI());

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

        MovingUI.SetActive(true);

        //int currentIndex = 0;
        int nextIndex;


        for (int i = 0; i < cableList.Count-1; i++)
        {
            nextIndex = (int)i + 1;

            Debug.Log("Cable Moving from cable list index " + i + " to " + nextIndex);

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

        StartCoroutine(MoveConnectionUI());
    }

    // Update is called once per frame
    void Update()
    {
        if(isConnected)
        {
            for (int i = 0; i < cableList.Count; i++)
            {

            }
        }
    }
}
