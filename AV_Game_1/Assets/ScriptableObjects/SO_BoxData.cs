using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SO_BoxData_", menuName = "Boxes/Box Data")]
public class SO_BoxData : ScriptableObject
{
    [Header("Identity")]
    public string boxName;
    public GameObject boxPrefab; // shared sBox prefab (base mesh, ring UI, panel, etc.)

    [Header("Scale")]
    [Tooltip("Applied to pModel's local scale. E.g. (2,1,1) for a 2x1 cable box, (1,1,1) for a motor box.")]
    public Vector3 modelScale = Vector3.one;

    [Tooltip("If true, ring/select UI scale is derived automatically from modelScale (max of x/z). If false, use ringUIScaleOverride.")]
    public bool autoScaleRingUI = true;
    public float ringUIScaleOverride = 1f;

    [Header("Inventory")]
    public int numberOfSlots;
    public List<SO_ItemData> startingItemData;

    [Tooltip("If true, the panel's z-offset flips sign based on the player's position relative to the box, so it always opens on the far side from the player. If false, uses inventoryPanelOffset exactly as set below.")]
    public bool useDynamicZOffset = true;

    [Header("Visuals")]
    public Material materialBoxClosed;
    public Material materialBoxOpen;
    public Material materialBoxEmpty;
    public Texture2D boxSelectMouseImage;

    [Header("UI")]
    public float UI_ToggleDistance = 5f;
    public Vector3 ui_Img_Offset;
    public Vector3 ui_Text_Offset;
    public Vector3 inventoryPanelOffset;
}