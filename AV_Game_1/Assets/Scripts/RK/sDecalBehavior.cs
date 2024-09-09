using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sDecalBehavior : MonoBehaviour
{

    public float lifeTime = 5f;

    // Start is called before the first frame update
    void Start()
    {
        //transform.parent = GameManager.gm.pGROUP_CHICKENS;

        Destroy(this.gameObject, lifeTime);
    }

}
