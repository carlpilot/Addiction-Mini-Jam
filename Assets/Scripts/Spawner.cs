using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    GameManager gm;

    [Header("Spawning Options")]
    public GameObject prefabToSpawn;
    public float spawnRadius = 1.5f;
    public float spawnDelay = 6.0f;
    public int numToSpawnAtOnce = 1;

    [Header ("Visual Options")]
    public float MaxSpinRateDPS = 360.0f;
    public AnimationCurve spinCurve;
    public Transform spinner;

    float spawnTimer = 0.0f;

    private void Start () {
        gm = FindObjectOfType<GameManager> ();
        spawnDelay *= Random.Range (0.9f, 1.1f);
        spawnTimer = Random.Range (0, spawnDelay);
    }

    private void Update () {
        spawnTimer += Time.deltaTime;

        spinner.transform.Rotate (Vector3.forward * spinCurve.Evaluate (spawnTimer / spawnDelay) * MaxSpinRateDPS * Time.deltaTime);

        if (spawnTimer > spawnDelay) Spawn ();
    }

    void Spawn () {
        spawnTimer = 0;
        if (gm.numMonstersCurrentlyInScene >= gm.monsterCap) return;
        for (int i = 0; i < numToSpawnAtOnce; i++) {
            GameObject newMonster = Instantiate (prefabToSpawn);
            newMonster.transform.position = transform.position + (Vector3) Random.insideUnitCircle.normalized * spawnRadius;
        }
    }
}
