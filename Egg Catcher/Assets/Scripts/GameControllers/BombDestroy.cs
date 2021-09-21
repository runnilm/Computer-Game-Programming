using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombDestroy : MonoBehaviour {
    private GameObject egg;
    private GameObject goldenEgg;
    private GameObject brokenEgg;

    private GameObject player;
    private PlayerMove playerScript;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Basket");
        playerScript = player.GetComponent<PlayerMove>();

        egg = GameObject.FindGameObjectWithTag("Egg");
        goldenEgg = GameObject.FindGameObjectWithTag("GoldenEgg");
        brokenEgg = GameObject.FindGameObjectWithTag("BrokenEgg");
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Basket") {
            if (!player.GetComponent<Ability_Permeability>().isPerm) {
                Destroy(this.gameObject);
                playerScript.isExploded = true;
            }
        } else if (col.gameObject.tag == "Terrain") {
            this.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
            this.GetComponent<Rigidbody2D>().freezeRotation = true;
            Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), player.GetComponent<Collider2D>());

            this.GetComponent<Animator>().Play("explode");
            Invoke("DelayedDestroy", 1.0f);
        }
    }

    private void DelayedDestroy() {
        Destroy(this.gameObject);
    }
}
