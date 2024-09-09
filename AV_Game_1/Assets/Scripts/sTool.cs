using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eToolType { ratchet, NONE }
public class sTool : MonoBehaviour
{
    public eToolType typeOfTool;

    public SO_ItemData itemData;

    public GameObject toolModel;

}
