using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sMouseClickController : MonoBehaviour
{
    Camera cam;
    public LayerMask mask_Clickable;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMouseClicking();   
    }

    void HandleMouseClicking()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, 100, mask_Clickable))
            {
                Debug.Log(hit.transform.name.ToString() + " Was clicked by mouse");

                if(hit.transform.gameObject.TryGetComponent<iClickable>(out iClickable _clickable))
                {
                    _clickable.OnClick();
                }
            }

        }
    }
}
