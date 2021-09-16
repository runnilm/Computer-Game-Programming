using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bombs : MonoBehaviour {
    public GameObject bombPrefab;
    private readonly float maxXVal = 9.0f;

    private float randX;
    private float minSpawnTime = 2.2f; // end game at 0.2
    private float maxSpawnTime = 5.0f; // end game at 0.02

    private readonly float spawnModifier = 0.5f;
    private float lastIncreased;
    private readonly float timeDelay = 10.0f;

    private GameObject player;
    private PlayerMove playerScript;

    // Start is called before the first frame update
    void Start() {
        player = GameObject.FindGameObjectWithTag("Basket");
        playerScript = player.GetComponent<PlayerMove>();

        StartCoroutine( BombTimer() );
        InvokeRepeating("SpawnCluster", 10.0f, 10.0f);
    }

    // Update is called once per frame
    void Update() {
        if (Time.time > timeDelay + lastIncreased) {
            lastIncreased = Time.time;
            if (minSpawnTime >= 0.0f) {
                minSpawnTime -= spawnModifier;
            }
            if (maxSpawnTime - 0.2f >= 0.0f) {
                maxSpawnTime -= spawnModifier + 0.33f;
            }
        }
    }

    void SpawnBomb() {
        Vector2 bombPos = transform.position;

        randX = Random.Range(-maxXVal, maxXVal);
        bombPos.x = randX;
        bombPos.y = 6.0f;

        GameObject bomb = Instantiate(bombPrefab) as GameObject;
        bomb.transform.position = bombPos;
    }

    void SpawnCluster() {
        int randNum = Random.Range(2, 4);

        for (int i = 0; i <= randNum; i++) {
            SpawnBomb();
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Basket") {
            Destroy(this.gameObject);
            playerScript.isExploded = true;
        } else if (col.gameObject.tag == "Terrain") {
            Destroy(this.gameObject);
        }
    }

    IEnumerator BombTimer() {
        while (true) {
            yield return new WaitForSeconds( Random.Range(minSpawnTime, maxSpawnTime) );
            SpawnBomb();

            if ( Random.Range(0, 10) == 0 ) {
                SpawnCluster();
            }
        }
    }
}
