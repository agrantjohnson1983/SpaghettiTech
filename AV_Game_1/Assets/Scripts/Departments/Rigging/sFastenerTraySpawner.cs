using UnityEngine;

// Spawns draggable bolt/nut icons into the tray, capped by whichever
// is smaller: how many this puzzle actually needs (one per hole), or
// how many the player currently has collected. If the player has more
// than needed (e.g. 5 bolts, 4 holes), only 4 spawn. If they have
// fewer than needed (e.g. 3 bolts, 4 holes), only 3 spawn and the
// puzzle cannot be completed until they collect more and come back.
//
// Spawning is delta-based rather than all-at-once: each time this
// canvas is enabled, it only spawns whatever additional icons are now
// possible beyond what it already has, so leaving with 3 spawned,
// collecting a 4th bolt elsewhere, and returning correctly tops the
// tray up to 4 without duplicating the first 3.
public class sFastenerTraySpawner : MonoBehaviour
{
    public GameObject boltPrefab;
    public GameObject nutPrefab;

    public Transform boltTrayContainer;
    public Transform nutTrayContainer;

    public sTrussBoltGameManager gameManager;

    int boltsSpawnedSoFar;
    int nutsSpawnedSoFar;

    void OnEnable()
    {
        SpawnAdditionalFastenersIfAvailable();
    }

    void SpawnAdditionalFastenersIfAvailable()
    {
        if (gameManager == null)
        {
            Debug.LogWarning("[" + this.name + "] sFastenerTraySpawner has no gameManager assigned - cannot determine how many holes need fasteners.");
            return;
        }

        int holesNeeded = gameManager.boltSlots.Count;

        int boltTarget = Mathf.Min(holesNeeded, uFasteners.numberOfBolts);
        int nutTarget = Mathf.Min(holesNeeded, uFasteners.numberOfNuts);

        int boltsToSpawnNow = Mathf.Max(0, boltTarget - boltsSpawnedSoFar);
        int nutsToSpawnNow = Mathf.Max(0, nutTarget - nutsSpawnedSoFar);

        for (int i = 0; i < boltsToSpawnNow; i++)
        {
            if (boltPrefab != null && boltTrayContainer != null)
            {
               /* Vector3 offset;// = new Vector3();

                Vector2 randomSpot = Random.insideUnitCircle;

                offset = new Vector3(randomSpot.x, randomSpot.y, 0f);*/

                GameObject tempObj;

                tempObj = Instantiate(boltPrefab, boltTrayContainer);

                //tempObj.transform.localPosition = Vector3.zero;

                //tempObj.transform.position = boltTrayContainer.transform.position;// + offset;

                //tempObj.SetParent(originalParent, true);
                //rectTransform.anchoredPosition = originalAnchoredPosition;
            }
        }

        for (int i = 0; i < nutsToSpawnNow; i++)
        {
            if (nutPrefab != null && nutTrayContainer != null)
            {
                //Vector3 offset;// = new Vector3();

                //Vector2 randomSpot = Random.insideUnitCircle * 10f;

                //offset = new Vector3(randomSpot.x, randomSpot.y, 0f);

                GameObject tempObj;

                tempObj = Instantiate(nutPrefab, nutTrayContainer);

                //tempObj.transform.localPosition = Vector3.zero;

                //tempObj.transform.position = nutTrayContainer.transform.position;// + offset;
            }
        }

        boltsSpawnedSoFar += boltsToSpawnNow;
        nutsSpawnedSoFar += nutsToSpawnNow;

        Debug.Log("[" + this.name + "] Spawned " + boltsToSpawnNow + " bolt(s) and " + nutsToSpawnNow
            + " nut(s) this activation - total so far: " + boltsSpawnedSoFar + " bolt(s), "
            + nutsSpawnedSoFar + " nut(s), puzzle needs " + holesNeeded + " of each.");
    }
}
