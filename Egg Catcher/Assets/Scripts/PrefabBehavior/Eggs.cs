using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eggs : MonoBehaviour {
    private readonly float minStartTime = 1.0f;
    private readonly float maxStartTime = 2.5f;
    public GameObject eggPrefab;
    public GameObject goldenEggPrefab;

    // for reference eggs have a 90% chance
    private readonly float goldenEggWeight = .05f; // (5% chance)

    // Start is called before the first frame update
    void Start() {
        StartCoroutine( EggTimer() );
    }

    void SpawnEgg() {
        Vector2 chickenPos = GameObject.FindGameObjectWithTag("Hen").transform.position;
        Vector2 eggPos = chickenPos;
        eggPos.y = chickenPos.y - 1.0f;

        if (Random.Range(0f, 1f) >= goldenEggWeight) {
            GameObject egg = Instantiate(eggPrefab) as GameObject;
            egg.transform.position = eggPos;
        } else {
            GameObject goldenEgg = Instantiate(goldenEggPrefab) as GameObject;
            goldenEgg.transform.position = eggPos;
        }
    }

    IEnumerator EggTimer() {
        while(true) {
            yield return new WaitForSeconds( Random.Range(minStartTime, maxStartTime) );
            SpawnEgg();
        }
    }
}
