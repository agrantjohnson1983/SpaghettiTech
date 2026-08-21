using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sCameraOverhead : MonoBehaviour
{
    private void OnEnable()
    {
        GoZoomOut();
    }

    void GoZoomOut()
    {
        //if (sPlayerCharacter.playerCharacterGlobal == null)
        //    return;

        Vector3 startPos = Vector3.zero;

        Vector3 endPos = this.transform.position;

        StartCoroutine(CameraMovement(startPos, endPos, 1f));
    }

    IEnumerator CameraMovement(Vector3 _startPos, Vector3 _endPos, float _time)
    {
        float counter = 0f;

        while (counter < _time)
        {
            this.transform.position = Vector3.Lerp(_startPos, _endPos, (counter / _time));

            counter += Time.deltaTime;

            yield return null;
        }
    }
}
