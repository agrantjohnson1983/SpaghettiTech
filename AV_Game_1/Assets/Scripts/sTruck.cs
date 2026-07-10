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
        
    }

    public void Drive()
    {
        animator.SetTrigger("Drive");
    }

    public void SceneChange()
    {
        SceneManager.LoadScene("TestLevel");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<sPlayerCharacter>(out sPlayerCharacter _player))
        {
            Debug.Log("Triggering player entering truck");
            _player.ToggleTruckCamera(true, this.transform, camMoveTransform, 0.5f, camOffset);
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
