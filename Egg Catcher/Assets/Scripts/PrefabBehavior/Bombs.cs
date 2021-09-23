using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bombs : MonoBehaviour {
    public GameObject bombPrefab;
    private GameObject bomb;

    private int maxSpawnAttempts = 100;
    // this was from before i found out about the layer collision matrix in the project settings,
    // so i'm not sure this actually does anything on its own without the layer collision matrix
    // settings, but it works like i wanted it to
    private static int layerId = 9;
    private int layerMask = 1 << layerId;

    private Vector2 bombPos;
    private float randX;
    public float minSpawnTime = 1.1f; // last 10 seconds at 0.1f
    public float maxSpawnTime = 3.0f; // last 10 seconds at 0.5f
    private readonly float maxXVal = 9.0f;
    private shake shake;
    float startTime = 0.0f;

    void Start() {
        StartCoroutine(BombTimer());
        InvokeRepeating("SpawnCluster", 5.0f, 5.0f);
        shake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<shake>();
    }

    private void Update() {
        // this occurs 5 times while game is played (every 10 seconds over 60 seconds)
        if (startTime < 10.0f) {
            startTime += Time.deltaTime;
        // every 10 seconds, decrease the minimum AND maximum spawn times between bombs
        // this increases difficulty as the game goes later
        } else if (startTime >= 10.0f) {
            minSpawnTime -= 0.2f;
            maxSpawnTime -= 0.5f;
            // shake the screen slightly to indicate when game is getting slightly more difficult
            shake.CamShake();
            // start counting again
            startTime = 0.0f;
        }
    }

    void SpawnBomb() {
        // chooses random x position to spawn bomb
        randX = Random.Range(-maxXVal, maxXVal);

        // bombs always spawn offscreen at 6.0f
        bombPos.y = 6.0f;
        bombPos.x = randX;

        // attempts to spawn bomb away from other bombs 'maxSpawnAttempts' times
        for (int i = 0; i <= maxSpawnAttempts; i++) {
            // if there are no other bombs in a 1f radius from the current randomly chosen bomb position
            if (Physics2D.OverlapCircle(bombPos, 1f, layerMask) == null) {
                // spawns instance of bomb prefab as a gameobject
                bomb = Instantiate(bombPrefab) as GameObject;
                // at chosen location
                bomb.transform.position = bombPos;
                return;
            }
        }
        // bomb still spawns if for loop is unsuccessful (bomb will be on another bomb)
        bomb = Instantiate(bombPrefab) as GameObject;
        bomb.transform.position = bombPos;
    }

    // spawning a cluster of bombs
    void SpawnCluster() {
        // number of bombs is between 2 and 4 exclusively
        int randNum = Random.Range(2, 4);

        for (int i = 0; i <= randNum; i++) {
            SpawnBomb();
        }
    }

    // every amount of time x (determined by the random.range), it spawns a bomb -
    // with a <10% chance to spawn a "cluster" of bombs as well
    IEnumerator BombTimer() {
        while (true) {
            // waits for clamped random amount of time
            yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));
            SpawnBomb();

            if (Random.Range(0, 10) == 0) {
                SpawnCluster();
            }
        }
    }
}
