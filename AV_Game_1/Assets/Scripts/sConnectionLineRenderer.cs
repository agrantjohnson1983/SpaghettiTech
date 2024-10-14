using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sConnectionLineRenderer : MonoBehaviour
{
    LineRenderer lr;

    GameObject startObj = null;
    GameObject endObj = null;

    int index;

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();

        lr.positionCount = 2;

        lr.enabled = false;
    }

    // This gets called after this object is spawned and sets the start and end obj
    public void SetConnectionObjects(GameObject _startObj, GameObject _endObj, int _index)
    {
        //Debug.Log("Setting start object to " + _startObj.name);

        lr.enabled = true;

        startObj = _startObj;

        //Debug.Log("Setting end object to " + _endObj.name);

        endObj = _endObj;

        index = _index;
    }

    // Update is called once per frame
    void Update()
    {
        // quick null check for start and end obj
        if(startObj && endObj)
        {
            // Sets the lowest point in the line renderer to the start point object conerted from screen to world space - this should be a connection available button
            //lr.SetPosition(0, Camera.main.ScreenToWorldPoint(startObj.transform.position));
            lr.SetPosition(0, startObj.transform.position);

            // Sets the end point of the line renderer to the end object position converted froms screen to world space - this should be the channel button
            //lr.SetPosition(1, Camera.main.ScreenToWorldPoint(endObj.transform.position));
            lr.SetPosition(1, endObj.transform.position);
        }
    }
}
