using UnityEngine;
using UnityEditor;

public class wPrefabSpawner : ScriptableWizard
{
    public GameObject prefab;

    public int rows = 5;
    public int columns = 5;

    public float spacing = 2;

    [MenuItem("Tools/Prefab Grid Wizard")]
    static void Open()
    {
        DisplayWizard<wPrefabSpawner>(
            "Prefab Grid Wizard",
            "Spawn");
    }

    void OnWizardCreate()
    {
        if (prefab == null)
            return;

        Undo.IncrementCurrentGroup();

        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                GameObject go =
                    (GameObject)PrefabUtility.InstantiatePrefab(prefab);

                Undo.RegisterCreatedObjectUndo(go, "Spawn Prefabs");

                go.transform.position = new Vector3(
                    x * spacing,
                    0,
                    y * spacing);
            }
        }
    }
}