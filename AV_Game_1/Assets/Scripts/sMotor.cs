using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sMotor : sRigGear
{
    Rigidbody rb;

    LineRenderer lineChain;

    float heightToRaise = 50;

    public float motorRaiseTime = 10f;

    public float motorRaiseSpeed = 10f;

    bool chainsUp = false;

    bool isMoving = false;

    private void OnEnable()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        lineChain = GetComponent<LineRenderer>();

        sRiggingManager.riggingManger.motorList.Add(this);
        //SetChainsLine();

        //StartMotorRaise();
    }

    // Update is called once per frame
    void Update()
    {
        if(isMoving)
        {
            lineChain.SetPosition(0, this.gameObject.transform.position);
        }
    }

    public void TriggerAction(GameObject _actionObj, eToolType _toolToUse)
    {

    }

    public void StopAction(GameObject _actionObj)
    {

    }

    // This handles the movement raising the motor
    public void StartMotorRaise()
    {
        StartCoroutine(MotorRaiseMovement());
    }
    IEnumerator MotorRaiseMovement()
    {
        Debug.Log("Raising Motor!");

        isMoving = true;

        float counter = 0f;

        Vector3 startPos = this.gameObject.transform.position;

        Vector3 destination = new Vector3(rb.transform.position.x, rb.transform.position.y + heightToRaise, rb.transform.position.z);

        while(counter < motorRaiseTime)
        {
            rb.gameObject.transform.position = Vector3.Lerp(startPos, destination, (counter / motorRaiseTime));

            counter += Time.deltaTime;

            yield return null;
        }

        isMoving = false;
    }

    // This sets the chains to start before the motor moves
    void SetChainsLine()
    {
        lineChain.SetVertexCount(2);

        lineChain.SetPosition(0, this.gameObject.transform.position);
        lineChain.SetPosition(1, this.gameObject.transform.position + Vector3.up * 50f);
    }

    // This sets the height the motor will raise to
    public void SetMotorRaiseHeight(float _amount)
    {
        heightToRaise = _amount;
    }
}
