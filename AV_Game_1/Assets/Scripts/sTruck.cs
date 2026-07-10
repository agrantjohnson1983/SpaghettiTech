using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sTruck : MonoBehaviour
{
    public Vector3 camOffset;

    public Transform camMoveTransform;

    public static bool isLoaded = false;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        if(GameManager.gm.GetGameMode() == eGameMode.gig)
        {
            
        }
    }

    public void Drive()
    {
        animator.SetTrigger("Drive");

        sGigManager.gigManagerGlobal.TruckDrive();
    }

    // This is getting called by an animator event
    public void SceneChange()
    {
        GameManager.gm.StartGig();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<sPlayerCharacter>(out sPlayerCharacter _player))
        {
            Debug.Log("Triggering player entering truck");
            _player.ToggleTruckCamera(true, this.transform, camMoveTransform, 0.5f, camOffset);
            return;
        }

        else
        {
            Debug.Log("Adding " + other.gameObject + " to gig mgr");
            sGigManager.gigManagerGlobal.AddItemToGig(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
        if (other.TryGetComponent<sPlayerCharacter>(out sPlayerCharacter _player))
        {
            Debug.Log("Triggering player exiting truck");
            _player.ToggleTruckCamera(false, this.transform, null, 0.5f, camOffset);
        }
    }
}
