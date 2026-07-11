using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sStairs : MonoBehaviour
{
    public Transform stairsTopTransform, stairsBottomTransform;

    public bool goesUp = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void TransportPlayer(GameObject _playerObject, Transform _location)
    {
        _playerObject.transform.position = _location.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<sPlayerCharacter>(out sPlayerCharacter _player))
        {
            if(goesUp)
            {
                TransportPlayer(_player.gameObject, stairsTopTransform);
            }

            else
            {
                TransportPlayer(_player.gameObject, stairsBottomTransform);
            }
        }
    }
}
