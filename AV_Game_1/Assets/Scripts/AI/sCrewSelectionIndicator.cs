using UnityEngine;

public class sCrewSelectionIndicator : MonoBehaviour
{
    [Header("References")]
    public MeshRenderer ringRenderer;


    [Header("Materials")]
    public Material idleMaterial;
    public Material selectedMaterial;
    public Material previewMaterial;
    public Material workingMaterial;
    public Material errorMaterial;


    void Awake()
    {
        Hide();
    }


    public void ShowSelected()
    {
        Show();

        if (selectedMaterial != null)
            ringRenderer.material = selectedMaterial;
    }


    public void ShowPreview()
    {
        Show();

        if (previewMaterial != null)
            ringRenderer.material = previewMaterial;
    }


    public void ShowWorking()
    {
        Show();

        if (workingMaterial != null)
            ringRenderer.material = workingMaterial;
    }


    public void ShowError()
    {
        Show();

        if (errorMaterial != null)
            ringRenderer.material = errorMaterial;
    }


    public void Hide()
    {
        gameObject.SetActive(false);
    }


    void Show()
    {
        gameObject.SetActive(true);
    }
}