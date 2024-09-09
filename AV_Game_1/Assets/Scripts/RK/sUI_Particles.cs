using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eParticleType { feather, acorn, }
public class sUI_Particles : MonoBehaviour
{
    GameManager gm;

    public eParticleType particleType;

    public Vector3 particleStart;

    Vector3 particleDestination;

    public float particleTime = 0.5f;

    Canvas canvas;

    // Start is called before the first frame update
    void Start()
    {

        gm = GameManager.gm;

        canvas = GetComponent<Canvas>();

        canvas.renderMode = RenderMode.ScreenSpaceCamera;

        canvas.worldCamera = Camera.main;

        switch(particleType)
        {
            case eParticleType.feather:

                //particleDestination = GameManager.gm.canvasGameplay.featherPanel.transform.position;

                break;

            case eParticleType.acorn:

                //particleDestination = GameManager.gm.canvasGameplay.moneyText.transform.position;

                break;
        }

        StartCoroutine(GoParticles());
    }

    IEnumerator GoParticles()
    {
        //Debug.Log("Particle movement started");

        float counter = 0f;

        while(counter < particleTime)
        {
            //this.transform.position = Vector3.Lerp(gm.rooster.transform.position, particleDestination, (counter / particleTime));

            //this.transform.LookAt(particleDestination);

            counter += Time.deltaTime;

            yield return null;
        }

        switch (particleType)
        {
            case eParticleType.feather:

                //GameManager.gm.FeatherAdd();

                break;

            case eParticleType.acorn:

                //GameManager.gm.bankManager.GetMoney(1, null);

                break;
        }

    }

}
