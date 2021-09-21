using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bombs : MonoBehaviour {
    public GameObject bombPrefab;
    private GameObject bomb;

    private int maxSpawnAttempts = 100;
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
        // this occurs 5 times while game is played
        if (startTime < 10.0f) {
            startTime += Time.deltaTime;
        } else if (startTime >= 10.0f) {
            minSpawnTime -= 0.2f;
            maxSpawnTime -= 0.5f;
            Debug.Log("shake");
            shake.CamShake();
            startTime = 0.0f;
        }
    }

    void SpawnBomb() {
        randX = Random.Range(-maxXVal, maxXVal);

        bombPos.y = 6.0f;
        bombPos.x = randX;

        // attempts to spawn bomb away from other bombs 'maxSpawnAttempts' times
        for (int i = 0; i <= maxSpawnAttempts; i++) {
            if (Physics2D.OverlapCircle(bombPos, 1f, layerMask) == null) {
                bomb = Instantiate(bombPrefab) as GameObject;
                bomb.transform.position = bombPos;
                return;
            }
        }
        // bomb still spawns if for loop is unsuccessful (bomb will be on another bomb)
        bomb = Instantiate(bombPrefab) as GameObject;
        bomb.transform.position = bombPos;
    }

    void SpawnCluster() {
        int randNum = Random.Range(2, 4);

        for (int i = 0; i <= randNum; i++) {
            SpawnBomb();
        }
    }

    IEnumerator BombTimer() {
        while (true) {
            yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));
            SpawnBomb();

            if (Random.Range(0, 10) == 0) {
                SpawnCluster();
            }
        }
    }
}
