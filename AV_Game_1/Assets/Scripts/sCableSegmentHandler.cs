using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sCableSegmentHandler : MonoBehaviour
{
    public GameObject pCableSegment;

    public GameObject pCableIn, pCableOut;

    public int numerOfSegments = 20;

    public Material connectionCompleteMaterial, connectionHalfMaterial;

    List<GameObject> cableList;

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
    }

    public void HalfConnect()
    {
        for (int i = 1; i < cableList.Count-2; i++)
        {
            cableList[i].GetComponent<MeshRenderer>().material = connectionHalfMaterial;
        }   
    }

    public void ConnectionComplete()
    {
        //Debug.Log("Cable connection complete - cable is now connected at location " + _tranform.position.ToString());

        GameManager.gm.ReturnCurrentPlayer().ReturnGrabController().GrabReset();

        //this.gameObject.transform.position = _tranform.position;
        //this.gameObject.transform.rotation = _tranform.rotation;

        for (int i = 1; i < cableList.Count-2; i++)
        {
            cableList[i].GetComponent<MeshRenderer>().material = connectionCompleteMaterial;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
