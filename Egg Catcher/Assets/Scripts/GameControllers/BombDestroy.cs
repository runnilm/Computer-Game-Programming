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
        // finding GameObjects in scene
        player = GameObject.FindGameObjectWithTag("Basket");
        playerScript = player.GetComponent<PlayerMove>();

        egg = GameObject.FindGameObjectWithTag("Egg");
        goldenEgg = GameObject.FindGameObjectWithTag("GoldenEgg");
        brokenEgg = GameObject.FindGameObjectWithTag("BrokenEgg");
    }

    private void OnTriggerEnter2D(Collider2D col) {
        // if bomb hits basket
        if (col.gameObject.tag == "Basket") {
            // and player isnt using permeability
            if (!player.GetComponent<Ability_Permeability>().isPerm) {
                // destroy the bomb
                Destroy(this.gameObject);
                // set player as "exploded" to carry out end game/loss
                playerScript.isExploded = true;
            }
        // if bomb hits ground
        } else if (col.gameObject.tag == "Terrain") {
            // lock y movement and rotation
            this.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
            this.GetComponent<Rigidbody2D>().freezeRotation = true;
            // bomb can no longer hit player
            Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), player.GetComponent<Collider2D>());

            // carry out explosion animation
            this.GetComponent<Animator>().Play("explode");
            // invoke destroy bomb function after 1 second (time for animation to happen)
            Invoke("DelayedDestroy", 1.0f);
        }
    }

    private void DelayedDestroy() {
        // destroy the bomb
        Destroy(this.gameObject);
    }
}
