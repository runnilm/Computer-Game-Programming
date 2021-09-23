using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGame : MonoBehaviour {
    void Update() {
        // if escape key is clicked at any time
        if (Input.GetKeyDown(KeyCode.Escape)) {
            // quit the game (this only applies on a built standalone game, not in the unity client)
            Application.Quit();
        }
    }
}
