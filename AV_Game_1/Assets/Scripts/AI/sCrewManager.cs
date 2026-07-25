using System.Collections.Generic;
using UnityEngine;

public class sCrewManager : MonoBehaviour
{
    public LayerMask groundMask;

    List<sCrewMember> selectedCrew = new List<sCrewMember>();

    public float formationSpacing = 1.5f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SelectCrew();
        }

        if (Input.GetMouseButtonDown(1))
        {
            MoveSelectedCrew();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            AssignCommand(CrewCommand.Rigging);
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