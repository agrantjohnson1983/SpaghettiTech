using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class sKitBuilder : MonoBehaviour
{
    public UnityEngine.GameObject canvasKit;

    public string kitName;

    List<SO_ItemData> itemList;

    public GameObject pBox;

    public TextMeshProUGUI spawnText;

    

    // Start is called before the first frame update
    void Start()
    {
        canvasKit.SetActive(false);

        itemList = new List<SO_ItemData>();
    }

    public void AddToBox(SO_ItemData _itemToAdd)
    {
        itemList.Add(_itemToAdd);

        spawnText.text = spawnText.text + " and " + _itemToAdd.itemName;

    }

    public void SpawnGear()
    {
        if(itemList.Count == 0)
            return;

        ScriptableObject _tempObj = ScriptableObject.CreateInstance(typeof(SO_BoxData));

        SO_BoxData _newBox = ((SO_BoxData)_tempObj);

        _newBox.numberOfSlots = itemList.Count;

        _newBox.boxName = kitName;

        _newBox.boxPrefab = pBox;

        _newBox.startingItemData = new List<SO_ItemData>();

        if(pBox != null)
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                //Debug.Log("added item for " + i);
                _newBox.startingItemData.Add(itemList[i]);
            }

            SpawnBox(_newBox, this.transform.position + Random.onUnitSphere * 0.5f, Quaternion.identity);
        }

        else
        {
            for (int s = 0; s < itemList.Count; s++)
            {
                Instantiate(itemList[s].prefabItem, this.transform.position + Random.onUnitSphere, Quaternion.identity);
            }
        }

        Destroy(this.gameObject);
    }

    sBox SpawnBox(SO_BoxData data, Vector3 position, Quaternion rotation)
    {
        if (data == null || data.boxPrefab == null)
        {
            Debug.LogWarning("sBoxSpawner: Missing box data or prefab reference.");
            return null;
        }

        UnityEngine.GameObject boxObj = Instantiate(data.boxPrefab, position, rotation);
        sBox box = boxObj.GetComponent<sBox>();

        if (box == null)
        {
            Debug.LogWarning($"sBoxSpawner: Spawned prefab for {data.boxName} has no sBox component.");
            return null;
        }

        box.Initialize(data);
        return box;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            canvasKit.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canvasKit.SetActive(false);
        }
    }
}
