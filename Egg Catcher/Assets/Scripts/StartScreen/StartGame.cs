using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// simply a class that moves the game from the start screen to the
// main game scene when any button is clicked
public class StartGame : MonoBehaviour {
    void Update() {
        if (Input.anyKeyDown) {
            SceneManager.LoadScene("Game");
        }
    }
}
