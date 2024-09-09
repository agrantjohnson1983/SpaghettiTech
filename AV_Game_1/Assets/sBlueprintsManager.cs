using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sBlueprintsManager : MonoBehaviour
{
    public static sBlueprintsManager blueprintManagerGlobal;

    public GameObject[] blueprintSetups;

    int activeBlueprintIndex = 0;

    private void Awake()
    {
        blueprintManagerGlobal = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < blueprintSetups.Length; i++)
        {
            blueprintSetups[i].SetActive(false);
        }

        blueprintSetups[activeBlueprintIndex].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchBlueprint(bool _isLeftButton)
    {
        blueprintSetups[activeBlueprintIndex].SetActive(false);

        if (_isLeftButton)
        {
            // setups -1
            if (activeBlueprintIndex <= 0)
            {
                activeBlueprintIndex = blueprintSetups.Length-1;
            }

            else
            {
                activeBlueprintIndex--;
            }

        }

        else
        {
            // setups +1
            if (activeBlueprintIndex >= blueprintSetups.Length)
            {
                activeBlueprintIndex = 0;
            }

            else
            {
                activeBlueprintIndex++;
                if(activeBlueprintIndex >= blueprintSetups.Length)
                {
                    activeBlueprintIndex = 0;
                }
            }
        }

        Debug.Log("Setting active with index of " + activeBlueprintIndex);
        blueprintSetups[activeBlueprintIndex].SetActive(true);
    }
}
