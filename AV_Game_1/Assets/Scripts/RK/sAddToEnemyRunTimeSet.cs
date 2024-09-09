using System.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sAddToEnemyRunTimeSet : MonoBehaviour
{
    //public soEnemyBehaviorRuntimeSet enemyList;

    //sENEMY_BASE enemyBase;

    private void Awake()
    {
        //enemyBase = GetComponent<sENEMY_BASE>();
    }

    public void OnEnable()
    {

        //enemyList.AddToList(enemyBase);

    }

    private void OnDisable()
    {

        //enemyList.RemoveFromList(enemyBase);

    }

}
