using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eggs : MonoBehaviour {
    private readonly float minStartTime = 1.0f;
    private readonly float maxStartTime = 2.5f;
    public GameObject eggPrefab;
    public GameObject goldenEggPrefab;

    // for reference eggs have a 95% chance
    private readonly float goldenEggWeight = .05f; // (5% chance)

    void Start() {
        StartCoroutine( EggTimer() );
    }

    // spawning an egg
    // i kind of wanted to implement the same functionality in egg spawning
    // as i have implemented in the bomb spawning, where the "clamp" of the random spawning interval
    // is decreased every 10 seconds to make it more challenging, but
    // it already gets a little too hectic with how many bombs are spawning -
    // it would be almost impossible to get all of the eggs without me
    // having to allow for longer boost / permeability abilities
    void SpawnEgg() {
        // finding chicken position to spawn egg under it
        Vector2 chickenPos = GameObject.FindGameObjectWithTag("Hen").transform.position;
        Vector2 eggPos = chickenPos;
        eggPos.y = chickenPos.y - 1.0f;

        // 95% chance to spawn a normal egg
        if (Random.Range(0f, 1f) >= goldenEggWeight) {
            // spawns instance of prefab normal egg as gameobject
            GameObject egg = Instantiate(eggPrefab) as GameObject;
            egg.transform.position = eggPos;
        // 5% chance to spawn a golden egg
        } else {
            // spawns instance of prefab golden egg as gameobject
            GameObject goldenEgg = Instantiate(goldenEggPrefab) as GameObject;
            goldenEgg.transform.position = eggPos;
        }
    }

    // spawns eggs at clamped random interval
    IEnumerator EggTimer() {
        while(true) {
            // waits random time to spawn egg
            yield return new WaitForSeconds( Random.Range(minStartTime, maxStartTime) );
            SpawnEgg();
        }
    }
}
