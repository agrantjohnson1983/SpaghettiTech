using System.Collections.Generic;
using UnityEngine;

public class sCrewManager : MonoBehaviour
{
    public LayerMask groundMask;

    List<sCrewMember> selectedCrew = new List<sCrewMember>();

    public float formationSpacing = 1.5f;

    [Header("Selection Box")]
    [SerializeField] sSelectionBoxUI selectionBox;

    [SerializeField] float dragThreshold = 10f;

    bool isDragging;

    Vector2 dragStart;

    List<sCrewMember> allCrew = new();

    List<sCrewMember> previewCrew = new List<sCrewMember>();

    public SO_AudioEventChannel soAudio;

    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    SelectCrew();
        //}

        HandleSelection();

        if (Input.GetMouseButtonDown(1))
        {
            MoveSelectedCrew();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            AssignCommand(CrewCommand.Rigging);
        }
    }

    public void RegisterCrew(sCrewMember crew)
    {
        if (!allCrew.Contains(crew))
            allCrew.Add(crew);
    }

    public void UnregisterCrew(sCrewMember crew)
    {
        allCrew.Remove(crew);
    }

    void UpdateSelectionPreview(Vector2 start, Vector2 end)
    {
        Rect rect = new Rect(
            Mathf.Min(start.x, end.x),
            Mathf.Min(start.y, end.y),
            Mathf.Abs(start.x - end.x),
            Mathf.Abs(start.y - end.y)
        );


        foreach (var crew in allCrew)
        {
            Vector3 screenPos =
                Camera.main.WorldToScreenPoint(
                    crew.transform.position
                );


            bool inside =
                rect.Contains(screenPos);


            if (inside)
            {
                if (!previewCrew.Contains(crew))
                {
                    previewCrew.Add(crew);
                    crew.SetPreview(true);
                }
            }
            else
            {
                if (previewCrew.Contains(crew))
                {
                    previewCrew.Remove(crew);
                    crew.SetPreview(false);
                }
            }
        }
    }

    void HandleSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragStart = Input.mousePosition;
            isDragging = false;
        }

        if (Input.GetMouseButton(0))
        {
            if (!isDragging)
            {
                if (Vector2.Distance(dragStart, Input.mousePosition) > dragThreshold)
                {
                    isDragging = true;
                    selectionBox.Begin(dragStart);
                }
            }

            if (isDragging)
            {
                selectionBox.UpdateBox(Input.mousePosition);

                UpdateSelectionPreview(
                    dragStart,
                    Input.mousePosition
                );
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (isDragging)
            {
                selectionBox.Hide();

                ClearPreview();

                BoxSelectCrew(
                    dragStart,
                    Input.mousePosition
                );
            }

            else
            {
                SelectCrew();
            }
        }
    }

    void ClearPreview()
    {
        foreach (var crew in previewCrew)
        {
            crew.SetPreview(false);
        }

        previewCrew.Clear();
    }

    void BoxSelectCrew(Vector2 start, Vector2 end)
    {
        bool shiftHeld =
            Input.GetKey(KeyCode.LeftShift) ||
            Input.GetKey(KeyCode.RightShift);

        if (!shiftHeld)
            DeselectAll();

        Rect rect = new Rect(
            Mathf.Min(start.x, end.x),
            Mathf.Min(start.y, end.y),
            Mathf.Abs(start.x - end.x),
            Mathf.Abs(start.y - end.y)
        );

        sCrewMember[] crewMembers = FindObjectsOfType<sCrewMember>();

        foreach (sCrewMember crew in allCrew)
        {
            Vector3 screenPos =
                Camera.main.WorldToScreenPoint(
                    crew.transform.position
                );

            if (screenPos.z < 0)
                continue;


            // Give characters a little selection padding
            Rect expandedRect = new Rect(
                rect.x - 25,
                rect.y - 25,
                rect.width + 50,
                rect.height + 50
            );


            if (expandedRect.Contains(screenPos))
            {
                SelectCrew(crew);
            }
        }
    }

    void SelectCrew()
    {
        bool shiftHeld =
            Input.GetKey(KeyCode.LeftShift) ||
            Input.GetKey(KeyCode.RightShift);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit hit))
            return;

        sCrewMember crew = hit.collider.GetComponentInParent<sCrewMember>();

        // Clicked on a crew member
        if (crew != null)
        {
            if (!shiftHeld)
                DeselectAll();

            if (selectedCrew.Contains(crew))
            {
                // Shift-click removes from selection
                if (shiftHeld)
                    DeselectCrew(crew);
            }
            else
            {
                SelectCrew(crew);
            }
        }
        // Clicked on empty ground
        else
        {
            if (!shiftHeld)
                DeselectAll();
        }
    }

    void SelectCrew(sCrewMember crew)
    {
        if (selectedCrew.Contains(crew))
            return;

        selectedCrew.Add(crew);
        crew.Select();
    }

    void DeselectCrew(sCrewMember crew)
    {
        if (!selectedCrew.Contains(crew))
            return;

        selectedCrew.Remove(crew);
        crew.Deselect();
    }

    void DeselectAll()
    {
        foreach (var crew in selectedCrew)
            crew.Deselect();

        selectedCrew.Clear();
    }

    void MoveSelectedCrew()
    {
        Debug.Log("Trying to move crew");

        if (selectedCrew.Count == 0)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, groundMask))
        {
            Debug.Log("Moving crew to : " + hit.point);
            //selectedCrew.MoveTo(hit.point);

            Vector3 center = hit.point;

            for (int i = 0; i < selectedCrew.Count; i++)
            {
                int row = i / 3;
                int col = i % 3;

                Vector3 offset = new Vector3(
                    (col - 1) * formationSpacing,
                    0,
                    -row * formationSpacing
                );

                selectedCrew[i].MoveTo(center + offset);
            }

            //foreach (var crew in selectedCrew)
            //{
            //    crew.MoveTo(hit.point);
            //}
        }
    }

    void AssignCommand(CrewCommand command)
    {
        foreach (var crew in selectedCrew)
        {
            crew.AssignCommand(command);
        }
    }
}